

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