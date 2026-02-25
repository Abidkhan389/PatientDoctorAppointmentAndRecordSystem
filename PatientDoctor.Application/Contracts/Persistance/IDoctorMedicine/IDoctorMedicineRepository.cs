namespace PatientDoctor.Application.Contracts.Persistance.IDoctorMedicine;
    public interface IDoctorMedicineRepository
    {
        Task<IResponse> GetDoctorMedicineById(Guid MedicineId);
        Task<IResponse> AddEditDoctorMedicine(AddEditDoctorMedicineCommand model);

    }

