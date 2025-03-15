using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PatientDoctor.Application.Contracts.Persistance.IIdentityRepository;
using PatientDoctor.Application.Contracts.Security;
using PatientDoctor.Application.Helpers;
using PatientDoctor.domain.Entities;
using System.Text;
using PatientDoctor.Infrastructure.Persistance;
using PatientDoctor.Application.Features.Identity.Commands.LoginUser;
using PatientDoctor.Application.Features.Identity.Commands.RegisterUser;
using PatientDoctor.Application.Features.Identity.Commands.ActiveInActive;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using PatientDoctor.Application.Features.Identity.Quries;
using Microsoft.EntityFrameworkCore;
using PatientDoctor.Infrastructure.Repositories.GeneralServices;
using System.Data;
using PatientDoctor.Application.Features.Identity.Quries.GetDoctorFee.GetDoctorFeeById;

namespace PatientDoctor.Infrastructure.Repositories.Identity;
    public class IdentityRepository : IIdentityRepository
    {
        private readonly DocterPatiendDbContext _context;
        private readonly IMapper _mapper;
        private readonly IResponse _response;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICountResponse _countResp;
        private readonly ICryptoService _crypto;
        public IdentityRepository(DocterPatiendDbContext context, IMapper mapper,
            IResponse response, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IConfiguration configuration,
            RoleManager<IdentityRole> roleManager, ICountResponse countResp
            , ICryptoService crypto)
        {
            this._context = context;
            this._mapper = mapper;
            this._response = response;
            this._userManager = userManager;
            //this._hostingEnvironment = hostingEnvironment;
            this._signInManager = signInManager;
            this._configuration = configuration;
            this._roleManager = roleManager;
            this._countResp = countResp;
            this._crypto = crypto;

        }
        public async Task<IResponse> ActiveInActiveUser(ActiveInActiveIdentity model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null )
            {
                _response.Success = Constants.ResponseFailure;
                _response.Message = Constants.NotFound.Replace("{data}", "user");
                return _response;
            }
            user.Status = model.Status;
            // Save the changes to the user
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                // User was successfully updated
                _response.Success = Constants.ResponseSuccess;
                _response.Message = Constants.DataUpdate;
            }
            else
            {
                // Handle the case where user update failed
                _response.Success = Constants.ResponseFailure;
                _response.Message = "User update failed";
            }
            return _response;
        }

        public async Task<IResponse> GetAllByProc(GetUserList model)
        {
            try
            {

            
            model.Sort = model.Sort == null || model.Sort == "" ? "FirstName" : model.Sort;

                var userList = (from user in _userManager.Users
                                join userdetails in _context.Userdetail on user.Id equals userdetails.UserId
                                where (user.Status == model.Status || model.Status == null)
                                && (string.IsNullOrEmpty(model.FirstName) || EF.Functions.Like(userdetails.FirstName, $"%{model.FirstName}%"))
                                && (string.IsNullOrEmpty(model.LastName) || EF.Functions.Like(userdetails.LastName, $"%{model.LastName}%"))
                                && (string.IsNullOrEmpty(model.Email) || EF.Functions.Like(user.Email, $"%{model.Email}%"))
                                && (string.IsNullOrEmpty(model.City) || EF.Functions.Like(userdetails.City, $"%{model.City}%"))
                                && (string.IsNullOrEmpty(model.Cnic) || EF.Functions.Like(userdetails.Cnic, $"%{model.Cnic}%"))
                                && (string.IsNullOrEmpty(model.MobileNumber) || EF.Functions.Like(user.PhoneNumber, $"%{model.MobileNumber}%"))
                                select new VM_Users
                                {
                                    Email = user.Email,
                                    UserId = user.Id,
                                    MobileNumber = user.PhoneNumber,
                                    FirstName = userdetails.FirstName,
                                    LastName = userdetails.LastName,
                                    Cnic = userdetails.Cnic,
                                    City = userdetails.City,
                                    RoleName = user.RoleName,
                                    Status = user.Status,
                                }).AsQueryable();


                var count = userList.Count();
            var sorted= await HelperStatic.OrderBy(userList, model.SortEx,model.OrderEx=="desc").Skip(model.Start).Take(model.LimitEx).ToListAsync();
            foreach (var item in sorted)
            {
                item.TotalCount = count;
                item.SerialNo = ++model.Start;
            }
            _countResp.DataList = sorted;
            _countResp.TotalCount = sorted.Count > 0 ? sorted.First().TotalCount : 0;
            _response.Success = Constants.ResponseSuccess;
            _response.Message = Constants.GetData;
            _response.Data = _countResp;
                return _response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _response.Success = Constants.ResponseFailure;
                _response.Message = Constants.NotFound;
                return _response;
            }
        }

        public async Task<IResponse> GetUserById(GetUserById model)
        {
            var user = await (from main in _userManager.Users
                              join userdetail in _context.Userdetail on main.Id equals userdetail.UserId
                              join userRoles in _context.UserRoles on main.Id equals userRoles.UserId
                              join roles in _context.Roles on userRoles.RoleId equals roles.Id // ✅ Join with Roles Table
                              where (main.Status == 1 && main.Id == model.id)
                              select new VM_User
                              {
                                  UserId = main.Id,
                                  MobileNumber = main.PhoneNumber,
                                  Status = main.Status,
                                  Email = main.Email,
                                  FirstName = userdetail.FirstName,
                                  LastName = userdetail.LastName,
                                  City = userdetail.City,
                                  Cnic = userdetail.Cnic,
                                  RoleId = roles.Id, // ✅ RoleId from Roles table
                                  RoleName = roles.Name, // ✅ Fetch Role Name
                                  Fee = userdetail.Fee ?? 0 // ✅ Fetch Role Name
                              }).FirstOrDefaultAsync();



            //var user = await _userManager.FindByIdAsync(UserId.ToString());
            if (user == null)
            {
                _response.Success = Constants.ResponseFailure;
                _response.Message = Constants.NotFound.Replace("{data}", "user");
                return _response;
            }
            // Get user roles
           // var roles = await _userManager.GetRolesAsync(new ApplicationUser { Id = user.UserId });
            // Assign roles to the user object
            //user.Roles = roles.ToList();
            _response.Data = user;
            _response.Success = Constants.ResponseSuccess;
            _response.Message = Constants.DataSaved;
            return _response;
        }
        public async Task<IResponse> LoginUserAsync(LoginUserCommand model)
        {
           
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
               // _logger.LogWarning($"User not found for email: {model.Email}");
               // return Unauthorized(new { message = "Invalid email or password" });
            }

            if (user != null && this._crypto.CheckKey(user.PasswordHash, user.PasswordSalt, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id), // Use ClaimTypes.NameIdentifier for user Id
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.UtcNow.AddHours(5),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
                var userdetailsObj = await _context.Userdetail.Where(x => x.UserId == user.Id).FirstOrDefaultAsync();
                
                var authenticatedUser = new 
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    User = user,
                    FirstName = userdetailsObj.FirstName,
                    LastName=userdetailsObj.LastName,
                    Roles = userRoles,
                };
               

                _response.Data = authenticatedUser;
                _response.Message = Constants.Login;
                _response.Success = Constants.ResponseSuccess;
            }
            else
            {
                _response.Message = Constants.IncorrectUsernamePassword;
                _response.Success = Constants.ResponseFailure;
            }

            return _response;
        }

        public async Task<IResponse> AddEditUser(AddEditUserWithCreatedOrUpdatedById model)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                if (string.IsNullOrEmpty(model.addEditUsermodel.Id))
                {
                    var existUser = await _userManager.FindByEmailAsync(model.addEditUsermodel.Email);
                    if (existUser != null)
                    {
                        _response.Message = Constants.Exists.Replace("{data}", "{existUser.email");
                        _response.Success = Constants.ResponseFailure;
                        return _response;
                    }
                    // Create a new ApplicationUser
                    var user = new ApplicationUser
                    {
                        IsSuperAdmin = false,
                        PhoneNumber = model.addEditUsermodel.MobileNumber,
                        Email = model.addEditUsermodel.Email,
                        UserName = model.addEditUsermodel.Email,
                        Status = 1,
                    };
                    // salt and hast the password
                    var salt = _crypto.CreateSalt();
                    user.PasswordSalt = salt;
                    user.PasswordHash = _crypto.CreateKey(salt, model.addEditUsermodel.Password);
                   
                    var exitrole = await _roleManager.FindByIdAsync(model.addEditUsermodel.RoleId);
                    if(exitrole == null)
                    {
                        //Handle the case where the role does not exist.
                            _response.Message = Constants.NotFound.Replace("{data}", "{role}");
                            _response.Success = Constants.ResponseFailure;
                            return _response;
                    }
                    user.RoleName = exitrole.Name;
                    var result = await _userManager.CreateAsync(user);
                    if (!result.Succeeded)
                    {
                        _response.Message = "User Creation is failed";
                        _response.Success = Constants.ResponseFailure;
                        return _response;
                    }
                    Userdetail userdetail = new Userdetail(model, user);

                    await _context.Userdetail.AddAsync(userdetail);
                    var resultrole = await _userManager.AddToRoleAsync(user, exitrole.Name);
                    if (!resultrole.Succeeded)
                    {
                        // Handle the case where role assignment failed
                        _response.Success = Constants.ResponseFailure;
                        _response.Message = "Role assignment failed.";
                        return _response;
                    }
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    _response.Success = Constants.ResponseSuccess;
                    _response.Message = Constants.Register;
                    return _response;
                }
                else
                {
                    var existUser = await _userManager.FindByIdAsync(model.addEditUsermodel.Id);
                    if (existUser == null)
                    {
                        _response.Message = Constants.NotFound.Replace("{data}", "{User}");
                        _response.Success = Constants.ResponseFailure;
                        return _response;
                    }
                    if (existUser.Email != model.addEditUsermodel.Email)
                    {
                        var CheckEmail = await _userManager.FindByEmailAsync(model.addEditUsermodel.Email);
                        if (existUser != null)
                        {
                            _response.Message = Constants.UniqueEmail;
                            _response.Success = Constants.ResponseFailure;
                            return _response;
                        }
                    }
                    //update existing user
                    existUser.PhoneNumber= model.addEditUsermodel.MobileNumber;
                    existUser.Email = model.addEditUsermodel.Email;
                    existUser.UserName = model.addEditUsermodel.Email;
                    var existinguserdetails = await _context.Userdetail.Where(x => x.UserId == existUser.Id).FirstOrDefaultAsync();
                    if (existinguserdetails == null)
                    {
                        _response.Message = Constants.NotFound.Replace("{data}", "{UserDetails}");
                        _response.Success = Constants.ResponseFailure;
                        return _response;
                    }
                    // update existing user details
                    existinguserdetails.Cnic=model.addEditUsermodel.Cnic;
                    existinguserdetails.City=model.addEditUsermodel.City;
                    existinguserdetails.UpdatedBy = model.UserId;
                    existinguserdetails.FirstName=model.addEditUsermodel.FirstName;
                    existinguserdetails.LastName = model.addEditUsermodel.LastName;
                    existinguserdetails.UpdatedOn= DateTime.Now;
                    existinguserdetails.Fee = model.addEditUsermodel.Fee;
                    //update user
                    var result = await _userManager.UpdateAsync(existUser);
                    if (!result.Succeeded)
                    {
                        _response.Message = Constants.NotFound.Replace("{data}", "{userObj}");
                        _response.Success = Constants.ResponseFailure;
                        return _response;
                    }
                    //get the user roles for handle the duplicate roles
                    var userRole = await _userManager.GetRolesAsync(existUser);
                    if(userRole.Count != 0)
                    {
                        // Remove user from all roles
                        var removeRolesResult = await _userManager.RemoveFromRolesAsync(existUser, userRole);

                        if (!removeRolesResult.Succeeded)
                        {
                            // Handle the case where role removal failed
                            _response.Success = Constants.ResponseFailure;
                            _response.Message = "Role removal failed.";
                            return _response;
                        }
                    }
                    var exitrole = await _roleManager.FindByIdAsync(model.addEditUsermodel.RoleId);

                    // Add the user to the new role
                    var addToRoleResult = await _userManager.AddToRoleAsync(existUser, exitrole.Name);

                    if (!addToRoleResult.Succeeded)
                    {
                        // Handle the case where role assignment failed
                        _response.Success = Constants.ResponseFailure;
                        _response.Message = "Role assignment failed.";
                        return _response;
                    }
                    await _userManager.UpdateAsync(existUser);
                          _context.Userdetail.Update(existinguserdetails);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    _response.Success = Constants.ResponseSuccess;
                    _response.Message = Constants.DataUpdate;
                    return _response;
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // Rollback the transaction in case of an exception
                return CreateErrorResponse(ex.Message);
            }
        }

        public async Task<IResponse> GetAllRoles()
        {
            List<GetAllRoles> getAllRoles = new List<GetAllRoles>();
            var roles = _roleManager.Roles;
            if (roles == null)
            {
                _response.Success = Constants.ResponseFailure;
                _response.Message = Constants.NotFound.Replace("{data}", "{roles}");
            }
            foreach(var role in roles)
            {
                GetAllRoles obj = new GetAllRoles();
                obj.Name = role.Name;
                obj.Id = role.Id;   
                getAllRoles.Add(obj);
            }
            _response.Data = getAllRoles;
            _response.Success = Constants.ResponseSuccess;
            _response.Message = Constants.GetData;
            return _response;

        }

        public async Task<IResponse> GetAllDoctors()
        {
        //var users= await _context.Users.Where(x=> x.RoleName=="DOCTOR" && x.Status==1).Select (y=> new {y.Id,y.UserName}).ToListAsync();
        var users = await (from u in _context.Users
                           join ud in _context.Userdetail on u.Id equals ud.UserId
                           where u.RoleName == "DOCTOR" && u.Status == 1
                           select new
                           {
                               u.Id,
                               u.UserName,
                               ud.Fee
                           }).ToListAsync();
        _response.Data=users;
            _response.Success = Constants.ResponseSuccess;
            _response.Message = Constants.GetData;
            return _response;
        }

        private IResponse CreateErrorResponse(string message)
        {
            return new Response { Success = Constants.ResponseFailure, Message = message };
        }

    public async Task<IResponse> GetDoctorFee(GetDoctorFee model)
    {
        var doctorFee = await (from main in _userManager.Users
                               join userdetails in _context.Userdetail on main.Id equals userdetails.UserId
                               where main.Id == model.DoctorId
                               select new { Fee = userdetails.Fee }
                              ).FirstOrDefaultAsync();
        if (doctorFee == null)
        {
            _response.Success = Constants.ResponseFailure;
            _response.Message = Constants.NotFound.Replace("{data}", "user");
        }
        else
        {
            _response.Success = Constants.ResponseSuccess;
            _response.Message = Constants.GetData;
            _response.Data = doctorFee;
        }
        return _response;
    }
}

