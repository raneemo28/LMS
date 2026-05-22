using LMS.Domain.Entities;

namespace LMS.Domain.Interfaces
{
    public interface ILogRepository
    {
        Task SaveLogAsync(LogEntry logEntry);
    }
}