
namespace PatientDoctor.Application.Features.DoctorMedicine.Query;
public class DoctorMedicinehandler: IRequestHandler<DoctorMedicineById, IResponse>
{
    private readonly IDoctorMedicineRepository _doctorMedicineRepository;

    public DoctorMedicinehandler(IDoctorMedicineRepository doctorMedicineRepository)
    {
        _doctorMedicineRepository = doctorMedicineRepository ?? throw new ArgumentNullException(nameof(doctorMedicineRepository));
    }
    public async Task<IResponse> Handle(DoctorMedicineById request, CancellationToken cancellationToken)
    {
        return await _doctorMedicineRepository.GetDoctorMedicineById(request.Medicineid);
    }

}
