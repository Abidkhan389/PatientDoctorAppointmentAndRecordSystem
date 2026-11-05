

namespace PatientDoctor.Application.Contracts.Persistance.IException;
public interface IExceptionLogRepository
{
    Task SaveAsync(GlobalExceptionLogDto log);
}

