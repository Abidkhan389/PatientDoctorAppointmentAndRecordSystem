
using MediatR;
using PatientDoctor.Application.Contracts.Persistance.IMedicine;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.Medicine.Quries.GetDoctorMedicine;
public class GetDoctorMedicineByDoctorIdHandler : IRequestHandler<GetDoctorMedicineByDoctorId, IResponse>
{
    private readonly IMedicineRepository _medicineRepository;

    public GetDoctorMedicineByDoctorIdHandler(IMedicineRepository medicineRepository)
    {
        _medicineRepository = medicineRepository ?? throw new ArgumentNullException(nameof(medicineRepository));
    }
    public async Task<IResponse> Handle(GetDoctorMedicineByDoctorId request, CancellationToken cancellationToken)
    {
        return await _medicineRepository.GetDoctorMedicineByDoctorId(request);
    }
}

