using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PatientDoctor.Application.Contracts.Persistance.IAdministratorRepository;
using PatientDoctor.Application.Contracts.Security;
using PatientDoctor.Application.Features.Administrator.Commands.Register;
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
        private readonly ICryptoService _crypto;
        private readonly IResponse _response;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdministratorRepository(DocterPatiendDbContext context, ICryptoService cryptoService,
            IResponse response, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager
            , ICryptoService crypto)
        {
            this._context = context;
            _crypto = cryptoService;
            this._response = response;
            this._userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IResponse> ResetPassword(ResetPasswordCommand model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            var passswordSalt = _crypto.CreateSalt();
            var passwordHash = _crypto.CreateKey(passswordSalt, model.Password);
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

        public async Task<IResponse> UserRegister(UserRegisterCommand model)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var userObj = await _userManager.FindByEmailAsync(model.Uname);
                if (userObj is not null)
                {
                    _response.Success = Constants.ResponseFailure;
                    _response.Message = Constants.UniqueUserDetails;
                    return _response;
                }
                // Create a new ApplicationUser
                var user = new ApplicationUser
                {
                    IsSuperAdmin = false,
                    Email = model.Uname,
                    Status = 1,
                };

                // Salt and hash the password
                var salt = _crypto.CreateSalt();
                user.PasswordSalt = salt;
                user.PasswordHash = _crypto.CreateKey(salt, model.password);

                // Check if the "USER" role exists
                var existingRole = await _roleManager.FindByNameAsync("USER");
                if (existingRole == null)
                {
                    // Create the "USER" role if it doesn't exist
                    var newRole = new IdentityRole { Name = "USER" };
                    var roleResult = await _roleManager.CreateAsync(newRole);
                    if (!roleResult.Succeeded)
                    {
                        _response.Success = Constants.ResponseFailure;
                        _response.Message = "Failed to create USER role.";
                        return _response;
                    }

                    existingRole = newRole; // Assign newly created role
                }
                user.RoleName = existingRole.Name;
                user.UserName = model.Uname;
                // Create the user
                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    _response.Success = Constants.ResponseFailure;
                    _response.Message = Constants.RegisterFailed;
                    return _response;
                }
                Userdetail userdetail = new Userdetail();
                userdetail.UserId = user.Id;
                userdetail.UserDetailId = Guid.NewGuid();
                userdetail.CreatedOn = DateTime.UtcNow;
                userdetail.City = "";
                userdetail.Cnic = "";
                userdetail.FirstName = "";
                userdetail.LastName = "";

                await _context.Userdetail.AddAsync(userdetail);
                // Assign the role to the user
                var roleAssignResult = await _userManager.AddToRoleAsync(user, existingRole.Name);
                if (!roleAssignResult.Succeeded)
                {
                    _response.Success = Constants.ResponseFailure;
                    _response.Message = "Failed to assign USER role.";
                    return _response;
                }
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                _response.Data = user;
                _response.Success = Constants.ResponseSuccess;
                _response.Message = Constants.Register;

                return _response;
            }
            catch (DbUpdateException dbEx)
            {
                await transaction.RollbackAsync().ConfigureAwait(false);
                return CreateErrorResponse("Database error: " + dbEx.Message);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // Rollback the transaction in case of an exception
                return CreateErrorResponse(ex.Message);
            }
        }
        private IResponse CreateErrorResponse(string message)
        {
            return new Response { Success = Constants.ResponseFailure, Message = message };
        }
    }
}
