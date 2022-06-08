using FluentTranslate.Engine.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace FluentTranslate.Engine
{
    [JsonSerializable(typeof(FluentProfileOptions))]
    public partial class FluentProfileOptionsJsonContext : JsonSerializerContext
    {

    }
}
