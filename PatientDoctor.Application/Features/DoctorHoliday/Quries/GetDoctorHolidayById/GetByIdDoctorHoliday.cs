
namespace PatientDoctor.Application.Features.DoctorHoliday.Quries.GetDoctorHolidayById;
public class GetByIdDoctorHoliday : IRequest<IResponse>
{
    public Guid DoctorHolidayId { get; set; }
}

