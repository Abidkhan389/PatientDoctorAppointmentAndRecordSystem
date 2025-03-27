using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PatientDoctor.domain.Entities.Public;
using PatientDoctor.Application.Features.DoctorHoliday.Command.AddEditDoctorHoliday;

namespace PatientDoctor.domain.Entities;
[Table("DoctorHoliday", Schema = "Admin")]
public class DoctorHolidays : LogFields
{
    [Key]
    public Guid DoctorHolidayId { get; set; }
    public string DoctorId { get; set; } // Foreign Key to Doctor Table
    public DateTime FromDate { get; set; } // Nullable for weekly off

    public DateTime ToDate { get; set; } // Nullable for weekly off

    public int DayOfWeek { get; set; } // Sunday, Monday, etc.

    public string? Reason { get; set; } // Optional reason for leave

    public int Status { get; set; }
    // Navigation property (if you have a Doctor entity)
    [ForeignKey("UserId")]
    public virtual ApplicationUser? Doctor { get; set; }
    public DoctorHolidays()
    {
        
    }
    public DoctorHolidays(AddEditDoctorHolidayCommand model)
    {
        DoctorHolidayId = Guid.NewGuid();
        DoctorId = model.DoctorId;
        FromDate = model.FromDate;
        ToDate = model.ToDate;
        DayOfWeek = model.DayOfWeek;
        Reason = model.Reason;
    }
}

