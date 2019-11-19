using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LinkedMink.Data.Domain.EnvironManager.PostgreSql
{
    public class SqlServerDbContext : EnvironManagerDbContext
    {
        public SqlServerDbContext(
            DbContextOptions<SqlServerDbContext> options, 
            ILogger<SqlServerDbContext> logger) : base(options)
        {
            Logger = logger;
        }

        protected ILogger<SqlServerDbContext> Logger { get; }
    }
}
