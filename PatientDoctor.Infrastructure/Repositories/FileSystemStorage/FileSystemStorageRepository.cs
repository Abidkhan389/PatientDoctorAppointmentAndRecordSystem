
using PatientDoctor.Application.Contracts.Persistance.IFileStorage;
using System.Security.Cryptography;

namespace PatientDoctor.Infrastructure.Repositories.FileSystemStorage;
public class FileSystemStorageRepository : IFileStorageRepository
{
    private readonly string _rootPath;

    public FileSystemStorageRepository(string rootPath)
    {
        _rootPath = rootPath;
        // Ensure the root path exists
        if (!Directory.Exists(_rootPath))
        {
            Directory.CreateDirectory(_rootPath);
        }
    }

    public async Task<string> SaveFileAsync(Stream fileStream, string path)
    {
        var fullPath = Path.Combine(_rootPath, path);
        var directory = Path.GetDirectoryName(fullPath);
        if (directory != null && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        using (var fileStreamOut = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
        {
            await fileStream.CopyToAsync(fileStreamOut);
        }
        return path;
    }


    public async Task<Stream?> GetFileAsync(string path)
    {
        var fullPath = Path.Combine(_rootPath, path);
        return File.Exists(fullPath) ? new FileStream(fullPath, FileMode.Open, FileAccess.Read) : null;
    }

    public async Task<bool> DeleteFileAsync(string path)
    {
        var fullPath = Path.Combine(_rootPath, path);
        if (!File.Exists(fullPath)) return false;

        File.Delete(fullPath);
        return true;
    }

    public string CalculateChecksum(Stream fileStream)
    {
        using (var md5 = MD5.Create())
        {
            fileStream.Position = 0;
            var hash = md5.ComputeHash(fileStream);
            fileStream.Position = 0; // Reset position after hashing
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}

