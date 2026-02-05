
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PatientDoctor.Application.Contracts.Persistance.IException;
using PatientDoctor.Application.Helpers.General.Exceptions.Models;

namespace PatientDoctor.Infrastructure.Utalities.ExceptionLoggers;
public class ExceptionLogger : IExceptionLogger
{
    private readonly IServiceScopeFactory _scopeFactory;
    public ExceptionLogger(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;
    public async Task LogAsync(HttpContext context, Exception ex, int statusCode)
    {
        using var scope = _scopeFactory.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IExceptionLogRepository>();
        var log = new GlobalExceptionLogDto
        {
            Message = ex.Message,
            StackTrace = ex.StackTrace,
            Path = context.Request.Path,
            StatusCode = statusCode
        };

        await repo.SaveAsync(log);
    }
}

