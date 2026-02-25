
namespace PatientDoctor.Application.Contracts.Persistance.IMedicineType
{
    public interface IMedicinetypeRepository
    {
        Task<IResponse> GetMedicineTypeById(Guid Id);
        Task<IResponse> AddEditMedicineType(AddEditMedicineTypeWithUserId model);
        Task<IResponse> ActiveInActive(ActiveInActiveMedicinetype model);
        Task<IResponse> GetAllByProc(GetMedicineTypeList model);
        Task<IResponse> GetAllMedicineTypeWithIdAndName();
    }
}
