using MediatR;
using PatientDoctor.Application.Contracts.Persistance.IMedicine;
using PatientDoctor.Application.Contracts.Persistance.IMedicineType;
using PatientDoctor.Application.Features.Medicinetype.Commands.ActiveInActive;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Medicine.Commands.ActiveInActive
{
    public class ActiveInActiveMedicineHandler : IRequestHandler<ActiveInActiveMedicine, IResponse>
    {
        private readonly IMedicineRepository _medicineRepository;

        public ActiveInActiveMedicineHandler(IMedicineRepository medicineRepository)
        {
            _medicineRepository = medicineRepository ?? throw new ArgumentNullException(nameof(IMedicinetypeRepository));
        }
        public async Task<IResponse> Handle(ActiveInActiveMedicine request, CancellationToken cancellationToken)
        {
            return await _medicineRepository.ActiveInActive(request);
        }
    }
}
