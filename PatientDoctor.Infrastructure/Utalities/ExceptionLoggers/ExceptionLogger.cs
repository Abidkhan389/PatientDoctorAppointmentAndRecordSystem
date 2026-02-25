
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

