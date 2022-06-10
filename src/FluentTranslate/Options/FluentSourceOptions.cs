using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FluentTranslate.Options
{
    public class FluentSourceOptions
    {
        public string Provider { get; set; }
        public string Path { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JsonElement> AdditionalProperties { get; set; } = new();
    }
}