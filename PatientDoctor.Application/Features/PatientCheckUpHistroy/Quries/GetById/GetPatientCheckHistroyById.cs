
namespace PatientDoctor.Application.Features.PatientCheckUpHistroy.Quries.GetById;
public class GetPatientCheckHistroyById : IRequest<IResponse>
{
    public Guid Id { get; set; }
}

