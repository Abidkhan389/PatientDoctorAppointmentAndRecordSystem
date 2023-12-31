using MediatR;
using PatientDoctor.Application.Contracts.Persistance.IIdentityRepository;
using PatientDoctor.Application.Contracts.Persistance.IMedicineType;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Medicinetype.Quries
{
    public class GetMedicineTypeByIdHandlerQuery : IRequestHandler<GetMedicineTypeById, IResponse>
    {
        private readonly IMedicinetypeRepository _medicinetypeRepository;

        public GetMedicineTypeByIdHandlerQuery(IMedicinetypeRepository medicinetypeRepository)
        {
            this._medicinetypeRepository = medicinetypeRepository ?? throw new ArgumentNullException(nameof(medicinetypeRepository));
        }
        public async Task<IResponse> Handle(GetMedicineTypeById request, CancellationToken cancellationToken)
        {
            var medicineTypeById = await _medicinetypeRepository.GetMedicineTypeById(request.Id);
            return medicineTypeById;
        }
    }
}
