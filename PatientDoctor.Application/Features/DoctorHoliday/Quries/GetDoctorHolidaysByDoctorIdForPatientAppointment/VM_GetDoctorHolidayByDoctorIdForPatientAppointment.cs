namespace PatientDoctor.Application.Features.DoctorHoliday.Quries.GetDoctorHolidaysByDoctorIdForPatientAppointment;
public    class VM_GetDoctorHolidayByDoctorIdForPatientAppointment
{
    public DateTime FromDate { get; set; } // Nullable for weekly off

    public DateTime ToDate { get; set; } // Nullable for weekly off
}

