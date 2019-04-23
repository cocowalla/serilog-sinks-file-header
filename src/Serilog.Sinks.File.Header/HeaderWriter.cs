using System;
using System.IO;
using System.Text;
using Serilog.Debugging;

namespace Serilog.Sinks.File.Header
{
    /// <inheritdoc />
    /// <summary>
    /// Writes a header at the start of every log file
    /// </summary>
    public class HeaderWriter : FileLifecycleHooks
    {
        // Same as the default StreamWriter buffer size
        private const int DEFAULT_BUFFER_SIZE = 1014;

        private readonly Func<string> headerFactory;

        public HeaderWriter(string header)
        {
            this.headerFactory = () => header;
        }

		public HeaderWriter(Func<string> headerFactory)
        {
            this.headerFactory = headerFactory;
        }

        public override Stream OnFileOpened(Stream underlyingStream, Encoding encoding)
        {
			if (underlyingStream.Length != 0)
			{
				SelfLog.WriteLine($"File header will not be written, as the stream already contains {underlyingStream.Length} bytes of content");
                return base.OnFileOpened(underlyingStream, encoding);
			}

			using (var writer = new StreamWriter(underlyingStream, encoding, DEFAULT_BUFFER_SIZE, true))
			{
				var header = this.headerFactory();

				writer.WriteLine(header);
				writer.Flush();
				underlyingStream.Flush();
			}

            return base.OnFileOpened(underlyingStream, encoding);
        }
    }
}
