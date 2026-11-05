
namespace PatientDoctor.Application.Features.Doctor_Availability.ActiveInActive;
public class ActiveInActiveDoctorAvailabilityHandler : IRequestHandler<ActiveInActiveDoctorAvailability, IResponse>
{
    private readonly IDoctorAvailabilityRepository _doctorAvailabilityRepository;

    public ActiveInActiveDoctorAvailabilityHandler(IDoctorAvailabilityRepository doctorAvailabilityRepository)
    {
        _doctorAvailabilityRepository = doctorAvailabilityRepository ?? throw new ArgumentNullException(nameof(doctorAvailabilityRepository));
    }
    public async Task<IResponse> Handle(ActiveInActiveDoctorAvailability request, CancellationToken cancellationToken)
    {
        return await _doctorAvailabilityRepository.ActiveInActive(request);
    }
}

