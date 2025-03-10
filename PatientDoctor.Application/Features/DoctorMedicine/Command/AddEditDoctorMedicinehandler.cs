
using MediatR;
using PatientDoctor.Application.Contracts.Persistance.IDoctorMedicine;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.DoctorMedicine.Command;
public class AddEditDoctorMedicinehandler : IRequestHandler<AddEditDoctorMedicineCommand, IResponse>
{
    private readonly IDoctorMedicineRepository _doctorMedicineRepository;

    public AddEditDoctorMedicinehandler(IDoctorMedicineRepository doctorMedicineRepository)
    {
        _doctorMedicineRepository = doctorMedicineRepository ?? throw new ArgumentNullException(nameof(doctorMedicineRepository));
    }
    public async Task<IResponse> Handle(AddEditDoctorMedicineCommand request, CancellationToken cancellationToken)
    {
        return await _doctorMedicineRepository.AddEditDoctorMedicine(request);
    }
}

