using System.Threading.Tasks;

namespace LinkedMink.Processor.EnvironManager.Tools.Commands
{
    internal enum CommandStatus
    {
        Succeeded = 0,
        Failed = 1
    }

    internal interface ICommand
    {
        Task<CommandStatus> ExecuteAsync(ToolApplicationContext context);
    }
}
