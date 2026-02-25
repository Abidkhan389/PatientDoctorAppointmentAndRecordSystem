
namespace PatientDoctor.Application.Features.Patient.Commands.ActiveInActive;
    public class ActiveInActivePatients : IRequest<IResponse>
    {
        public Guid Id { get; set; }
        public int Status { get; set; }

    }

