namespace ServiceBusEmulator.Healthcheck.Config;

public record ServiceBusEmulatorOptions
{
    public required ServiceBusUserConfig UserConfig { get; set; }
}

public record ServiceBusUserConfig
{
    public List<ServiceBusNamespace>? Namespaces { get; set; }
}

public record ServiceBusNamespace
{
    public required string Name { get; set; }
    public List<ServiceBusQueue>? Queues { get; set; }
    public List<ServiceBusTopic>? Topics { get; set; }
}

public record ServiceBusQueue
{
    public required string Name { get; set; }
}

public record ServiceBusTopic
{
    public required string Name { get; set; }
    public List<ServiceBusSubscription>? Subscriptions { get; set; }
}

public record ServiceBusSubscription
{
    public required string Name { get; set; }
}