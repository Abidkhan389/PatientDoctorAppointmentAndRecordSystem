using MediatR;
using PatientDoctor.Application.Contracts.Persistance.IPatientCheckUpHistroy;
using PatientDoctor.Application.Features.PatientCheckUpHistroy.Quries.GetById;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.PatientCheckUpHistroy.Quries.GetByIdForHistoryShow_OfPateint;
    public class GetByIdForHistoryShow_OfPateintByIdHandler(IPatientCheckUpHistroyRepository _patientCheckUpHistroyRepository)
     : IRequestHandler<GetByIdForHistoryShow_OfPateintById, IResponse>
    {
        public async Task<IResponse> Handle(GetByIdForHistoryShow_OfPateintById request, CancellationToken cancellationToken)
        {
        return await _patientCheckUpHistroyRepository.GetPatientCheckHistroyByIdForHistory(request);
        }
    }

