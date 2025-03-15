using PatientDoctor.Application.Features.Patient.Commands.ActiveInActive;
using PatientDoctor.Application.Features.PatientCheckUpHistroy.Quries.Commands.ActiveInActive;
using PatientDoctor.Application.Features.PatientCheckUpHistroy.Quries.GetAll;
using PatientDoctor.Application.Features.PatientCheckUpHistroy.Quries.GetById;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Contracts.Persistance.IPatientCheckUpHistroy;
public  interface IPatientCheckUpHistroyRepository
{
    Task<IResponse> GetAllByProc(GetAllPatientCheckUpHistroyByDoctor model);
    Task<IResponse> GetPatientCheckHistroyById(GetPatientCheckHistroyById model);
    Task<IResponse> ActiveInActive(ActiveInActivePatientCheckUpHistory model);
}

