using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LinkedMink.Base.Extensions;
using LinkedMink.Data.Base;
using LinkedMink.Data.Domain.EnvironManager.Entities;
using Microsoft.EntityFrameworkCore;

namespace LinkedMink.Data.Domain.EnvironManager.Repositories.Concrete
{
    public class LogEntryRepository : EnvironManagerRepository<LogEntry>, ILogEntryRepository
    {
        public LogEntryRepository(EnvironManagerDbContext context) : base(context) {}

        public async Task<PagedData<LogEntry>> GetByDeviceAsync(long hardwareDeviceId, PageCriteria criteria)
        {
            PagedData<LogEntry> returnValue = new PagedData<LogEntry>() { Criteria = criteria };
            IQueryable<LogEntry> query = EntityDbSet;

            query = query.Where(l => l.HardwareDeviceId == hardwareDeviceId);
            query = SortData(query, new SortCriteria()
            {
                Order = SortCriteria.SortOrder.Descending,
                Property = RecordedOnProperty
            });

            var result = PageData(query, criteria);

            returnValue.NumberOfRecords = result.numberOfRecords;
            returnValue.NumberOfPages = result.numberOfPages;
            returnValue.RecordsInPage = await result.query.ToListAsync();

            return returnValue;
        }

        private static readonly PropertyInfo RecordedOnProperty = TypeHelpers.GetProperty<LogEntry>(e => e.RecordedOn);
    }
}
