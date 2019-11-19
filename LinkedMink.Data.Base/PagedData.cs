using System.Collections.Generic;

namespace LinkedMink.Data.Base
{
    public class PagedData<TEntity>
    {
        public long NumberOfRecords { get; set; }

        public int NumberOfPages { get; set; }

        public IEnumerable<TEntity> RecordsInPage { get; set; }

        public PageCriteria Criteria { get; set; }
    }
}
