
namespace PatientDoctor.Application.Features.Reports.Quries.GetCheckedPatientHistoryByDoctor;
public class GetCheckedPatientHistoryByDoctorQuery :IRequest<IResponse>
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}

