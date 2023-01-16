using FluentTranslate.Domain;

namespace FluentTranslate.Serialization.Json
{
    [JsonSerializable(typeof(FluentResource))]
    public partial class FluentResourceJsonContext : JsonSerializerContext
    {
    }
}
