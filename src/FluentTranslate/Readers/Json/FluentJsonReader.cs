using FluentTranslate.Domain;
using FluentTranslate.Serialization.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace FluentTranslate.Readers.Json
{
    public interface IFluentJsonReader
    {

    }

    public class FluentJsonReader : IFluentJsonReader
    {
        public FluentJsonReader()
        {
        }

        public FluentResource Read(string path, JsonSerializerOptions options = null)
        {
            using var stream = File.OpenRead(path);
            FluentResourceJsonContext context = new(options);
            var resource = JsonSerializer.Deserialize(stream, context.FluentResource);
            return resource;
        }
    }
}
