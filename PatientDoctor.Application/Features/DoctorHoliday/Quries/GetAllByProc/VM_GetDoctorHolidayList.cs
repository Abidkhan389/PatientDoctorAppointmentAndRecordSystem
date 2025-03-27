
using PatientDoctor.Application.Helpers.General;

namespace PatientDoctor.Application.Features.DoctorHoliday.Quries.GetAllByProc;
public   class VM_GetDoctorHolidayList: ListingLogFields
{
    public Guid DoctorHolidayId { get; set; }
    public string DoctorId { get; set; } // Foreign Key to Doctor Table
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime FromDate { get; set; } // Nullable for weekly off

    public DateTime ToDate { get; set; } // Nullable for weekly off

    public int DayOfWeek { get; set; } // Sunday, Monday, etc.

    public string Reason { get; set; } // Optional reason for leave
}

