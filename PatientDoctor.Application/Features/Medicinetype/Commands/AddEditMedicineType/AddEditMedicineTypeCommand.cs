
namespace PatientDoctor.Application.Features.Medicinetype.Commands.AddEditMedicineType;
    public class AddEditMedicineTypeCommand : IRequest<IResponse>
    {
        public Guid? MedicineTypeId { get; set; }
        public string TypeName { get; set; }
        public List<string>? MedicinePotency { get; set; } = new List<string>(); // Optional
    }

