
namespace PatientDoctor.Application.Contracts.Persistance.IException;
public interface IExceptionLogger
{
    Task LogAsync(HttpContext context, Exception ex, int statusCode);
}

