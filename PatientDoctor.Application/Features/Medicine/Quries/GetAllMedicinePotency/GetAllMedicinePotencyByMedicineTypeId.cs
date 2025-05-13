using MediatR;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.Medicine.Quries.GetAllMedicinePotency;
public class GetAllMedicinePotencyByMedicineTypeId : IRequest<IResponse>
{
    public Guid Id { get; set; }
    public GetAllMedicinePotencyByMedicineTypeId(Guid id)
    {
        this.Id = id;
    }

}

