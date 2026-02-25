
namespace PatientDoctor.Application.Features.Patient.Quries.GetDoctorSlots;
public class GetDoctorTimeSlotByDayIdAndDoctorIdHandler : IRequestHandler<GetDoctorTimeSlotsByDayIdAndDoctorId, IResponse>
{
    private readonly IPatientRepository _patientRepository;
    public GetDoctorTimeSlotByDayIdAndDoctorIdHandler(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
    }
    public async Task<IResponse> Handle(GetDoctorTimeSlotsByDayIdAndDoctorId request, CancellationToken cancellationToken)
    {
        return await _patientRepository.GetDoctorAppointmentsSlotsOfDay(request);
    }
}

