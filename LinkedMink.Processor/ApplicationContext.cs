using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace LinkedMink.Processor
{
    public abstract class ApplicationContext
    {
        public List<string> CommandLineArguments { get; set; }

        public IServiceProvider ServiceProvider { get; private set; }

        public IConfigurationRoot Configuration { get; private set; }

        public virtual void Load()
        {
            var configBuilder = GetConfigurationBuilder();
            Configuration = configBuilder.Build();

            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
        }

        protected virtual IConfigurationBuilder GetConfigurationBuilder()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(SettingsFile, optional: true, reloadOnChange: true);
            builder.AddEnvironmentVariables();

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (!string.IsNullOrEmpty(environment))
            {
                builder.AddJsonFile(string.Format(SettingsFileFormat, environment), optional: true);
            }

            return builder;
        }

        protected virtual void ConfigureServices(IServiceCollection services)
        {
            AddBaseServices(services);
        }

        protected IServiceCollection AddBaseServices(IServiceCollection services)
        {
            services.AddLogging(options =>
            {
                options.AddConfiguration(Configuration.GetSection("Logging"));
                options.AddConsole();
            });

            return services;
        }

        public const string SettingsFile = "appsettings.json";
        public const string SettingsFileFormat = "appsettings.{0}.json";
    }
}
