using FluentTranslate.Common;
using FluentTranslate.Domain;
using FluentTranslate.Serialization.Fluent;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FluentTranslate.Providers.EmbeddedResource
{
    public interface IFluentEmdbeddedResourceReader
    {

    }

    public class FluentEmdbeddedResourceReader : IFluentEmdbeddedResourceReader
    {
        public IFluentDeserializer Deserializer { get; }

        public FluentEmdbeddedResourceReader(IFluentDeserializer deserializer!!)
        {
            Deserializer = deserializer;
        }

        public FluentResource Read(Assembly assembly, string path)
        {
            using var stream = assembly.GetManifestResourceStream(path);
            if (stream is null) 
                return null;

            using var reader = new StreamReader(stream);
            var source = reader.ReadToEnd();
            var extension = Path.GetExtension(path);
            var result = Deserializer.Deserialize(source);

            return result;
        }

        public IEnumerable<(FluentResource Resource, string Path)> Read(Assembly assembly)
        {
            var names = assembly.GetManifestResourceNames();
            var fluent = names.Where(x => x.EndsWith(FluentConstants.Extension, StringComparison.OrdinalIgnoreCase));
            foreach(var name in fluent)
            {
                var resource = Read(assembly, name);
                yield return (resource, name);
            }
        }
    }
}
