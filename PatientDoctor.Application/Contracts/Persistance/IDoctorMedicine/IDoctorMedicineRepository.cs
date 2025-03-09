using PatientDoctor.Application.Features.DoctorMedicine.Command;
using PatientDoctor.Application.Features.Medicine.Commands.AddEditMedicine;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Contracts.Persistance.IDoctorMedicine;
    public interface IDoctorMedicineRepository
    {
        Task<IResponse> GetDoctorMedicineById(Guid MedicineId);
        Task<IResponse> AddEditDoctorMedicine(AddEditDoctorMedicineCommand model);

    }

