
using MediatR;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.PatientCheckUpHistroy.Quries.GetByIdForHistoryShow_OfPateint;
   public class GetByIdForHistoryShow_OfPateintById : IRequest<IResponse>
{
    public Guid Id { get; set; }

}

