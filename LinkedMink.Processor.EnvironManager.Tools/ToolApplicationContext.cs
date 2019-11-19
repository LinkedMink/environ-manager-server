using Microsoft.Extensions.DependencyInjection;

namespace LinkedMink.Processor.EnvironManager.Tools
{
    internal class ToolApplicationContext : EnvironManagerApplicationContext
    {
        protected override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            AddEntityFramework(services);
        }
    }
}
