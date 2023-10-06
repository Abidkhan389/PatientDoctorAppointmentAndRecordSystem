using PatientDoctor.Application.Features.Identity.Commands.ActiveInActive;
using PatientDoctor.Application.Features.Identity.Commands.LoginUser;
using PatientDoctor.Application.Features.Identity.Commands.RegisterUser;
using PatientDoctor.Application.Features.Identity.Quries;
using PatientDoctor.Application.Helpers;
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
        Task<IResponse> LoginUserAsync(LoginUserCommand model);
        Task<IResponse> ActiveInActiveUser(ActiveInActiveIdentity model);
        Task<IResponse> GetAllByProc(GetUserList model);
        Task<IResponse> AddEditUser(AddEditUserWithCreatedOrUpdatedById model);
    }
}
