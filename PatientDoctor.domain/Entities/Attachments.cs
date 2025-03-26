using PatientDoctor.domain.Entities.Public;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatientDoctor.domain.Entities
{
    [Table("Attachments", Schema = "Admin")]
    public class Attachments
    {
        [Key]
        public int AttachmentId { get; set; }
        public string userId { get; set; }
        public string EntityType { get; set; } = string.Empty;
        public string DocumentPath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string UploadedBy { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public bool IsArchived { get; set; }
        public string HashChecksum { get; set; } = string.Empty;
    }
}

