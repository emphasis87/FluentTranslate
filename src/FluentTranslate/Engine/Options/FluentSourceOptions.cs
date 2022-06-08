using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FluentTranslate.Engine.Options
{
    public class FluentSourceOptions
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Provider { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JsonElement> AdditionalProperties { get; }

        public FluentSourceOptions()
        {
            AdditionalProperties = new Dictionary<string, JsonElement>();
        }
    }
}