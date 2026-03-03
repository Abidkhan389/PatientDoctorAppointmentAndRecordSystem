using Microsoft.AspNetCore.Http;
namespace BuildingBlocks.Exceptions.Handler.ExceptionLogger;
public interface IExceptionLogger
{
    Task LogAsync(HttpContext context, Exception ex, int statusCode);
}
