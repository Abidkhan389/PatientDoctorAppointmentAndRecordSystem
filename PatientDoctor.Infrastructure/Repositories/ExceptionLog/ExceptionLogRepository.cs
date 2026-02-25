
namespace PatientDoctor.Infrastructure.Repositories.ExceptionLog;
public class ExceptionLogRepository : IExceptionLogRepository
{
    private readonly DocterPatiendDbContext _context;

    public ExceptionLogRepository(DocterPatiendDbContext context)
    {
        _context = context;
    }
    public async Task SaveAsync(GlobalExceptionLogDto log)
    {
        GlobalExceptionLog globalExceptionLog = new GlobalExceptionLog(log);
        await _context.GlobalExceptionLogs.AddAsync(globalExceptionLog);
        await _context.SaveChangesAsync();
    }
}

