
namespace PatientDoctor.Application.Features.Patient.Quries;
    public class GetPatientDescription : IRequest<IResponse>
    {
        public Guid PatientId { get; set; }
    }

