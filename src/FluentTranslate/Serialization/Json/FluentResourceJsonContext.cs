using FluentTranslate.Domain;

namespace FluentTranslate.Serialization.Json
{
    [JsonSerializable(typeof(FluentDocument))]
    public partial class FluentResourceJsonContext : JsonSerializerContext
    {
    }
}
