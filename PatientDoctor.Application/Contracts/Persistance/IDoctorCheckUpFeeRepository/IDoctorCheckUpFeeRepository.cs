

namespace PatientDoctor.Application.Contracts.Persistance.IDoctorCheckUpFeeRepository
{
	public interface IDoctorCheckUpFeeRepository
    {
        Task<IResponse> GetDoctorCheckUpFeeById(Guid Id);
        Task<IResponse> AddEditDoctorCheckUpFee(DoctorCheckUpFeeWithUserId model);
        Task<IResponse> ActiveInActive(ActiveInActiveDoctorCheckupFee nodal);
        Task<IResponse> GetAllByProc(GetDoctorCheckUpFeeDetailsList model);
    }
}

