using MediatR;
using PatientDoctor.Application.Contracts.Persistance.IMedicine;
using PatientDoctor.Application.Contracts.Persistance.IMedicineType;
using PatientDoctor.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientDoctor.Application.Features.Medicine.Quries.GetById
{
    public class GetMedicineByIdHandler : IRequestHandler<GetMedicineById, IResponse>
    {
        private readonly IMedicineRepository _medicineRepository;

        public GetMedicineByIdHandler(IMedicineRepository medicineRepository)
        {
            _medicineRepository = medicineRepository ?? throw new ArgumentNullException(nameof(medicineRepository));
        }
        public async Task<IResponse> Handle(GetMedicineById request, CancellationToken cancellationToken)
        {
            return await _medicineRepository.GetMedicineById(request.Id);
        }
    }
}
