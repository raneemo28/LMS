using LMS.Domain.Entities;
using LMS.Domain.Interfaces;
using LMS.Infra.Database;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infra.Repository{
    public class LogRepository : ILogRepository
    {
        private readonly LoggingDbContext _context;

        public LogRepository(LoggingDbContext context)
        {
            _context = context;
        }

        public async Task SaveLogAsync(LogEntry logEntry)
        {
            await _context.Logs.AddAsync(logEntry);
            await _context.SaveChangesAsync();
        }
    }
}