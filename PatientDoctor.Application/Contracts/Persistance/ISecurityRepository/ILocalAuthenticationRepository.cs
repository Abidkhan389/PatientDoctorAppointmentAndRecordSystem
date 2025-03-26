
namespace PatientDoctor.Application.Contracts.Persistance.ISecurity;
public interface ILocalAuthenticationRepository
{
    Task<bool> ResolveUser(string username, string password, bool isSuperAdmin);
   
}

