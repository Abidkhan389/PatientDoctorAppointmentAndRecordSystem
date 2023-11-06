using PatientDoctor.Application.Features.Identity.Quries;
using PatientDoctor.Application.Features.Patient.Commands.ActiveInActive;
using PatientDoctor.Application.Features.Patient.Commands.AddEditPatient;
using PatientDoctor.Application.Features.Patient.Quries;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Contracts.Persistance.Patient
{
    public interface IPatientRepository
    {
        Task<IResponse> GetPatientById(GetPatientById Id);
        Task<IResponse> AddEditPatient(AddEditPatientCommand model);
        Task<IResponse> ActiveInActive(ActiveInActivePatients model);
        Task<IResponse> GetAllByProc(GetPatientList model);
    }
}
