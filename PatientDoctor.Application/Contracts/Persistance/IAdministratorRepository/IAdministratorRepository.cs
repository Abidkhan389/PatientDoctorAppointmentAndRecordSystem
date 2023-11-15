using PatientDoctor.Application.Features.Administrator.Commands.ResetPassword;
using PatientDoctor.Application.Features.Patient.Commands.ActiveInActive;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Contracts.Persistance.IAdministratorRepository
{
    public interface IAdministratorRepository
    {
        Task<IResponse> ResetPassword(ResetPasswordCommand model);
    }
}
