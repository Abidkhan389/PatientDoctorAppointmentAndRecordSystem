
namespace PatientDoctor.Application.Helpers.General.Exceptions.Models;
public class GlobalExceptionLogDto
{
    public long Id { get; set; }
    public string Message { get; set; } = null!;
    public string? StackTrace { get; set; }
    public string Path { get; set; } = null!;
    public int StatusCode { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
}

