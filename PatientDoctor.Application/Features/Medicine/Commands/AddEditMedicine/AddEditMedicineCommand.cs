
namespace PatientDoctor.Application.Features.Medicine.Commands.AddEditMedicine
{
    public class AddEditMedicineCommand : IRequest<IResponse>
    {
        public Guid? Id { get; set; }
        public Guid MedicineTypeId { get; set; }
        public Guid MedicineTypePotencyId { get; set; }
        public string MedicineName { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
