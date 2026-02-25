using PatientDoctor.Application.Contracts.Persistance.IIdentityRepository;

namespace PatientDoctor.Application.Features.Identity.Commands.ActiveInActive
{
    public class ActiveInActiveIdentityHandlerQuery : IRequestHandler<ActiveInActiveIdentity, IResponse>
    {
        private readonly IIdentityRepository _identityRepository;

        public ActiveInActiveIdentityHandlerQuery(IIdentityRepository identityRepository)
        {
            _identityRepository = identityRepository ?? throw new ArgumentNullException(nameof(identityRepository));
        }
        public async Task<IResponse> Handle(ActiveInActiveIdentity request, CancellationToken cancellationToken)
        {
            var user = await _identityRepository.ActiveInActiveUser(request);
            return user;
        }
    }
}
