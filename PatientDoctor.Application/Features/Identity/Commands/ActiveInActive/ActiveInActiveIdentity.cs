
namespace PatientDoctor.Application.Features.Identity.Commands.ActiveInActive
{
    public class ActiveInActiveIdentity : IRequest<IResponse>
    {
        public string Id { get; set; }
        public int Status { get; set; }

    }
}
