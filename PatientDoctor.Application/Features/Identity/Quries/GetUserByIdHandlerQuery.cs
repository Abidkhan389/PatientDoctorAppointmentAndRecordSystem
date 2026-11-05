
namespace PatientDoctor.Application.Features.Identity.Quries
{
    public class GetUserByIdHandlerQuery : IRequestHandler<GetUserById, IResponse>
    {
        private readonly IIdentityRepository _identityRepository;

        public GetUserByIdHandlerQuery(IIdentityRepository identityRepository)
        {
            this._identityRepository = identityRepository?? throw new ArgumentNullException(nameof(identityRepository));
        }
        public async Task<IResponse> Handle(GetUserById request, CancellationToken cancellationToken)
        {
            var user = await _identityRepository.GetUserById(request);
            if (user.Data == null)
                throw new NotFoundException($"User with Id {request.id} not found.");
            return user;
        }
    }
}
