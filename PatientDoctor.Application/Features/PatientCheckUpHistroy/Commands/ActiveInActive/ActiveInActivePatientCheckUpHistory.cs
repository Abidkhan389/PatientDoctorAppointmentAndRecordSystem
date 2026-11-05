
namespace PatientDoctor.Application.Features.PatientCheckUpHistroy.Commands.ActiveInActive;
 public   class ActiveInActivePatientCheckUpHistory : IRequest<IResponse>
{
    public Guid Id { get; set; }
    public int Status { get; set; }

}

