using MediatR;
using PatientDoctor.Application.Contracts.Persistance.IAdministratorRepository;
using PatientDoctor.Application.Helpers;
namespace PatientDoctor.Application.Features.Administrator.Commands.UserProfile;
public class UserProfileCommandHandler(IAdministratorRepository _administratorRepository) : IRequestHandler<UserProfileCommand, IResponse>
{
    private readonly IAdministratorRepository _repo =
        _administratorRepository ?? throw new ArgumentNullException(nameof(_administratorRepository));
    public async Task<IResponse> Handle(UserProfileCommand request, CancellationToken cancellationToken)
    {
        return await _administratorRepository.UpdateUserProfile(request);
    }
}

