
using PatientDoctor.Application.Helpers.General.Attachments;

namespace PatientDoctor.Application.Contracts.Persistance.IFileRepository;
    public interface IFileUploader
    {
        Task<AttachmentDto> UploadFileAsync(Stream fileStream, string fileName, string userId, string uploadType, string uploadedBy);
        Task<AttachmentDto?> RetrieveFileAsync(int attachmentId);
        Task<bool> ArchiveFileAsync(int attachmentId);
        Task<Stream?> GetFileStream(string path);
        Task<string> SaveFileInRootAsync(Stream file, string fileName, string userId, string entityType);

    }

