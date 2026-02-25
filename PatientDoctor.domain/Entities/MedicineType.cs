
namespace PatientDoctor.domain.Entities;
    [Table("MedicineType", Schema = "Admin")]
    public class MedicineType : LogFields
    {
        public Guid Id { get; set; }

        public string TypeName { get; set; }

        public int Status { get; set; }

        // Navigation Property
        public virtual ICollection<MedicinePotency> MedicinePotencies { get; set; } = new List<MedicinePotency>();

        public MedicineType() { }

        public MedicineType(AddEditMedicineTypeCommand model, Guid userId)
        {
            Id = Guid.NewGuid();
            Status = 1;
            TypeName = model.TypeName;
            CreatedBy = userId;
            CreatedOn = DateTime.UtcNow;

            //// Initialize MedicinePotencies correctly
            //MedicinePotencies = model.MedicinePotency
            //    .Select(p => new MedicinePotency(p, Id, userId)) // ✅ Extracting 'Potency' property
            //    .ToList();
        }
    }


