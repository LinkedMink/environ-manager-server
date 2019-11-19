using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LinkedMink.Data.Domain.EnvironManager.PostgreSql
{
    public class PostgreSqlDbContext : EnvironManagerDbContext
    {
        public PostgreSqlDbContext(
            DbContextOptions<PostgreSqlDbContext> options, 
            ILogger<PostgreSqlDbContext> logger) : base(options)
        {
            Logger = logger;
        }

        protected ILogger<PostgreSqlDbContext> Logger { get; }
    }
}
