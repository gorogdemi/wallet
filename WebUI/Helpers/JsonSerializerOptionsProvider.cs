using System.Text.Json;
using System.Text.Json.Serialization;

namespace DevQuarter.Wallet.WebUI.Helpers
{
    public static class JsonSerializerOptionsProvider
    {
        static JsonSerializerOptionsProvider()
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            options.Converters.Add(new JsonStringEnumConverter());
            DefaultOptions = options;
        }

        public static JsonSerializerOptions DefaultOptions { get; }
    }
}