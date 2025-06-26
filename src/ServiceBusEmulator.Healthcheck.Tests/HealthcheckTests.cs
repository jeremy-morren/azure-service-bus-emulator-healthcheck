namespace ServiceBusEmulator.Healthcheck.Tests;

public class HealthcheckTests
{
    [Fact]
    public Task DoHealthcheck() =>
        Healthcheck.Run(
            ConfigFile,
            EmulatorConnectionString,
            new StringWriter(),
            CancellationToken.None);

    private const string EmulatorConnectionString =
        "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";

    private static readonly string ConfigFile = Path.Combine(AppContext.BaseDirectory, "Config.json");
}