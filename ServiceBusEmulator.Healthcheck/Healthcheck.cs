using System.Diagnostics;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using ServiceBusEmulator.Healthcheck.Config;

// ReSharper disable MethodHasAsyncOverload
// ReSharper disable ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator

namespace ServiceBusEmulator.Healthcheck;

public static class Healthcheck
{
    public static async Task Run(
        string configFile,
        string connectionString,
        TextWriter output,
        CancellationToken ct)
    {
        ArgumentException.ThrowIfNullOrEmpty(configFile);

        var ts = Stopwatch.GetTimestamp();
        var options = LoadOptions(configFile);
        var client = new ServiceBusClient(connectionString);
        foreach (var ns in options.UserConfig.Namespaces ?? [])
        {
            output.WriteLine($"Namespace: {ns.Name}");
            // Test queues
            foreach (var queue in ns.Queues ?? [])
            {
                var receiver = client.CreateReceiver(queue.Name);
                await receiver.PeekMessageAsync(cancellationToken: ct);
                output.WriteLine($"Queue: {queue.Name}");
            }
            // Test topics
            foreach (var topic in ns.Topics ?? [])
            {
                foreach (var subscription in topic.Subscriptions ?? [])
                {
                    var subscriptionReceiver = client.CreateReceiver(topic.Name, subscription.Name);
                    await subscriptionReceiver.PeekMessageAsync(cancellationToken: ct);
                    output.WriteLine($"Topic: {topic.Name}. Subscription: {subscription.Name}");
                }
            }

            var elapsed = Stopwatch.GetElapsedTime(ts).TotalMilliseconds;
            output.WriteLine($"Finished in {elapsed:0.00} ms");
        }
    }

    private static ServiceBusEmulatorOptions LoadOptions(string configFile)
    {
        using var fs = new FileStream(configFile, FileMode.Open, FileAccess.Read);
        return JsonSerializer.Deserialize(fs, ServiceBusEmulatorOptionsJsonContext.Default.ServiceBusEmulatorOptions)!;
    }
}