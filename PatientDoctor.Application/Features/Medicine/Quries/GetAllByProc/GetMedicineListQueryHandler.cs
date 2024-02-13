using MediatR;
using PatientDoctor.Application.Contracts.Persistance.IMedicine;
using PatientDoctor.Application.Contracts.Persistance.IMedicineType;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.Medicine.Quries.GetAllByProc
{
    public class GetMedicineListQueryHandler : IRequestHandler<GetMedicineList, IResponse>
    {
        private readonly IMedicineRepository _medicineRepository;

        public GetMedicineListQueryHandler(IMedicineRepository medicineRepository)
        {
            _medicineRepository = medicineRepository ?? throw new ArgumentNullException(nameof(medicineRepository));
        }
        public async Task<IResponse> Handle(GetMedicineList request, CancellationToken cancellationToken)
        {
            return await _medicineRepository.GetAllByProc(request);
        }
    }
}
