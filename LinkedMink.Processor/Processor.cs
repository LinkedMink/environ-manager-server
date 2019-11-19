using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace LinkedMink.Processor
{
    public abstract class Processor<TProcessor, TContext> 
        where TContext : ApplicationContext, new()
    {
        public Processor(string[] arguments)
        {
            Context = new TContext();
            Context.CommandLineArguments = arguments.ToList();
        }

        public class ProgressedEventArgs : EventArgs
        {
            public ProgressedEventArgs(int? percent) => PercentComplete = percent;

            public int? PercentComplete { get; private set; } = null;
        }

        public event EventHandler<ProgressedEventArgs> Progressed;

        public DateTime StartDateTime { get; protected set; }

        public DateTime LastProgressed { get; protected set; }

        public TimeSpan UpTime => DateTime.Now - StartDateTime;

        public virtual void Run()
        {
            StartDateTime = DateTime.Now;
            Initialize();
            Logger.LogInformation($"{GetType().Name} Started at {StartDateTime}");

            DoWork();

            Logger.LogInformation($"{GetType().Name} Ended at {DateTime.Now}");
            Logger.LogInformation($"Total Runtime: {UpTime}");
        }

        protected abstract void DoWork();

        protected virtual void ReportProgress(int? percent = null)
        {
            if (percent == null || percent < 0 || percent > 100)
                Logger.LogInformation("Processing...");
            else
                Logger.LogInformation($"{percent}% Complete");

            LastProgressed = DateTime.Now;
        }

        protected virtual void Initialize()
        {
            Context.Load();
            Logger = (ILogger<TProcessor>) Context.ServiceProvider.GetService(typeof(ILogger<TProcessor>));
            Logger.LogDebug("Application Context Initialied");
        }

        protected virtual void OnProgressed(ProgressedEventArgs args)
        {
            Progressed?.Invoke(this, args);
        }

        protected ILogger<TProcessor> Logger { get; private set; }

        protected TContext Context { get; private set; }
    }
}
