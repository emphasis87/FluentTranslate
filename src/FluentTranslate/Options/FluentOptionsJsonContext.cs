using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace FluentTranslate.Options
{
    [JsonSerializable(typeof(FluentClientOptions))]
    //[JsonSerializable(typeof(List<FluentProfileOptions>))]
    //[JsonSerializable(typeof(List<FluentSourceOptions>))]
    //[JsonSerializable(typeof(string))]
    public partial class FluentClientOptionsJsonContext : JsonSerializerContext
    {

    }

    //[JsonSerializable(typeof(FluentProfileOptions))]
    //public partial class FluentProfileOptionsJsonContext : JsonSerializerContext
    //{

    //}

    //[JsonSerializable(typeof(FluentSourceOptions))]
    //public partial class FluentSourceOptionsJsonContext : JsonSerializerContext
    //{

    //}
}
