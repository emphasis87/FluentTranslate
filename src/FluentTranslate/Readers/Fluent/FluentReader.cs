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

    public class FluentReader : FluentService<FluentReader>, IFluentReader
    {
        private IFluentDeserializer _deserializer;
        public IFluentDeserializer Deserializer 
        {
            get => _deserializer ?? FluentServices.Deserializer;
            set => _deserializer = value;
        }

        public FluentReader(IFluentDeserializer deserializer = null, ILogger<FluentReader> logger = null)
            : base(logger)
        {
            Deserializer = deserializer;
        }

        public FluentResource Read(string path)
        {
            try
            {
                var content = File.ReadAllText(path, Encoding.UTF8);
                var resource = Deserializer.Deserialize(content);
                return resource;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to read a fluent file: {Path}", path);
                return null;
            }
        }
    }
}
