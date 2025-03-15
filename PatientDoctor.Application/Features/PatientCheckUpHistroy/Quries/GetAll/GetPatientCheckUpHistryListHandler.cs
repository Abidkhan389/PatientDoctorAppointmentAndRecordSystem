using MediatR;
using PatientDoctor.Application.Contracts.Persistance.IPatientCheckUpHistroy;
using PatientDoctor.Application.Helpers;
namespace PatientDoctor.Application.Features.PatientCheckUpHistroy.Quries.GetAll;
public class GetPatientCheckUpHistryListHandler(IPatientCheckUpHistroyRepository _patientCheckUpHistroyRepository) : IRequestHandler<GetAllPatientCheckUpHistroyByDoctor, IResponse>
{
    private readonly IPatientCheckUpHistroyRepository _repo =
        _patientCheckUpHistroyRepository ?? throw new ArgumentNullException(nameof(_patientCheckUpHistroyRepository));
    public async Task<IResponse> Handle(GetAllPatientCheckUpHistroyByDoctor request, CancellationToken cancellationToken)
    {
        return await _patientCheckUpHistroyRepository.GetAllByProc(request);
    }
}

