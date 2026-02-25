

namespace PatientDoctor.Application.Features.Doctor_Availability.ActiveInActive;
public class ActiveInActiveDoctorAvailability : IRequest<IResponse>
{
    public Guid Id { get; set; }
    public int Status { get; set; }
}

