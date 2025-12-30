
using PatientDoctor.Application.Helpers.General.Exceptions.Models;

namespace PatientDoctor.Application.Contracts.Persistance.IException;
public interface IExceptionLogRepository
{
    Task SaveAsync(GlobalExceptionLogDto log);
}

