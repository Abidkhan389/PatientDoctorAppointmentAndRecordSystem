using MediatR;
using PatientDoctor.Application.Contracts.Persistance.IDoctorAvailability;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.Doctor_Availability.Commands;
public class AddEditDoctorAvailabilityCommandHandler : IRequestHandler<AddEditDoctorAvailabilityWithUserId, IResponse>
{
    private readonly IDoctorAvailabilityRepository _doctorAvailabilityRepository;

    public AddEditDoctorAvailabilityCommandHandler(IDoctorAvailabilityRepository doctorAvailabilityRepository)
    {
        _doctorAvailabilityRepository = doctorAvailabilityRepository ?? throw new ArgumentNullException(nameof(doctorAvailabilityRepository));
    }
    public async Task<IResponse> Handle(AddEditDoctorAvailabilityWithUserId request, CancellationToken cancellationToken)
    {
        return await _doctorAvailabilityRepository.AddEditDoctorAvaibality(request);
    }
}

