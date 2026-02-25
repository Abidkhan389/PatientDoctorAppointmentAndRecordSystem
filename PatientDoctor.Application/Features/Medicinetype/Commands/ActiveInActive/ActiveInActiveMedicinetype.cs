 
namespace PatientDoctor.Application.Features.Medicinetype.Commands.ActiveInActive
{
    public class ActiveInActiveMedicinetype : IRequest<IResponse>
    {
        public Guid Id { get; set; }
        public int Status { get; set; }
    }
}
