using MediatR;
using PatientDoctor.Application.Contracts.Persistance.Patient;
using PatientDoctor.Application.Helpers;


namespace PatientDoctor.Application.Features.Patient.Commands.PatientDiscount;
    public class PatientDiscountHandlerQuery : IRequestHandler<PatientDiscount, IResponse>
    {
        private readonly IPatientRepository _ipatientRepository;

        public PatientDiscountHandlerQuery(IPatientRepository ipatientRepository)
        {
            this._ipatientRepository = ipatientRepository ?? throw new ArgumentNullException(nameof(IPatientRepository));
        }
        public async Task<IResponse> Handle(PatientDiscount request, CancellationToken cancellationToken)
        {            
            var user = await _ipatientRepository.patientDiscount(request);
            return user;
        }
    }

