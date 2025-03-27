using System;
using System.Collections.Generic;

namespace PatientDoctor.Application.Features.DoctorHoliday.Quries.GetDoctorHolidayById;
public   class VM_GetByIdDoctorHoliday
{
    public Guid DoctorHolidayId { get; set; }
    public string DoctorId { get; set; } // Foreign Key to Doctor Table

    public DateTime FromDate { get; set; } // Nullable for weekly off

    public DateTime ToDate { get; set; } // Nullable for weekly off

    public int DayOfWeek { get; set; } // Sunday, Monday, etc.

    public string Reason { get; set; } // Optional reason for leave
}

