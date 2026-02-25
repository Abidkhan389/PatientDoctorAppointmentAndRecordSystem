
namespace PatientDoctor.Application.Features.Doctor_Availability.Quries.GetAllByProc;
    public class GetDoctorAvailabiltiesList : TableParam, IRequest<IResponse>
{
    public int? DayId { get; set; }
    public string? UserId { get; set; }
}

