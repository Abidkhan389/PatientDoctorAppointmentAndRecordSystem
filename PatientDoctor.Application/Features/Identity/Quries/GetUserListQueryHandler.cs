
namespace PatientDoctor.Application.Features.Identity.Quries
{
    public class GetUserListQueryHandler : IRequestHandler<GetUserList, IResponse>
    {
        private readonly IIdentityRepository _identityRepository;

        public GetUserListQueryHandler(IIdentityRepository identityRepository)
        {
            this._identityRepository = identityRepository?? throw new ArgumentNullException(nameof(identityRepository));
        }
        public Task<IResponse> Handle(GetUserList request, CancellationToken cancellationToken)
        {
            var UserList = _identityRepository.GetAllByProc(request);
            return UserList;
        }
    }
}
