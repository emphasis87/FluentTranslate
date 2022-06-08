using FluentTranslate.Domain;
using FluentTranslate.Serialization.Json;
using FluentTranslate.Services;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text.Json;

namespace FluentTranslate.Readers
{
    public interface IFluentJsonReader
    {

    }

    public class FluentJsonReader : FluentService<FluentJsonReader>, IFluentJsonReader
    {
        public FluentJsonReader(ILogger<FluentJsonReader> logger = null)
            : base(logger)
        {

        }

        public FluentResource Read(string path, JsonSerializerOptions options = null)
        {
            try
            {
                using var stream = File.OpenRead(path);
                FluentResourceJsonContext context = new(options);
                var resource = JsonSerializer.Deserialize(stream, context.FluentResource);
                return resource;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to read a json file: {Path}", path);
                return null;
            }
        }
    }
}
