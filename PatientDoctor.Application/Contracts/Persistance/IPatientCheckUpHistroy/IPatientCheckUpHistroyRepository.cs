using PatientDoctor.Application.Features.PatientCheckUpHistroy.Commands.ActiveInActive;
using PatientDoctor.Application.Features.PatientCheckUpHistroy.Quries.GetAll;
using PatientDoctor.Application.Features.PatientCheckUpHistroy.Quries.GetById;
using PatientDoctor.Application.Features.PatientCheckUpHistroy.Quries.GetByIdForHistoryShow_OfPateint;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Contracts.Persistance.IPatientCheckUpHistroy;
public  interface IPatientCheckUpHistroyRepository
{
    Task<IResponse> GetAllByProc(GetAllPatientCheckUpHistroyByDoctor model);
    Task<IResponse> GetPatientCheckHistroyById(GetPatientCheckHistroyById model);
    Task<IResponse> GetPatientCheckHistroyByIdForHistory(GetByIdForHistoryShow_OfPateintById model);
    Task<IResponse> ActiveInActive(ActiveInActivePatientCheckUpHistory model);
    Task<IResponse> FetchPatientTrackingNumberByPatientId(Guid PatientId);
}

