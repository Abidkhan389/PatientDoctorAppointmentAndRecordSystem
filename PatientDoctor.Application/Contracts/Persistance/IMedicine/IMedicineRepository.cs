using PatientDoctor.Application.Features.Medicine.Commands.ActiveInActive;
using PatientDoctor.Application.Features.Medicine.Commands.AddEditMedicine;
using PatientDoctor.Application.Features.Medicine.Quries.GetAllByProc;
using PatientDoctor.Application.Features.Medicine.Quries.GetAllMedicine;
using PatientDoctor.Application.Features.Medicine.Quries.GetDoctorMedicine;
using PatientDoctor.Application.Features.Medicine.Quries.GetDoctorMedicinePotency;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Contracts.Persistance.IMedicine
{
    public interface IMedicineRepository
    {
        Task<IResponse> GetMedicineById(Guid Id);
        Task<IResponse> GetAllMedicineTypePotency(Guid Id);
        Task<IResponse> AddEditMedicine(AddEditMedicineWithUserId model);
        Task<IResponse> ActiveInActive(ActiveInActiveMedicine model);
        Task<IResponse> GetAllByProc(GetMedicineList model);
        Task<IResponse> GetAllMedicineTypeList();
        Task<IResponse> GetAllMedicineList(GetAllMedicines model);
        Task<IResponse> GetDoctorMedicineByDoctorId(GetDoctorMedicineByDoctorId model);
        Task<IResponse> GetDoctorMedicinePotencyById(GetDoctorMedicinePotencyById model);
    }
}
