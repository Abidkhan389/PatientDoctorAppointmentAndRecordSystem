using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Contracts.Persistance.ISecurity
{
    public interface ILocalAuthenticationRepository
    {
        Task<bool> ResolveUser(string username, string password, bool isSuperAdmin);
    }
}
