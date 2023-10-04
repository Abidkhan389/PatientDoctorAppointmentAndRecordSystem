using MediatR;
using PatientDoctor.Application.Contracts.Persistance.Patient;
using PatientDoctor.Application.Helpers;


namespace PatientDoctor.Application.Features.Patient.Commands.ActiveInActive
{
    public class ActiveInActivepatientsHandlerQuery : IRequestHandler<ActiveInActivePatients, IResponse>
    {
        private readonly IPatientRepository _ipatientRepository;

        public ActiveInActivepatientsHandlerQuery(IPatientRepository ipatientRepository)
        {
            this._ipatientRepository = ipatientRepository ?? throw new ArgumentNullException(nameof(IPatientRepository));
        }
        public async Task<IResponse> Handle(ActiveInActivePatients request, CancellationToken cancellationToken)
        {
            var user = await _ipatientRepository.ActiveInActive(request);
            return user;
        }
    }
}
