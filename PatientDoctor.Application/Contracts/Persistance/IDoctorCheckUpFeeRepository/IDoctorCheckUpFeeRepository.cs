
using System;
using PatientDoctor.Application.Features.DoctorCheckUpFee.Command.ActiveInActive;
using PatientDoctor.Application.Features.DoctorCheckUpFee.Command.AddEditDoctorCheckFees;
using PatientDoctor.Application.Features.DoctorCheckUpFee.Quries.GetAllByProc;
using PatientDoctor.Application.Helpers;

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

