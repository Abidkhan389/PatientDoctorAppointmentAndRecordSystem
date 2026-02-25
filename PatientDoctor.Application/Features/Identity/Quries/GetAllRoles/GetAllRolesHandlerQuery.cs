
namespace PatientDoctor.Application.Features.Identity.Quries.GetAllRoles
{
    internal class GetAllRolesHandlerQuery : IRequestHandler<GetAllRolesQuery, IResponse>
    {
        private readonly IIdentityRepository _identityRepository;

        public GetAllRolesHandlerQuery(IIdentityRepository identityRepository)
        {
            _identityRepository = identityRepository ?? throw new ArgumentNullException(nameof(identityRepository));
        }
        public async Task<IResponse> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            return await _identityRepository.GetAllRoles();
        }
    }
}
