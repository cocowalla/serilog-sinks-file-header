using System;
using System.IO;
using System.Linq;
using Serilog.Sinks.File.Header.Tests.Support;
using Shouldly;
using Xunit;

namespace Serilog.Sinks.File.Header.Tests
{
    public class RollingFileSinkTests
    {
        // Standard W3C log header
        // ReSharper disable once InconsistentNaming
        private const string W3C_HEADER = "date time cs-uri-stem cs(User-Agent) sc-status";

        [Fact]
        public void Should_write_header_at_start_of_rolling_log_files()
        {
            var headerWriter = new HeaderWriter(W3C_HEADER);

            var logEvents = Some.LogEvents(3);

            using (var temp = TempFolder.ForCaller())
            {
                var path = temp.AllocateFilename("txt");

                // Use a rolling log file configuration with a 50-byte size limit, so we roll after writing the header and a single log event
                using (var log = new LoggerConfiguration()
                    .WriteTo.File(path, rollOnFileSizeLimit: true, fileSizeLimitBytes: 50, hooks: headerWriter)
                    .CreateLogger())
                {
                    log.WriteAll(logEvents);
                }

                // Get all the files the rolling file sink wrote
                var files = Directory.GetFiles(temp.Path)
                    .OrderBy(p => p, StringComparer.OrdinalIgnoreCase)
                    .ToArray();

                // We should have found a file for each entry in logEvents
                files.Length.ShouldBe(logEvents.Length);

                // Check each file to ensure it contains our header plus 1 log event
                for (var i = 0; i < files.Length; i++)
                {
                    var lines = files[i].ReadAllLines();

                    // File should contain our header line plus 1 log event
                    lines.Count.ShouldBe(2);

                    // Ensure the file starts with the header
                    lines[0].ShouldBe(W3C_HEADER);

                    // Ensure the log event was written as normal
                    lines[1].ShouldEndWith(logEvents[i].MessageTemplate.Text);
                }
            }
        }
    }
}

