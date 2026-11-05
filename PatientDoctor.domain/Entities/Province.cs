
namespace PatientDoctor.domain.Entities;
    [Table("Province", Schema = "Admin")]
    public class Province : LogFields
    {
        public Guid ID { get; set; }
        public string ProvinceName { get; set; }
        public int Status { get; set; }

    }

