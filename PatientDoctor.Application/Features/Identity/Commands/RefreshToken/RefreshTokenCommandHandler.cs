
using MediatR;
using PatientDoctor.Application.Contracts.Persistance.IIdentityRepository;
using PatientDoctor.Application.Helpers;
using PatientDoctor.Application.Helpers.General.Dtos.Auth;

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
    public async Task<IResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var storedToken =
            await _repo.GetRefreshTokenAsync(request.RefreshToken);
        

        if (storedToken == null ||
            storedToken.Expires < DateTime.UtcNow)
        {
            _response.Success = false;
            _response.Message = "Invalid or expired refresh token";
            return _response;
        }

        await _repo.RevokeRefreshTokenAsync(storedToken.Token);

        var tokens =
            await _repo.GenerateTokensAsync(storedToken.UserId);

        _response.Success = true;
        _response.Data = tokens;
        _response.Message = "Token refreshed successfully";

        return _response;

    }
}

