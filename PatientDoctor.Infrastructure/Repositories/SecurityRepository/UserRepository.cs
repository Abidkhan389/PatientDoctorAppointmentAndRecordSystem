using Microsoft.AspNetCore.Identity;
using PatientDoctor.Application.Contracts.Persistance.ISecurity;
using PatientDoctor.Application.Contracts.Security;
using PatientDoctor.domain.Entities;
using PatientDoctor.Infrastructure.Persistance;

namespace PatientDoctor.Infrastructure.Repositories.SecurityRepository;
    //public interface IUserRepository : ILocalAuthenticationRepository, IEntityRepository<ApplicationUser> { }
    public class UserRepository : ILocalAuthenticationRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DocterPatiendDbContext _context;
        private readonly ICryptoService _crypto;

        public UserRepository(UserManager<ApplicationUser> userManager, DocterPatiendDbContext context,
            ICryptoService crypto)
        {
            this._userManager = userManager;
            this._context = context;
            this._crypto = crypto;
        }
        public async Task<bool> ResolveUser(string Email, string password, bool isSuperAdmin)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user != null)
            {
                if (this._crypto.CheckKey(user.PasswordHash, user.PasswordSalt, password))
                {
                    return true;
                }
                return false;
            }
            return false;
        } 
    }

