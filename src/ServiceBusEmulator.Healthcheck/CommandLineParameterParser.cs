namespace ServiceBusEmulator.Healthcheck;

public static class CommandLineParameterParser
{
    /// <summary>
    /// Parses unix-style command line parameters
    /// </summary>
    public static Dictionary<string, string> Parse(string[] arguments)
    {
        if (arguments.Length % 2 != 0)
            throw new ArgumentException("Arguments must be in pairs.");
        var parameters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        for (var i = 0; i < arguments.Length; i += 2)
        {
            var key = arguments[i];
            var value = arguments[i + 1];
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"Invalid parameter: {key} {value}");
            if (!parameters.TryAdd(key, value))
                throw new ArgumentException($"Duplicate parameter: {key}");
        }
        return parameters;
    }
}