
namespace PatientDoctor.Application.Features.Medicinetype.Quries;
    public class VM_MedicineType : ListingLogFields
    {
        public Guid Id { get; set; }
        public string TypeName { get; set; }
        public List<string>? MedicinePotency { get; set; } = new List<string>(); // Optional

        public int Status { get; set; }
    }

