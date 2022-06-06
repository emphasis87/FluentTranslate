using FluentTranslate.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace FluentTranslate.Serialization.Json
{
    [JsonSerializable(typeof(FluentResource))]
    public partial class FluentResourceJsonContext : JsonSerializerContext
    {
    }
}
