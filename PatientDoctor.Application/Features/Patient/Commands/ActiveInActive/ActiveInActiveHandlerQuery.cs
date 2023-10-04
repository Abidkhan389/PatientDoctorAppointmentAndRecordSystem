using MediatR;
using PatientDoctor.Application.Contracts.Persistance.IIdentityRepository;
using PatientDoctor.Application.Contracts.Persistance.Patient;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Patient.Commands.ActiveInActive
{
    public class ActiveInActiveHandlerQuery : IRequestHandler<ActiveInActive, IResponse>
    {
        private readonly IPatientRepository _ipatientRepository;

        public ActiveInActiveHandlerQuery(IPatientRepository ipatientRepository)
        {
            this._ipatientRepository = ipatientRepository ?? throw new ArgumentNullException(nameof(IPatientRepository));
        }
        public async Task<IResponse> Handle(ActiveInActive request, CancellationToken cancellationToken)
        {
            var user = await _ipatientRepository.ActiveInActive(request);
            return user;
        }
    }
}
