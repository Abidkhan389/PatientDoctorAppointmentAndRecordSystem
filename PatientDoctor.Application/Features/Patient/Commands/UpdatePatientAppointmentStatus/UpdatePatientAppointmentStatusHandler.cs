
namespace PatientDoctor.Application.Features.Patient.Commands.UpdatePatientAppointmentStatus;
public class UpdatePatientAppointmentStatusHandler(IPatientRepository _ipatientRepository) : IRequestHandler<UpdatePatientAppointmentStatusCommand, IResponse>
{
    public async Task<IResponse> Handle(UpdatePatientAppointmentStatusCommand request, CancellationToken cancellationToken)
    {
        return await _ipatientRepository.UpdatePatientAppointmentStatus(request, cancellationToken);
    }
}

