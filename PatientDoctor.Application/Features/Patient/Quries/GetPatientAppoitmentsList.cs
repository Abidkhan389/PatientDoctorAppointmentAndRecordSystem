
namespace PatientDoctor.Application.Features.Patient.Quries;
    public class GetPatientAppoitmentsList : TableParam, IRequest<IResponse>
    {
        public string? PatientName { get; set; }
        public string? Cnic { get; set; }
        public string? MobileNumber { get; set; }
        public string? City { get; set; }
        public DateTime Todeydatetime { get; set; }
    }

