

namespace PatientDoctor.Application.Contracts.Persistance.IAdministratorRepository;
public interface IAdministratorRepository
{
    Task<IResponse> UpdateUserProfile(UserProfileCommand model);
    Task<IResponse> UserRegister(UserRegisterCommand model);
    Task<IResponse> GetUserProfileByEmailAndId(GetUserProfileByEmailAndId model);
}

