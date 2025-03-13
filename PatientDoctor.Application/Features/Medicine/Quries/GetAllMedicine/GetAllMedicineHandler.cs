using MediatR;
using PatientDoctor.Application.Contracts.Persistance.IMedicine;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.Medicine.Quries.GetAllMedicine;
public class GetAllMedicineHandler : IRequestHandler<GetAllMedicines, IResponse>
{
    private readonly IMedicineRepository _medicineRepository;

    public GetAllMedicineHandler(IMedicineRepository medicineRepository)
    {
        _medicineRepository = medicineRepository ?? throw new ArgumentNullException(nameof(medicineRepository));
    }
    public async Task<IResponse> Handle(GetAllMedicines request, CancellationToken cancellationToken)
    {
        return await _medicineRepository.GetAllMedicineList(request);
    }
}

