using PatientDoctor.Application.Features.Administrator.Commands.Register;
using PatientDoctor.Application.Features.Administrator.Commands.UserProfile;
using PatientDoctor.Application.Features.Administrator.Quries;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Contracts.Persistance.IAdministratorRepository;
public interface IAdministratorRepository
{
    Task<IResponse> UpdateUserProfile(UserProfileCommand model);
    Task<IResponse> UserRegister(UserRegisterCommand model);
    Task<IResponse> GetUserProfileByEmailAndId(GetUserProfileByEmailAndId model);
}

