using FluentTranslate.Common;
using FluentTranslate.Domain;
using FluentTranslate.Serialization.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
        public ILogger<FluentJsonReader> Logger { get; set; }
        private ILogger<FluentJsonReader> GetLogger() => Logger ?? FluentServices.Default.GetService<ILogger<FluentJsonReader>>();

        public FluentJsonReader(ILogger<FluentJsonReader> logger)
        {
            Logger = logger;
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
                GetLogger().LogError(ex, "Unable to read a json file: {Path}", path);
                return null;
            }
        }
    }
}
