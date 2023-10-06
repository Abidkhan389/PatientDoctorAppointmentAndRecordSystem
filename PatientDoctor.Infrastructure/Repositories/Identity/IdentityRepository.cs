using AutoMapper;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PatientDoctor.Application.Contracts.Persistance.IIdentityRepository;
using PatientDoctor.Application.Contracts.Security;
using PatientDoctor.Application.Helpers;
using PatientDoctor.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PatientDoctor.Infrastructure.Persistance;
using PatientDoctor.Application.Features.Identity.Commands.LoginUser;
using PatientDoctor.Application.Features.Identity.Commands.RegisterUser;
using PatientDoctor.Application.Features.Identity.Commands.ActiveInActive;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using PatientDoctor.Infrastructure.Utalities;
using PatientDoctor.Application.Features.Identity.Quries;
using Microsoft.EntityFrameworkCore;
using PatientDoctor.Infrastructure.Repositories.GeneralServices;

namespace PatientDoctor.Infrastructure.Repositories.Identity
{
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
            var user = await _userManager.FindByIdAsync(model.Id.ToString());
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
            model.Sort = model.Sort == null || model.Sort == "" ? "UserName" : model.Sort;

            var userList = await(from user in _userManager.Users
                           join userdetails in _context.Userdetail on user.Id equals userdetails.UserId
                           where (user.Status == model.Status || model.Status == null)
                           &&(EF.Functions.ILike(user.UserName,$"%{model.UserName}%") || string.IsNullOrEmpty(model.UserName))
                           &&(EF.Functions.ILike(user.Email,$"%{model.Email}%") || string.IsNullOrEmpty(model.Email))
                           &&(EF.Functions.ILike(userdetails.City,$"%{model.City}%") || string.IsNullOrEmpty(model.City))
                           &&(EF.Functions.ILike(userdetails.Cnic,$"%{model.Cnic}%") || string.IsNullOrEmpty(model.Cnic))
                           &&(EF.Functions.ILike(user.PhoneNumber,$"%{model.MobileNumber}%") || string.IsNullOrEmpty(model.MobileNumber))
                           select new
                           {
                               User=user,
                               Userdetail=userdetails
                           }).ToListAsync();

            var tasks = userList.Select(async item => new VM_Users
            {
                UserId=item.User.Id,
                MobileNumber=item.User.PhoneNumber,
                FullName=item.User.UserName,
                Status=item.User.Status,
                Email=item.User.Email,
                City=item.Userdetail.City,
                Roles = (List<string>)await _userManager.GetRolesAsync(item.User) // fetching user roles using _usermanager.getrolesasync for each user
            }).ToList();
            var vmUserList = (await Task.WhenAll(tasks)).AsQueryable();
            var count= vmUserList.Count();
            var sorted= await HelperStatic.OrderBy(vmUserList,model.SortEx,model.OrderEx=="desc").Skip(model.Start).Take(model.LimitEx).ToListAsync();
            foreach (var item in sorted)
            {
                item.TotalCount = count;
                item.SerialNo = ++model.Start;
            }
            _countResp.DataList = sorted;
            _countResp.TotalCount = sorted.Count > 0 ? sorted.First().TotalCount : 0;
            _response.Success = Constants.ResponseSuccess;
            _response.Data = _countResp;
            return _response;
        }

        public async Task<IResponse> GetUserById(GetUserById Id)
        {
            var user = await(from main in _userManager.Users
                             join userdetail in _context.Userdetail on main.Id equals userdetail.UserId
                             where (main.Status == 1 && main.Id == Id.ToString())
                             select new VM_Users
                             {
                                 UserId = main.Id,
                                 MobileNumber = main.PhoneNumber,
                                 Status = main.Status,
                                 Email = main.Email,
                                 FullName = main.UserName,
                                 City=userdetail.City,
                             }).FirstOrDefaultAsync();
            //var user = await _userManager.FindByIdAsync(UserId.ToString());
            if (user == null)
            {
                _response.Success = Constants.ResponseFailure;
                _response.Message = Constants.NotFound.Replace("{data}", "user");
                return _response;
            }
            // Get user roles
            var roles = await _userManager.GetRolesAsync(new ApplicationUser { Id = user.UserId });
            // Assign roles to the user object
            user.Roles = roles.ToList();
            _response.Data = user;
            _response.Success = Constants.ResponseSuccess;
            _response.Message = Constants.DataSaved;
            return _response;
        }

        public async Task<IResponse> LoginUserAsync(LoginUserCommand model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null && this._crypto.CheckKey(user.PasswordHash, user.PasswordSalt, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
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

                var authenticatedUser = new AuthenticatedUser
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    User = user
                };

                _response.Data = authenticatedUser;
                _response.Success = Constants.ResponseSuccess;
            }
            else
            {
                _response.Message = Constants.NotFound.Replace("{data}", "{user}");
                _response.Success = Constants.ResponseFailure;
            }

            return _response;
        }

        public async Task<IResponse> AddEditUser(AddEditUserWithCreatedOrUpdatedById model)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                if (model.addEditUsermodel.Id == null)
                {
                    var existUser = await _userManager.FindByEmailAsync(model.addEditUsermodel.Email);
                    if (existUser != null)
                    {
                        _response.Message = Constants.Exists.Replace("{data}", "{User}");
                        _response.Success = Constants.ResponseFailure;
                        return _response;
                    }
                    var UserRoles = new List<IdentityRole>();
                    foreach (var roleid in model.addEditUsermodel.RoleIds)
                    {
                        var role = await _roleManager.FindByIdAsync(roleid);
                        if (role != null)
                        {
                            UserRoles.Add(role);
                        }
                        else
                        {
                            // Handle the case where the role does not exist.
                            _response.Message = Constants.NotFound.Replace("{data}", "{role}");
                            _response.Success = Constants.ResponseFailure;
                            return _response;
                        }
                    }
                    // Create a new ApplicationUser
                    var user = new ApplicationUser
                    {
                        IsSuperAdmin = false,
                        PhoneNumber = model.addEditUsermodel.MobileNumber,
                        Email = model.addEditUsermodel.Email,
                        UserName = model.addEditUsermodel.FirstName + model.addEditUsermodel.LastName,
                        Status = 1,
                    };
                    // salt and hast the password
                    var salt = _crypto.CreateSalt();
                    user.PasswordSalt = salt;
                    user.PasswordHash = _crypto.CreateKey(salt, model.addEditUsermodel.Password);
                    var result = await _userManager.CreateAsync(user);
                    if (!result.Succeeded)
                    {
                        _response.Message = Constants.NotFound.Replace("{data}", "{userObj}");
                        _response.Success = Constants.ResponseFailure;
                        return _response;
                    }
                    Userdetail userdetail = new Userdetail(model);
                    userdetail.Initialize(user);
                    await _context.Userdetail.AddAsync(userdetail);
                    await _context.SaveChangesAsync();
                    //get the user roles for handle the duplicate roles
                    var userRoles = await _userManager.GetRolesAsync(user);

                    // Determine roles to be added or updated
                    var rolesToAddOrUpdate = model.addEditUsermodel.RoleIds.Except(userRoles).ToList();

                    var rolesResult = await _userManager.AddToRolesAsync(user, rolesToAddOrUpdate);
                    if (!rolesResult.Succeeded)
                    {
                        // Handle the case where role assignment failed
                        _response.Success = Constants.ResponseFailure;
                        _response.Message = "Role assignment failed.";
                        return _response;
                    }
                    await transaction.CommitAsync();
                    _response.Success = Constants.ResponseSuccess;
                    _response.Message = Constants.DataSaved;
                    return _response;
                }
                else
                {
                    var existUser = await _userManager.FindByIdAsync(model.addEditUsermodel.Id);
                    if (existUser != null)
                    {
                        _response.Message = Constants.NotFound.Replace("{data}", "{User}");
                        _response.Success = Constants.ResponseFailure;
                        return _response;
                    }
                    var UserRoles = new List<IdentityRole>();
                    foreach (var roleid in model.addEditUsermodel.RoleIds)
                    {
                        var role = await _roleManager.FindByIdAsync(roleid);
                        if (role != null)
                        {
                            UserRoles.Add(role);
                        }
                        else
                        {
                            // Handle the case where the role does not exist.
                            _response.Message = Constants.NotFound.Replace("{data}", "{role}");
                            _response.Success = Constants.ResponseFailure;
                            return _response;
                        }
                    }
                    //update existing user
                    existUser.PhoneNumber= model.addEditUsermodel.MobileNumber;
                    existUser.Email = model.addEditUsermodel.Email;
                    existUser.UserName = model.addEditUsermodel.FirstName + model.addEditUsermodel.LastName;
                    var existinguserdetails= await _context.Userdetail.FindAsync(existUser.Id);
                    if (existinguserdetails != null)
                    {
                        _response.Message = Constants.NotFound.Replace("{data}", "{UserDetails}");
                        _response.Success = Constants.ResponseFailure;
                        return _response;
                    }
                    // update existing user details
                    existinguserdetails.Cnic=model.addEditUsermodel.Cnic;
                    existinguserdetails.City=model.addEditUsermodel.City;
                    existinguserdetails.UpdatedBy = model.UserId;
                    existinguserdetails.UpdatedOn= DateTime.UtcNow;
                    //update user
                    var result = await _userManager.UpdateAsync(existUser);
                    if (!result.Succeeded)
                    {
                        _response.Message = Constants.NotFound.Replace("{data}", "{userObj}");
                        _response.Success = Constants.ResponseFailure;
                        return _response;
                    }
                    //get the user roles for handle the duplicate roles
                    var userRoles = await _userManager.GetRolesAsync(existUser);

                    // Determine roles to be added or updated
                    var rolesToAddOrUpdate = model.addEditUsermodel.RoleIds.Except(userRoles).ToList();

                    var rolesResult = await _userManager.AddToRolesAsync(existUser, rolesToAddOrUpdate);
                    if (!rolesResult.Succeeded)
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
                _response.Message = ex.Message;
                _response.Success = Constants.ResponseFailure;
                return _response;
            }
        }
    }
}
