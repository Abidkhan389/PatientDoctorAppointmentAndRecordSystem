
namespace PatientDoctor.domain.Entities;
public class GlobalExceptionLog
{
    public Guid Id { get; set; }
    public string Message { get; set; } = null!;
    public string? StackTrace { get; set; }
    public string Path { get; set; } = null!;
    public int StatusCode { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public GlobalExceptionLog()
    {

    }
    public GlobalExceptionLog(GlobalExceptionLogDto model)
    {
        this.Id = Guid.NewGuid();
        this.Message = model.Message;
        this.StackTrace = model.StackTrace;
        this.Path= model.Path;
        this.StatusCode= model.StatusCode;
        this.CreatedOn= DateTime.UtcNow;
    }
}


