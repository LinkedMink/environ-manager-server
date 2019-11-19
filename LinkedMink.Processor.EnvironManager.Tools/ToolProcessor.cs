using LinkedMink.Processor.EnvironManager.Tools.Commands;
using Microsoft.Extensions.Logging;
using System;

namespace LinkedMink.Processor.EnvironManager.Tools
{
    internal class ToolProcessor : Processor<ToolProcessor, ToolApplicationContext>
    {
        public ToolProcessor(string[] arguments) : base(arguments) { }

        protected override void DoWork()
        {
            ParseArguments();
            try
            {
                var code = _command.ExecuteAsync(Context).Result;
                Logger.LogInformation($"Exit Code: {code}");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
            }
        }

        private void PrintHelp()
        {
            Console.WriteLine("Usage: dotnet ThracianGate.Processor.Tools.dll [Command] [Arguments]*");
        }

        private void ParseArguments()
        {
            if (Context.CommandLineArguments.Count < 1)
            {
                PrintHelp();
                return;
            }

            switch (Context.CommandLineArguments[0].ToLower())
            {
                case "seed":
                    _command = new SeedCommand();
                    break;
                default:
                    PrintHelp();
                    Environment.Exit(0);
                    break;
            }

            Context.CommandLineArguments.RemoveAt(0);
        }

        private ICommand _command;
    }
}
