
using PatientDoctor.Application.Features.Reports.Quries.GetCheckedPatientHistoryByDoctor;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Contracts.Persistance.IReports;
public interface IReports
{
    Task<IResponse> GetCheckedPatientHistoryByDoctor(GetCheckedPatientHistoryByDoctorQuery model);
}

