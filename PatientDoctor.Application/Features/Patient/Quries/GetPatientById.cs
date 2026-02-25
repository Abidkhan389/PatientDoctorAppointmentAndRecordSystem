
namespace PatientDoctor.Application.Features.Patient.Quries;
    public class GetPatientById : IRequest<IResponse>
    {
        public Guid Id { get; set; }
    }

