using System;
using Serilog.Debugging;

namespace Serilog.Sinks.File.Header.Sample
{
    /// <summary>
    /// Sample that demonstrates writing tab-delimited logs with a header row, configured programmatically
    /// </summary>
    internal static class Program
    {
        private const string HEADER = "Timestamp\tLevel\tMessage";
        private const string OUTPUT_TEMPLATE = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}\t{Level}\t{Message:lj}{NewLine}";

        private static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("log-with-header.txt", outputTemplate: OUTPUT_TEMPLATE, hooks: new HeaderWriter(HEADER))
                .CreateLogger();

            SelfLog.Enable(Console.Error);

            for (var i = 0; i < 10; i++)
            {
                Log.Information($"Log event {i}");
            }

            Console.WriteLine("Log events have been written to log-with-header.txt");
            Log.CloseAndFlush();
        }
    }
}
