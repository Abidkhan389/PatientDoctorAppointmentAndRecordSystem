using PatientDoctor.Application.Features.Identity.Commands.ActiveInActive;
using PatientDoctor.Application.Features.Identity.Commands.LoginUser;
using PatientDoctor.Application.Features.Identity.Commands.RegisterUser;
using PatientDoctor.Application.Features.Identity.Quries;
using PatientDoctor.Application.Features.Identity.Quries.GetDoctorFee.GetDoctorFeeById;
using PatientDoctor.Application.Helpers;
using PatientDoctor.Application.Helpers.General.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Contracts.Persistance.IIdentityRepository
{
    public interface IIdentityRepository
    {
        // Task<IResponse> GetAllProducts(GetProductList model);
        Task<IResponse> GetUserById(GetUserById Id);
        Task<IResponse> GetDoctorFee(GetDoctorFee Id);
        Task<IResponse> LoginUserAsync(LoginUserCommand model);
        Task<IResponse> ActiveInActiveUser(ActiveInActiveIdentity model);
        Task<IResponse> GetAllByProc(GetUserList model);
        Task<IResponse> GetAllRoles();
        Task<IResponse> GetAllDoctors();
        Task<IResponse> AddEditUser(AddEditUserWithCreatedOrUpdatedById model);

        Task<UserRefreshTokenDto> GetRefreshTokenAsync(string token);
        Task RevokeRefreshTokenAsync(string token);

        Task<AuthTokenResultDto> GenerateTokensAsync(string userId);
    }
}
