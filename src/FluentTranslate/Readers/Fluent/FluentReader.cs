using FluentTranslate.Common;
using FluentTranslate.Domain;
using FluentTranslate.Serialization.Fluent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FluentTranslate.Readers.Fluent
{
    public interface IFluentReader
    {

    }

    public class FluentReader : IFluentReader
    {
        public ILogger<FluentReader> Logger { get; set; }
        private ILogger<FluentReader> GetLogger() => Logger ?? FluentServices.Default.GetService<ILogger<FluentReader>>();

        public IFluentDeserializer Deserializer { get; set; }
        private IFluentDeserializer GetDeserializer() => Deserializer ?? FluentServices.Default.GetService<IFluentDeserializer>();

        public FluentReader(IFluentDeserializer deserializer = null, ILogger<FluentReader> logger = null)
        {
            Deserializer = deserializer;
            Logger = logger;
        }

        public FluentResource Read(string path)
        {
            try
            {
                var content = File.ReadAllText(path, Encoding.UTF8);
                var resource = GetDeserializer().Deserialize(content);
                return resource;
            }
            catch (Exception ex)
            {
                GetLogger().LogError(ex, "Unable to read a fluent file: {Path}", path);
                return null;
            }
        }
    }
}
