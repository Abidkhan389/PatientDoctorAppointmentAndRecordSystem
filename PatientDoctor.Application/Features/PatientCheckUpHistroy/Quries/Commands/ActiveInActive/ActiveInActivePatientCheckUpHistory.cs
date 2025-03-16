
using MediatR;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.PatientCheckUpHistroy.Quries.Commands.ActiveInActive;
 public   class ActiveInActivePatientCheckUpHistory : IRequest<IResponse>
{
    public Guid Id { get; set; }
    public int Status { get; set; }

}

