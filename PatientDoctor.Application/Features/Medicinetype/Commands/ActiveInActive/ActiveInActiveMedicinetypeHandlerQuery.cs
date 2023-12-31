using MediatR;
using PatientDoctor.Application.Contracts.Persistance.IMedicineType;
using PatientDoctor.Application.Contracts.Persistance.Patient;
using PatientDoctor.Application.Features.Patient.Commands.ActiveInActive;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Medicinetype.Commands.ActiveInActive
{
    public class ActiveInActiveMedicinetypeHandlerQuery : IRequestHandler<ActiveInActiveMedicinetype, IResponse>
    {
        private readonly IMedicinetypeRepository _medicinetypeRepository;

        public ActiveInActiveMedicinetypeHandlerQuery(IMedicinetypeRepository medicinetypeRepository)
        {
            this._medicinetypeRepository = medicinetypeRepository  ?? throw new ArgumentNullException(nameof(IMedicinetypeRepository));
        }
        public async Task<IResponse> Handle(ActiveInActiveMedicinetype request, CancellationToken cancellationToken)
        {
            var medicinetype = await _medicinetypeRepository.ActiveInActive(request);
            return medicinetype;
        }
    }
}
