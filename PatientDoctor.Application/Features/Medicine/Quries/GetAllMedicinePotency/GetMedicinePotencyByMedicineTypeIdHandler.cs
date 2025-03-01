using MediatR;
using PatientDoctor.Application.Contracts.Persistance.IMedicine;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.Medicine.Quries.GetAllMedicinePotency;
public class GetMedicinePotencyByMedicineTypeIdHandlerIRequestHandler : IRequestHandler<GetAllMedicinePotencyByMedicineTypeId, IResponse>
{
    private readonly IMedicineRepository _medicineRepository;

    public GetMedicinePotencyByMedicineTypeIdHandlerIRequestHandler(IMedicineRepository medicineRepository)
    {
        _medicineRepository = medicineRepository ?? throw new ArgumentNullException(nameof(medicineRepository));
    }
    public async Task<IResponse> Handle(GetAllMedicinePotencyByMedicineTypeId request, CancellationToken cancellationToken)
    {
        return await _medicineRepository.GetAllMedicineTypePotency(request.Id);
    }
}

