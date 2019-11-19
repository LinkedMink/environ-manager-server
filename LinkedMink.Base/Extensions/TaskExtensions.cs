using System.Threading.Tasks;

namespace LinkedMink.Base.Extensions
{
    public static class TaskExtensions
    {
        public static bool IsRunningOrQueued(this Task task) =>
            task.Status == TaskStatus.Running ||
            task.Status == TaskStatus.WaitingForActivation ||
            task.Status == TaskStatus.WaitingForChildrenToComplete ||
            task.Status == TaskStatus.WaitingToRun;
    }
}
