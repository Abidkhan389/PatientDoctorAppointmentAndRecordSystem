namespace PatientDoctor.Application.Features.Identity.Commands.GoogleLogin;
public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommand, IResponse>
{
    private readonly IIdentityRepository _identityRepository;
    private readonly IGoogleTokenValidator _googleTokenValidator;

    public GoogleLoginCommandHandler(IIdentityRepository identityRepository, IGoogleTokenValidator googleTokenValidator)
    {
        _identityRepository = identityRepository?? throw new ArgumentNullException(nameof(identityRepository));
        _googleTokenValidator = googleTokenValidator;
    }
    public async Task<IResponse> Handle(GoogleLoginCommand command, CancellationToken cancellationToken)
    {
        var payload = await _googleTokenValidator.ValidateAsync(command.IdToken);
        if (payload == null) {
            throw new UnauthorizedExceptionDto($"Google Login With {command.IdToken} Invalid");
        }
        // 2. Extract Google data
        var email = payload.Email;
        var name = payload.Name;
        var providerKey = payload.Subject;
        var googleLoginResult = await _identityRepository.GoogleLoginAsync(email,name, providerKey, cancellationToken);
        if (googleLoginResult.Data == null)
            throw new NotFoundException($"User with Id {command.IdToken} not found.");
        return googleLoginResult;
    }
}
