using System;

namespace T3BridgePattern
{
    public interface ILogWr
    {
        void Write(string message);
    }

    public abstract class Logger
    {
        protected ILogWr _writer;

        public Logger(ILogWr writer) => _writer = writer;

        public abstract void Log(string message);
    }

    public class StoreModuleLogger : Logger
    {
        private string _module;

        public StoreModuleLogger(ILogWr writer, string module) : base(writer)
            => _module = module;

        public override void Log(string message)
            => _writer.Write($"[{_module}] {DateTime.Now:HH:mm:ss} - {message}");
    }

    public class ConsoleWr : ILogWr
    {
        public void Write(string message) => Console.WriteLine(message);
    }

    public class FileWr : ILogWr
    {
        private string _filename;

        public FileWr(string filename) => _filename = filename;

        public void Write(string message)
            => System.IO.File.AppendAllText(_filename, message + Environment.NewLine);
    }

    internal class T3BridgePattern
    {
        static void Main(string[] args)
        {
            var console = new ConsoleWr();
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string logPath = Path.Combine(projectDirectory, "logs.txt");
            var file = new FileWr(logPath);

            var orderLogger = new StoreModuleLogger(console, "ORDERS");
            var userLogger = new StoreModuleLogger(file, "USERS");

            orderLogger.Log("New order #952");
            userLogger.Log("User registered");

            orderLogger = new StoreModuleLogger(file, "ORDERS");
            orderLogger.Log("Order completed");
        }
    }
}