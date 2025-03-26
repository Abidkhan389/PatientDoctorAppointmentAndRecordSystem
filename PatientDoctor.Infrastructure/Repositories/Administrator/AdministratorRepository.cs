using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PatientDoctor.Application.Contracts.Persistance.IAdministratorRepository;
using PatientDoctor.Application.Contracts.Persistance.IFileRepository;
using PatientDoctor.Application.Contracts.Persistance.ISecurity;
using PatientDoctor.Application.Contracts.Security;
using PatientDoctor.Application.Features.Administrator.Commands.Register;
using PatientDoctor.Application.Features.Administrator.Commands.UserProfile;
using PatientDoctor.Application.Features.Administrator.Quries;
using PatientDoctor.Application.Helpers;
using PatientDoctor.Application.Helpers.Auth;
using PatientDoctor.domain.Entities;
using PatientDoctor.Infrastructure.Persistance;
using System.Threading.Tasks.Dataflow;

namespace PatientDoctor.Infrastructure.Repositories.Administrator;
    public class AdministratorRepository : IAdministratorRepository
    {
        private readonly DocterPatiendDbContext _context;
        private readonly ICryptoService _crypto;
        private readonly IResponse _response;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILocalAuthenticationRepository _localAuthenticationRepository;
    private readonly IFileUploader _fileUploader;

    public AdministratorRepository(DocterPatiendDbContext context, ICryptoService cryptoService,
            IResponse response, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager
            , ICryptoService crypto, ILocalAuthenticationRepository localAuthenticationRepository, IFileUploader fileUploader)
    {
        _context = context;
        _crypto = cryptoService;
        _response = response;
        _userManager = userManager;
        _roleManager = roleManager;
        _localAuthenticationRepository = localAuthenticationRepository;
        _fileUploader = fileUploader;
    }

    public async Task<IResponse> GetUserProfileByEmailAndId(GetUserProfileByEmailAndId model)
    {
        try
        {
            var user = await (from main in _userManager.Users
                              join userdetails in _context.Userdetail on main.Id equals userdetails.UserId
                              where main.Email == model.EmailOrPhoneNumber
                              select new VM_GetUserProfileByEmailAndId
                              {
                                  UserId = main.Id,
                                  FirstName = userdetails.FirstName,
                                  LastName = userdetails.LastName,
                                  ProfilePicture = main.ProfilePicture,
                                  PhoneNumber = main.PhoneNumber,
                                  EmailorPhoneNumber = main.Email
                              }).FirstOrDefaultAsync();
            if (user != null)
            {
                _response.Success = Constants.ResponseSuccess;
                _response.Message = Constants.DataUpdate;
                _response.Data = user;
            }
            else
            {
                _response.Success = Constants.ResponseFailure;
                _response.Message = Constants.NotFound;
            }
            return _response;
        }
        catch (Exception ex)
        {
            return CreateErrorResponse(ex.Message);
        }
    }

    public async Task<IResponse> UpdateUserProfile(UserProfileCommand model)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            bool ProfileImageChange = false;
            var userObj = await _userManager.FindByIdAsync(model.UserId);
            if (userObj == null)
            {
                return CreateErrorResponse("User not found.");
            }

            // Password Change Handling
            if (model.PasswordChange)
            {
                var passwordCheck = await _localAuthenticationRepository.ResolveUser(userObj.Email, model.OldPassword, false);
                if (!passwordCheck)
                {
                    return CreateErrorResponse("Old password does not match.");
                }

                var passwordSalt = _crypto.CreateSalt();
                var passwordHash = _crypto.CreateKey(passwordSalt, model.NewPassword);
                userObj.PasswordSalt = passwordSalt;
                userObj.PasswordHash = passwordHash;
            }
            if(model.File is not null)
            {

            using (var stream = model.File.OpenReadStream())
            {

                //var attachmentDto = await _fileUploader.UploadFileAsync(stream, model.File.FileName, model.EntityId ?? 0, model.EntityType, model.UserId);
                var attachmentDto = await _fileUploader.SaveFileInRootAsync(stream, model.File.FileName,model.UserId ?? "", model.EntityType);
                    model.ProfilePicture = attachmentDto;
                    ProfileImageChange = true;
                }

            }
            if (ProfileImageChange)
            {
                userObj.ProfilePicture = model.ProfilePicture;
            }

            // Update User Phone Number
            userObj.PhoneNumber = model.PhoneNumber;
            // Update User Details
            var existingUserDetails = await _context.Userdetail
                .FirstOrDefaultAsync(x => x.UserId == userObj.Id);

            if (existingUserDetails == null)
            {
                return CreateErrorResponse("User details not found.");
            }

            existingUserDetails.FirstName = model.FirstName;
            existingUserDetails.LastName = model.LastName;
            existingUserDetails.UpdatedBy = model.LoedInUserId;
            existingUserDetails.UpdatedOn = DateTime.UtcNow;
            // Save Changes

            var identityResult = await _userManager.UpdateAsync(userObj);
            if (!identityResult.Succeeded)
            {
                return CreateErrorResponse("Failed to update user profile.");
            }

            _context.Userdetail.Update(existingUserDetails);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return new Response
            {
                Success = Constants.ResponseSuccess,
                Message = Constants.DataUpdate,
                Data = userObj
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return CreateErrorResponse($"An error occurred while updating profile: {ex.Message}");
        }
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

