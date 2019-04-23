using System.Collections.Generic;
using System.IO;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Sinks.File.Header.Tests.Support
{
    public static class Utils
    {
        public static List<string> ReadAllLines(this string path)
        {
            var lines = new List<string>();

            using (var fs = System.IO.File.OpenRead(path))
            using (var reader = new StreamReader(fs))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            return lines;
        }

        public static void WriteAll(this Logger log, IEnumerable<LogEvent> logEvents)
        {
            foreach (var logEvent in logEvents)
            {
                log.Write(logEvent);
            }
        }
    }
}
