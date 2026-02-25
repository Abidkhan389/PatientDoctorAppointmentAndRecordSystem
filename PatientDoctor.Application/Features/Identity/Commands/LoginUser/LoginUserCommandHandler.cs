 
namespace PatientDoctor.Application.Features.Identity.Commands.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, IResponse>
    {
        private readonly IIdentityRepository _identityRepository;

        public LoginUserCommandHandler(IIdentityRepository identityRepository)
        {
            _identityRepository = identityRepository ?? throw new ArgumentNullException(nameof(identityRepository));
        }
        public async Task<IResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _identityRepository.LoginUserAsync(request);
            return user;
        }
    }
}
