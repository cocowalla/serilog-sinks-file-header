dotnet restore .\serilog-sinks-file-header.sln
dotnet build .\src\Serilog.Sinks.File.Header\Serilog.Sinks.File.Header.csproj --configuration Release
dotnet build .\sample\Serilog.Sinks.File.Header.Sample\Serilog.Sinks.File.Header.Sample.csproj --configuration Release

dotnet test .\test\Serilog.Sinks.File.Header.Test\Serilog.Sinks.File.Header.Test.csproj

dotnet pack .\src\Serilog.Sinks.File.Header\Serilog.Sinks.File.Header.csproj -c Release
