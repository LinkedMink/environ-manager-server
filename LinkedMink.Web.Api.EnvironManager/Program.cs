using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;

namespace LinkedMink.Web.Api.EnvironManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var hostBuilder = WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

            if (Environment.GetEnvironmentVariable("IS_CONTAINERIZED") == "true")
            {
                hostBuilder.UseUrls("http://*:80");
            }

            return hostBuilder;
        }

    }
}
