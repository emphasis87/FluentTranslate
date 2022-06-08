using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace FluentTranslate.Engine
{
    [JsonSerializable(typeof(FluentProfile))]
    public partial class FluentProfileJsonContext : JsonSerializerContext
    {

    }
}
