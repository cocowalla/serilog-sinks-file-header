#!/bin/bash
set -ev

dotnet restore ./serilog-sinks-file-header.sln --runtime netstandard2.0
dotnet build ./src/Serilog.Sinks.File.Header/Serilog.Sinks.File.Header.csproj --runtime netstandard2.0 --configuration Release
dotnet build ./sample/Serilog.Sinks.File.Header.Sample/Serilog.Sinks.File.Header.Sample.csproj --framework netcoreapp2.2 --configuration Release

dotnet test ./test/Serilog.Sinks.File.Header.Test/Serilog.Sinks.File.Header.Test.csproj --framework netcoreapp2.2
