
namespace PatientDoctor.Application.Features.Medicine.Commands.ActiveInActive
{
    public class ActiveInActiveMedicine : IRequest<IResponse>
    {
        public Guid Id { get; set; }
        public int Status { get; set; }
    }
}
