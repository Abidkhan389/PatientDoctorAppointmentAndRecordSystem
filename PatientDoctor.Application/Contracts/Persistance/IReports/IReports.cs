
namespace PatientDoctor.Application.Contracts.Persistance.IReports;
public interface IReports
{
    Task<IResponse> GetCheckedPatientHistoryByDoctor(GetCheckedPatientHistoryByDoctorQuery model);
}

