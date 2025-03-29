
using MediatR;
using PatientDoctor.Application.Helpers;
using PatientDoctor.Application.Helpers.General;

namespace PatientDoctor.Application.Features.DoctorHoliday.Quries.GetAllByProc;
public  class GetDoctorHolidayList : TableParam, IRequest<IResponse>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? FromDate { get; set; } // Nullable for weekly off
    public DateTime? ToDate { get; set; } // Nullable for weekly off
    //public int? DayOfWeek { get; set; } // Sunday, Monday, etc.
    public string? LogedInDoctorId { get; set; }

}

