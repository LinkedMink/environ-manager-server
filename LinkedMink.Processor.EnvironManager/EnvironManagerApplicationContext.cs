using LinkedMink.Data.Base.Entities.Identity;
using LinkedMink.Data.Base.Repositories;
using LinkedMink.Data.Base.Repositories.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using LinkedMink.Data.Domain.EnvironManager;
using LinkedMink.Data.Domain.EnvironManager.Entities;
using LinkedMink.Data.Domain.EnvironManager.PostgreSql;
using LinkedMink.Data.Domain.EnvironManager.Repositories.Concrete;

namespace LinkedMink.Processor.EnvironManager
{
    public class EnvironManagerApplicationContext : ApplicationContext
    {
        protected IServiceCollection AddEntityFramework(IServiceCollection services)
        {
            var connectionStringKey = Environment.GetEnvironmentVariable("ASPNETCORE_CONNECT_STRING_KEY");
            if (connectionStringKey == null)
                connectionStringKey = "DefaultConnection";

            if (connectionStringKey == "PostgreSql")
            {
                AddEntityFrameworkProvider<PostgreSqlDbContext>(
                    (options, s) => options.UseNpgsql(s), () => services.AddEntityFrameworkNpgsql());
            }
            else
            {
                AddEntityFrameworkProvider<SqlServerDbContext>(
                    (options, s) => options.UseSqlServer(s), () => services.AddEntityFrameworkSqlServer());
            }

            services.AddTransient(typeof(IRepository<>), typeof(EnvironManagerRepository<>));

            return services;

            void AddEntityFrameworkProvider<TContext>(
                Func<DbContextOptionsBuilder, string, DbContextOptionsBuilder> addContextFunction,
                Action addEfFunction = null) where TContext : DbContext
            {
                addEfFunction?.Invoke();

                services.AddDbContext<TContext>(
                    options => addContextFunction(
                        options, Configuration.GetConnectionString("DefaultConnection")));

                services
                    .AddIdentity<ClientUser, ClientRole>()
                    .AddEntityFrameworkStores<TContext>();

                var constructor = typeof(TContext).GetConstructor(
                    new[] { typeof(DbContextOptions<TContext>), typeof(ILogger<TContext>) });

                services.AddTransient(p =>
                    (EnvironManagerDbContext)constructor.Invoke(new object[] {
                        (DbContextOptions<TContext>)p.GetService(typeof(DbContextOptions<TContext>)),
                        (ILogger<TContext>)p.GetService(typeof(ILogger<TContext>)) }));
            }
        }
    }
}
