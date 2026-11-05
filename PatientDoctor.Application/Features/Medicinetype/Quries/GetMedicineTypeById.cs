 
namespace PatientDoctor.Application.Features.Medicinetype.Quries;
    public class GetMedicineTypeById : IRequest<IResponse>
    {
        public Guid Id { get; set; }
        public GetMedicineTypeById(Guid Id)
        {
            this.Id = Id;
        }

    }

