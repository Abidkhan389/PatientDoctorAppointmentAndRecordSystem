using PatientDoctor.Application.Features.Identity.Quries;
using PatientDoctor.Application.Features.Patient.Commands.ActiveInActive;
using PatientDoctor.Application.Features.Patient.Commands.AddEditPatient;
using PatientDoctor.Application.Features.Patient.Commands.AddPatientDescription;
using PatientDoctor.Application.Features.Patient.Quries;
using PatientDoctor.Application.Features.Patient.Quries.GetAllPatientRecordsByDoctor;
using PatientDoctor.Application.Features.Patient.Quries.GetPatientDetailForPdf;
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
        Task<IResponse> AddEditPatient(AddEditPatientWithUserId model);
        Task<IResponse> ActiveInActive(ActiveInActivePatients model);
        Task<IResponse> GetAllByProc(GetPatientList model);
        Task<IResponse> GetAllPatientAppoitmentWithDoctorProc(GetPatientAppoitmentListWithDocter model);
        Task<IResponse> AddPatientDescription(AddPatientDescriptionCommand model);
        Task<IResponse> GetPatientDescriptionById(GetPatientDescription model);
        Task<IResponse> GetPatientsRecordWithDoctorProc(GetPatientRecordListWithDoctor model);
        Task<IResponse> GetPatientDetailsForPdf(GetPatientDetailsForPdfRequest model);
    }
}
