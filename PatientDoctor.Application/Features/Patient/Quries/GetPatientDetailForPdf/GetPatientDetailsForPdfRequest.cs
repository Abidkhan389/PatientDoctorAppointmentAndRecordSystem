using MediatR;
using PatientDoctor.Application.Helpers;
namespace PatientDoctor.Application.Features.Patient.Quries.GetPatientDetailForPdf
{
    public class GetPatientDetailsForPdfRequest :IRequest<IResponse>
    {
        public string DoctorId { get; set; }
        public Guid PatientId { get; set;}
    }
}
