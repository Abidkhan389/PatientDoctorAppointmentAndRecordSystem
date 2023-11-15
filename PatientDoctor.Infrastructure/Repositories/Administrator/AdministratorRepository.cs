using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PatientDoctor.Application.Contracts.Persistance.IAdministratorRepository;
using PatientDoctor.Application.Contracts.Security;
using PatientDoctor.Application.Features.Administrator.Commands.ResetPassword;
using PatientDoctor.Application.Helpers;
using PatientDoctor.domain.Entities;
using PatientDoctor.Infrastructure.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Infrastructure.Repositories.Administrator
{
    public class AdministratorRepository : IAdministratorRepository
    {
        private readonly DocterPatiendDbContext _context;
        private readonly ICryptoService _cryptoService;
        private readonly IResponse _response;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdministratorRepository(DocterPatiendDbContext context, ICryptoService cryptoService,
            IResponse response, UserManager<ApplicationUser> userManager
            , ICryptoService crypto)
        {
            this._context = context;
            this._cryptoService = cryptoService;
            this._response = response;
            this._userManager = userManager;
        }
        public async Task<IResponse> ResetPassword(ResetPasswordCommand model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            var passswordSalt = _cryptoService.CreateSalt();
            var passwordHash = _cryptoService.CreateKey(passswordSalt, model.Password);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passswordSalt;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                // User was successfully updated
                _response.Success = Constants.ResponseFailure;
                _response.Message = Constants.NotFound;
            }
            _response.Data = user;
            _response.Success = Constants.ResponseSuccess;
            _response.Message = Constants.DataUpdate;
            return _response;
        }
    }
}
