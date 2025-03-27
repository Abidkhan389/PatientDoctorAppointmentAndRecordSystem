
using PatientDoctor.Application.Features.Doctor_Availability.ActiveInActive;
using PatientDoctor.Application.Features.Doctor_Availability.Quries.GetAllByProc;
using PatientDoctor.Application.Features.DoctorHoliday.Command.ActiveInActive;
using PatientDoctor.Application.Features.DoctorHoliday.Command.AddEditDoctorHoliday;
using PatientDoctor.Application.Features.DoctorHoliday.Quries.GetAllByProc;
using PatientDoctor.Application.Features.DoctorHoliday.Quries.GetDoctorHolidayById;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Contracts.Persistance.IDoctorHolidayRepository;
public   interface IDoctorHolidayRepository
{
    Task<IResponse> AddEditDoctorHoliday(AddEditDoctorHolidayCommand model);
    Task<IResponse> GetByIdDoctorHoliday(GetByIdDoctorHoliday Id);
    Task<IResponse> GetAllByProc(GetDoctorHolidayList model);
    Task<IResponse> ActiveInActive(ActiveInActiveDoctorHoliday model);
}

