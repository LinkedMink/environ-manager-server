using System.Threading.Tasks;
using LinkedMink.Data.Base;
using LinkedMink.Data.Base.Repositories;
using LinkedMink.Data.Domain.EnvironManager.Entities;

namespace LinkedMink.Data.Domain.EnvironManager.Repositories
{
    public interface ILogEntryRepository : IRepository<LogEntry>
    {
        Task<PagedData<LogEntry>> GetByDeviceAsync(long hardwareDeviceId, PageCriteria criteria);
    }
}
