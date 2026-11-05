
namespace PatientDoctor.Application.Features.DoctorHoliday.Command.ActiveInActive;
public   class ActiveInActiveDoctorHoliday :IRequest<IResponse>
{
    public Guid Id { get; set; }
    public int Status { get; set; }
}

