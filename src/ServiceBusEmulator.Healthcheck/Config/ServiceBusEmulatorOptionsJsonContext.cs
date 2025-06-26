using System.Text.Json;
using System.Text.Json.Serialization;

namespace ServiceBusEmulator.Healthcheck.Config;

[JsonSourceGenerationOptions(
    PropertyNameCaseInsensitive = true,
    ReadCommentHandling = JsonCommentHandling.Skip)]

[JsonSerializable(typeof(ServiceBusEmulatorOptions))]
public partial class ServiceBusEmulatorOptionsJsonContext : JsonSerializerContext;