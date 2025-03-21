
using MediatR;
using PatientDoctor.Application.Contracts.Persistance.IMedicine;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.Medicine.Quries.GetDoctorMedicinePotency;
public class GetDoctorMedicinePotencyByIdHandler : IRequestHandler<GetDoctorMedicinePotencyById, IResponse>
{
    private readonly IMedicineRepository _medicineRepository;

    public GetDoctorMedicinePotencyByIdHandler(IMedicineRepository medicineRepository)
    {
        _medicineRepository = medicineRepository ?? throw new ArgumentNullException(nameof(medicineRepository));
    }
    public async Task<IResponse> Handle(GetDoctorMedicinePotencyById request, CancellationToken cancellationToken)
    {
        return await _medicineRepository.GetDoctorMedicinePotencyById(request);
    }
}

