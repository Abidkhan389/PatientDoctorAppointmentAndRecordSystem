using MediatR;
using PatientDoctor.Application.Helpers.General;
using PatientDoctor.Application.Helpers;
namespace PatientDoctor.Application.Features.Doctor_Availability.Quries.GetAllByProc;
    public class GetDoctorAvailabiltiesList : TableParam, IRequest<IResponse>
    {
        public int? DayId { get; set; }
    }

