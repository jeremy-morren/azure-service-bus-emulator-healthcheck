using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ServiceBusEmulator.Healthcheck.Tests;

[TestClass]
public class HealthcheckTests
{
    [TestMethod]
    public async Task RunHealthcheck()
    {
        var configFile = Path.Combine(GetSolutionRoot(), "Test/Config.json");
        Assert.IsTrue(File.Exists(configFile), $"Config file not found at {configFile}");
        var ct = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;
        var output = new StringWriter();
        await Healthcheck.Run(configFile, EmulatorConnectionString, output, ct);
        Assert.IsTrue(output.ToString().Contains("queue"), $"Expected output to contain queue, but got {output}");
    }

    private const string EmulatorConnectionString =
        "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";

    private static string GetSolutionRoot()
    {
        var directory = Path.GetDirectoryName(AppContext.BaseDirectory);
        Assert.IsNotNull(directory);
        while (true)
        {
            if (Directory.GetFiles(directory, "*.sln").Length > 0)
                return directory;
            var parent = Directory.GetParent(directory) ?? 
                         throw new Exception($"Could not find solution root starting at {Path.GetDirectoryName(AppContext.BaseDirectory)}");
            directory = parent.FullName;
        }
    }
}