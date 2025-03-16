
using MediatR;
using PatientDoctor.Application.Contracts.Persistance.IPatientCheckUpHistroy;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.PatientCheckUpHistroy.Quries.Commands.ActiveInActive;
public class ActiveInActivePatientCheckUpHistoryHndler(IPatientCheckUpHistroyRepository _patientCheckUpHistroyRepository) : IRequestHandler<ActiveInActivePatientCheckUpHistory, IResponse>
{
    private readonly IPatientCheckUpHistroyRepository _repo =
      _patientCheckUpHistroyRepository ?? throw new ArgumentNullException(nameof(_patientCheckUpHistroyRepository));
    public async Task<IResponse> Handle(ActiveInActivePatientCheckUpHistory request, CancellationToken cancellationToken)
    {
        return await _patientCheckUpHistroyRepository.ActiveInActive(request);
    }
}

