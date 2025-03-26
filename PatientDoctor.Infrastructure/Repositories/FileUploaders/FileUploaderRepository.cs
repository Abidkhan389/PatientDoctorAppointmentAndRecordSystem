
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using PatientDoctor.Application.Contracts.Persistance.IFileRepository;
using PatientDoctor.Application.Contracts.Persistance.IFileStorage;
using PatientDoctor.Application.Helpers.General.Attachments;
using PatientDoctor.domain.Entities;
using PatientDoctor.Infrastructure.Persistance;
using PatientDoctor.Infrastructure.Utalities;

namespace PatientDoctor.Infrastructure.Repositories.FileUploaders;
public   class FileUploaderRepository(IFileStorageRepository _fileStorageRepository, DocterPatiendDbContext _context
                                      , IWebHostEnvironment _hostingEnvironment) : IFileUploader
{

    public async Task<AttachmentDto> UploadFileAsync(Stream fileStream, string fileName, string userId, string entityType, string uploadedBy)
    {
        var checksum = _fileStorageRepository.CalculateChecksum(fileStream);

        // Check for duplicate file based on checksum
        var duplicateExists = await _context.Attachments
                                .AnyAsync(a => a.HashChecksum == checksum && a.userId == userId && a.EntityType == entityType);

        if (duplicateExists)
        {
            throw new Exception("Duplicate file detected.");
        }

        // Build the file path and save the file to the file storage service
        var path = FilePathBuilder.BuildPath(entityType, userId, fileName);
        var savedPath = await _fileStorageRepository.SaveFileAsync(fileStream, path);

        // Create a new attachment entity and save it via the repository
        var attachment = new Attachments
        {
            userId = userId,
            EntityType = entityType,
            DocumentPath = savedPath,
            FileName = fileName,
            FileSize = fileStream.Length,
            UploadedBy = uploadedBy,
            HashChecksum = checksum,
            CreatedDate = DateTime.UtcNow,

        };

        

        await _context.Attachments.AddAsync(attachment);
        await _context.SaveChangesAsync();
        // Return the attachment metadata as a DTO
        return new AttachmentDto
        {
            AttachmentId = attachment.AttachmentId,
            FileName = attachment.FileName,
            DocumentPath = attachment.DocumentPath,
            CreatedDate = attachment.CreatedDate,
            UploadedBy = attachment.UploadedBy
        };
    }
    public async Task<string> SaveFileInRootAsync(Stream file, string fileName, string userId, string entityType)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File is empty or null.");

        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "UploadedFiles", entityType, userId);
        string rootImagesPath = Path.Combine(_hostingEnvironment.WebRootPath, "UploadedFiles") + Path.DirectorySeparatorChar;
        string relativePath = uploadsFolder.Substring(rootImagesPath.Length);

        // Ensure the directory exists
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        string filePath = Path.Combine(uploadsFolder, fileName);
        relativePath = Path.Combine(relativePath, fileName);

        // Check if the file already exists
        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath); // Delete the old file
        }

        // Save the new file
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        return relativePath;
    }


    public async Task<AttachmentDto?> RetrieveFileAsync(int attachmentId)
    {
        var attachment = await _context.Attachments.FindAsync(attachmentId);
        if (attachment == null || attachment.IsArchived) return null;

        return new AttachmentDto
        {
            AttachmentId = attachment.AttachmentId,
            FileName = attachment.FileName,
            DocumentPath = attachment.DocumentPath,
            CreatedDate = attachment.CreatedDate,
            UploadedBy = attachment.UploadedBy
        };
    }

    public async Task<bool> ArchiveFileAsync(int attachmentId)
    {
        var attachment = await _context.Attachments.FindAsync(attachmentId);
        if (attachment == null) return false;

        attachment.IsArchived = true;
        _context.Attachments.Update(attachment);
        return true;
    }

    public Task<Stream?> GetFileStream(string path)
    {
        return _fileStorageRepository.GetFileAsync(path);
    }
}

