using MediatR;
using PatientDoctor.Application.Contracts.Persistance.IPatientCheckUpHistroy;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.PatientCheckUpHistroy.Quries.GetById;
public class GetPatientCheckHistroyByIdHandler(IPatientCheckUpHistroyRepository _patientCheckUpHistroyRepository)
    : IRequestHandler<GetPatientCheckHistroyById, IResponse>
{
    private readonly IPatientCheckUpHistroyRepository _repo =
        _patientCheckUpHistroyRepository ?? throw new ArgumentNullException(nameof(_patientCheckUpHistroyRepository));

    public Task<IResponse> Handle(GetPatientCheckHistroyById request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}


