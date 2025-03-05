using MediatR;
using PatientDoctor.Application.Contracts.Persistance.IDoctorAvailability;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.Doctor_Availability.Quries.GetById;
    public class GetDoctorAvailabitiesByIdHandler : IRequestHandler<GetByIdDoctorAvailabiliteis, IResponse>
    {
        private readonly IDoctorAvailabilityRepository _doctorAvailabilityRepository;

        public GetDoctorAvailabitiesByIdHandler(IDoctorAvailabilityRepository doctorAvailabilityRepository)
        {
            _doctorAvailabilityRepository = doctorAvailabilityRepository ?? throw new ArgumentNullException(nameof(doctorAvailabilityRepository));
        }
        public async Task<IResponse> Handle(GetByIdDoctorAvailabiliteis request, CancellationToken cancellationToken)
        {
            return await _doctorAvailabilityRepository.GetByIdDoctorAvaibality(request.Id);
        }
    }

