FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# Install the required dependencies for the build
RUN apt update && apt install -y clang zlib1g-dev
WORKDIR /src
COPY "ServiceBusEmulator.Healthcheck.csproj" .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /publish -r linux-x64 -p 'PublishAot=true'

FROM mcr.microsoft.com/azure-messaging/servicebus-emulator:latest AS final

# Ignore the .dbg file, we don't need it (saves 15MB)
# See https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/diagnostics#importance-of-the-symbol-file
COPY --from=build /publish/ServiceBusEmulator.Healthcheck /