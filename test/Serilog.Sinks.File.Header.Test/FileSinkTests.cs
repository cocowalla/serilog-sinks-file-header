using System.IO;
using Serilog.Sinks.File.Header.Tests.Support;
using Shouldly;
using Xunit;

namespace Serilog.Sinks.File.Header.Tests
{
    public class FileSinkTests
    {
        // Standard W3C log header
        // ReSharper disable once InconsistentNaming
        private const string W3C_HEADER = "date time cs-uri-stem cs(User-Agent) sc-status";

        [Fact]
        public void Should_write_header_at_start_of_log_file()
        {
            var headerWriter = new HeaderWriter(W3C_HEADER);

            var logEvents = Some.LogEvents(3);

            using (var temp = TempFolder.ForCaller())
            {
                var path = temp.AllocateFilename("txt");

                using (var log = new LoggerConfiguration()
                    .WriteTo.File(Path.Combine(path), hooks: headerWriter)
                    .CreateLogger())
                {
                    log.WriteAll(logEvents);
                }

                // Read the contents of the file that was written
                var lines = path.ReadAllLines();

                // File should contain our header line plus 3 log events
                lines.Count.ShouldBe(1 + logEvents.Length);

                // Ensure the file starts with the header
                lines[0].ShouldBe(W3C_HEADER);

                // Ensure all the log events were written as normal
                for (var i = 0; i < logEvents.Length; i++)
                {
                    lines[i + 1].ShouldEndWith(logEvents[i].MessageTemplate.Text);
                }
            }
        }

        // If the log file already contains anything, ensure we don't write the header
        [Fact]
        public void Should_not_write_header_to_non_empty_stream()
        {
            var headerWriter = new HeaderWriter(W3C_HEADER);

            var logEvents = Some.LogEvents(3);

            using (var temp = TempFolder.ForCaller())
            {
                var path = temp.AllocateFilename("txt");

                // Write something to the start of the log file
                System.IO.File.WriteAllText(path, "Myergen");

                using (var log = new LoggerConfiguration()
                    .WriteTo.File(Path.Combine(path), hooks: headerWriter)
                    .CreateLogger())
                {
                    log.WriteAll(logEvents);
                }

                // Read the contents of the file that was written
                var lines = path.ReadAllLines();

                // File should contain only the 3 log events
                lines.Count.ShouldBe(logEvents.Length);

                // Ensure all the log events were written as normal
                for (var i = 0; i < logEvents.Length; i++)
                {
                    lines[i].ShouldEndWith(logEvents[i].MessageTemplate.Text);
                }
            }
        }
    }
}
