
namespace PatientDoctor.Application.Features.Identity.Commands.RefreshToken;
public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, IResponse>
{
    private readonly IIdentityRepository _repo;
    private readonly IResponse _response;

    public RefreshTokenCommandHandler(IIdentityRepository repo,IResponse response)
    {
        _repo = repo;
        _response = response;
    }
    public async Task<IResponse> Handle(
       RefreshTokenCommand request,
       CancellationToken cancellationToken)
    {
        // 1️⃣ Validate refresh token (hash + expiry + flags)
        var storedToken =
            await _repo.GetRefreshTokenAsync(request.RefreshToken);

        if (storedToken == null)
        {
            var userId =
           await _repo.GetUserIdByRefreshTokenAsync(request.RefreshToken);

            if (userId != null)
            {
                await _repo.RevokeAllTokensAsync(userId);
            }

            _response.Success = false;
            _response.Message = "Refresh token reuse detected";
            return _response;
        }

        // 2️⃣ Extra expiry safety (already DB level, but defensive)
        if (storedToken.Expires <= DateTime.UtcNow)
        {
            _response.Success = false;
            _response.Message = "Refresh token expired";
            return _response;
        }

        // 3️⃣ Mark token as USED (rotation)
        await _repo.MarkTokenAsUsedAsync(request.RefreshToken);

        // 4️⃣ Generate new access + refresh tokens
        var tokens =
            await _repo.GenerateTokensAsync(storedToken.UserId);

        _response.Success = true;
        _response.Data = tokens;
        _response.Message = "Token refreshed successfully";

        return _response;
    }
}

