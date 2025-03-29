
using MediatR;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.DoctorHoliday.Command.AddEditDoctorHoliday;
public   class AddEditDoctorHolidayCommand :IRequest<IResponse>
{
    public Guid? DoctorHolidayId { get; set; }
    public string DoctorId { get; set; } // Foreign Key to Doctor Table

    public DateTime FromDate { get; set; } // Nullable for weekly off

    public DateTime ToDate { get; set; } // Nullable for weekly off

    //public int DayOfWeek { get; set; } // Sunday, Monday, etc.

    public string? Reason { get; set; } // Optional reason for leave
    public string? LogedInUserId { get; set; }
    public int Status { get; set; }
}

