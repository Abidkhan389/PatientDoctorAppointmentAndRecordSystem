using Microsoft.EntityFrameworkCore;
using PatientDoctor.Application.Contracts.Persistance.IException;
using PatientDoctor.Application.Helpers.General.Exceptions.Models;
using PatientDoctor.domain.Entities;
using PatientDoctor.Infrastructure.Persistance;

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

