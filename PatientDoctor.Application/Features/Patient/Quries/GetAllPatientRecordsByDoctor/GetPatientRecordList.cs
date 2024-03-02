using MediatR;
using PatientDoctor.Application.Helpers.General;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.Patient.Quries.GetAllPatientRecordsByDoctor
{
    public class GetPatientRecordList : TableParam, IRequest<IResponse>
    {
        public string? DoctorName { get; set; }
        public string? PatientName { get; set; }
        public string? PatientCnic { get; set; }
        public DateTime? PatientCheckUpDateFrom {  get; set; }
        public DateTime? PatientCheckUpDateTo { get; set; }
    }
}
