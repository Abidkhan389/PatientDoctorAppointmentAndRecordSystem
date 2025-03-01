using MediatR;
using PatientDoctor.Application.Helpers;

namespace PatientDoctor.Application.Features.Medicine.Quries.GetById
{
    public class GetMedicineById : IRequest<IResponse>
    {
        public Guid Id { get; set; }
    }
}
