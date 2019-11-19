namespace LinkedMink.Processor.EnvironManager.Tools
{
    static class Program
    {
        static void Main(string[] args)
        {
            var processor = new ToolProcessor(args);
            processor.Run();
        }
    }
}
