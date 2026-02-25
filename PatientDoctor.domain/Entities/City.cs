
namespace PatientDoctor.domain.Entities;
    [Table("City", Schema = "Admin")]
    public class City : LogFields
    {
        public Guid Id { get; set; }

        public string CityName { get; set; }

        public Guid ProvinceID { get; set; } // Foreign Key

        public int Status { get; set; }

        // Navigation Property
        public virtual Province Province { get; set; }

    }

