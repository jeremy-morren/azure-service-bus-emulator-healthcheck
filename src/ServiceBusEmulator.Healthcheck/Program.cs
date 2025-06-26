using ServiceBusEmulator.Healthcheck;

var cts = new CancellationTokenSource();

// Handle Ctrl+C to cancel the application
Console.CancelKeyPress += (_, args) =>
{
    args.Cancel = true; // Prevent the process from terminating.
    cts.Cancel();
};

if (args.Length > 0 && args[0] == "help")
{
    Console.WriteLine("""
                      Healthcheck for Service Bus emulator. Runs a healthcheck against the Service Bus Emulator.
                      Command line options:
                        help                    Show this help message
                        --config <path>         Path to the config file. Default: /ServiceBus_Emulator/ConfigFiles/Config.json
                        --endpoint <endpoint>   IP address or hostname of the Service Bus Emulator. Default: 127.0.0.1
                      """);
    return 0;
}

try
{
    var options = CommandLineParameterParser.Parse(args);
    var configFile = options.GetValueOrDefault("--config") ?? "/ServiceBus_Emulator/ConfigFiles/Config.json";
    var endpoint = options.GetValueOrDefault("--endpoint") ?? "127.0.0.1";
    await Healthcheck.Run(
        configFile: configFile,
        connectionString: $"Endpoint=sb://{endpoint};SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;",
        output: Console.Out,
        cts.Token);
    return 0;
}
catch (OperationCanceledException e) when (e.CancellationToken == cts.Token)
{
    return 2;
}
catch (Exception e)
{
    Console.WriteLine(e);
    return 1;
}