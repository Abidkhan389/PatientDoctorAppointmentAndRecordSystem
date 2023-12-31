using MediatR;
using PatientDoctor.Application.Contracts.Persistance.IMedicineType;
using PatientDoctor.Application.Contracts.Persistance.Patient;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Medicinetype.Commands.AddEditMedicineType
{
    internal class AddEditMedicinetypeCommandHandler : IRequestHandler<AddEditMedicineTypeWithUserId, IResponse>
    {
        private readonly IMedicinetypeRepository _medicinetypeRepository;

        public AddEditMedicinetypeCommandHandler(IMedicinetypeRepository medicinetypeRepository)
        {
            this._medicinetypeRepository = medicinetypeRepository  ?? throw new ArgumentNullException(nameof(medicinetypeRepository));
        }
        public async Task<IResponse> Handle(AddEditMedicineTypeWithUserId request, CancellationToken cancellationToken)
        {
            var medicinetype= await _medicinetypeRepository.AddEditMedicineType(request);
            return medicinetype;
        }
    }
}
