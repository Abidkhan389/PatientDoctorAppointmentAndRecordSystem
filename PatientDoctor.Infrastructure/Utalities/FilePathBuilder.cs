
namespace PatientDoctor.Infrastructure.Utalities;
public static class FilePathBuilder
{
    public static string BuildPath(string entityType, string userId, string fileName)
    {
        return Path.Combine(entityType, userId, fileName);
    }
}

