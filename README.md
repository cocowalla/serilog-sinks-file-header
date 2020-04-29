# Serilog.Sinks.File.Header
[![NuGet](https://img.shields.io/nuget/v/Serilog.Sinks.File.Header.svg)](https://www.nuget.org/packages/Serilog.Sinks.File.Header)
[![Build status](https://ci.appveyor.com/api/projects/status/initq19hswan7q4u?svg=true)](https://ci.appveyor.com/project/cocowalla/serilog-sinks-file-header)

A `FileLifecycleHooks`-based plugin for the [Serilog File Sink](https://github.com/serilog/serilog-sinks-file) that writes a configurable header at the start of each log file.

### Getting started
To get started, install the latest [Serilog.Sinks.File.Header](https://www.nuget.org/packages/Serilog.Sinks.File.Header) package from NuGet:

```powershell
Install-Package Serilog.Sinks.File.Header -Version 1.0.1
```

To enable writing a header, use one of the new `LoggerSinkConfiguration` extensions that has a `FileLifecycleHooks` argument, and create a new `HeaderWriter`:

```csharp
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("log.txt", hooks: new HeaderWriter("Timestamp,Level,Message"))
    .CreateLogger();
```

Note this also works if you enable rolling log files.

Instead of writing a static string, you can instead provide a factory method that resolves the header at runtime:

```csharp
Func<string> headerFactory = () => $"My dynamic header {DateTime.UtcNow}";

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("log.txt", hooks: new HeaderWriter(headerFactory))
    .CreateLogger();
```

### JSON appsettings.json configuration
It's also possible to enable log file headers  when configuring Serilog from a configuration file using [Serilog.Settings.Configuration](https://github.com/serilog/serilog-settings-configuration/). To do this, you will first need to create a public static class that can provide the configuration system with a configured instance of `HeaderWriter`:

```csharp
using Serilog.Sinks.File.Header;

namespace MyApp.Logging
{
    public class SerilogHooks
    {
        public static HeaderWriter MyHeaderWriter => new HeaderWriter("Timestamp,Level,Message");
    }
}
```

The `hooks` argument in Your `appsettings.json` file should be configured as follows:

```json
{
  "Serilog": {
    "WriteTo": [
      {
        "Name": "File",
        "Args": {
          "path": "log.gz",
          "hooks": "MyApp.Logging.SerilogHooks::MyHeaderWriter, MyApp"
        }
      }
    ]
  }
}
```

To break this down a bit, what you are doing is specifying the fully qualified type name of the static class that provides your `HeaderWriter`, using `Serilog.Settings.Configuration`'s special `::` syntax to point to the `MyHeaderWriter` member.

### Sample application
A basic [console app](https://github.com/cocowalla/serilog-sinks-file-header/tree/master/sample/Serilog.Sinks.File.Header.Sample) is provides as a sample.

### About `FileLifecycleHooks`
`FileLifecycleHooks` is a Serilog File Sink mechanism that allows hooking into log file lifecycle events, enabling scenarios such as wrapping the Serilog output stream in another stream, or capturing files before they are deleted by Serilog's retention mechanism.

Other available hooks include:

- [serilog-sinks-file-gzip](https://github.com/cocowalla/serilog-sinks-file-gzip): compresses logs as they are written, using streaming GZIP compression
- [serilog-sinks-file-archive](https://github.com/cocowalla/serilog-sinks-file-archive): archives completed log files before they are deleted by Serilog's retention mechanism
