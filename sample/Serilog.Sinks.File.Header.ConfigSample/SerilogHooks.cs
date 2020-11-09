
namespace Serilog.Sinks.File.Header.ConfigSample
{
    public class SerilogHooks
    {
        public static HeaderWriter MyHeaderWriter => new HeaderWriter("Timestamp,Level,Source Context,Message");
    }
}
