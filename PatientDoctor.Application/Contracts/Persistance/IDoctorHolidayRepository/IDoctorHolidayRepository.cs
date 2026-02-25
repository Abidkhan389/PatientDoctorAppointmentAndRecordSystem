namespace PatientDoctor.Application.Contracts.Persistance.IDoctorHolidayRepository;
public   interface IDoctorHolidayRepository
{
    Task<IResponse> AddEditDoctorHoliday(AddEditDoctorHolidayCommand model);
    Task<IResponse> GetByIdDoctorHoliday(GetByIdDoctorHoliday Id);
    Task<IResponse> GetAllByProc(GetDoctorHolidayList model);
    Task<IResponse> ActiveInActive(ActiveInActiveDoctorHoliday model);
    Task<IResponse> GetDoctorHolidayByDoctorIdForPatientAppointment(GetDoctorHolidayByDoctorIdForPatientAppointment model);
}

