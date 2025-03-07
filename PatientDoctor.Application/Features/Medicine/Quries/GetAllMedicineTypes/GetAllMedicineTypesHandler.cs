using MediatR;
using PatientDoctor.Application.Contracts.Persistance.IMedicine;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.Medicine.Quries.GetAllMedicineTypes;
public class GetAllMedicineTypesHandler : IRequestHandler<GetAllMedicineTypes, IResponse>
{
    private readonly IMedicineRepository _medicineRepository;

    public GetAllMedicineTypesHandler(IMedicineRepository medicineRepository)
    {
        _medicineRepository = medicineRepository ?? throw new ArgumentNullException(nameof(medicineRepository));
    }
    public async Task<IResponse> Handle(GetAllMedicineTypes request, CancellationToken cancellationToken)
    {
        return await _medicineRepository.GetAllMedicineTypeList();
    }
}

