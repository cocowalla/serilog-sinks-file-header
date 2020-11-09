using System;
using Microsoft.Extensions.Configuration;
using Serilog.Debugging;

namespace Serilog.Sinks.File.Header.ConfigSample
{
    /// <summary>
    /// Sample that demonstrates writing CSV-formatted logs with a header row, configured from appsettings.json
    /// </summary>
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();

            SelfLog.Enable(Console.Error);

            for (var i = 0; i < 10; i++)
            {
                Log.Information($"Log event {i}");
            }

            Console.WriteLine("Log events have been written to .\\Logs\\log-with-header.csv");
            Log.CloseAndFlush();
        }
    }
}
