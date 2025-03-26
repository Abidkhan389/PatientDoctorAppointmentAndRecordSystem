
namespace PatientDoctor.Application.Helpers.General.Attachments;
    public class AttachmentDto
    {
        public int AttachmentId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string DocumentPath { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string UploadedBy { get; set; } = string.Empty;
    }

