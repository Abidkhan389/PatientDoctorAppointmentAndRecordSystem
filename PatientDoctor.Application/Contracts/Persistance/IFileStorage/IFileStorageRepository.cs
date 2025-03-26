
namespace PatientDoctor.Application.Contracts.Persistance.IFileStorage;
 public   interface IFileStorageRepository
{
    Task<string> SaveFileAsync(Stream fileStream, string path);
    Task<Stream?> GetFileAsync(string path);
    Task<bool> DeleteFileAsync(string path);
    string CalculateChecksum(Stream fileStream);
}

