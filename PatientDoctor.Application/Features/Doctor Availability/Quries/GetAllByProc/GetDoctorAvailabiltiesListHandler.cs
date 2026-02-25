
namespace PatientDoctor.Application.Features.Doctor_Availability.Quries.GetAllByProc;
    public class GetDoctorAvailabiltiesListHandler : IRequestHandler<GetDoctorAvailabiltiesList, IResponse>
    {
        private readonly IDoctorAvailabilityRepository _doctorAvailabilityRepository;

        public GetDoctorAvailabiltiesListHandler(IDoctorAvailabilityRepository doctorAvailabilityRepository)
        {
            _doctorAvailabilityRepository = doctorAvailabilityRepository ?? throw new ArgumentNullException(nameof(doctorAvailabilityRepository));
        }
        public async Task<IResponse> Handle(GetDoctorAvailabiltiesList request, CancellationToken cancellationToken)
        {
            return await _doctorAvailabilityRepository.GetAllByProc(request);
        }
    }

