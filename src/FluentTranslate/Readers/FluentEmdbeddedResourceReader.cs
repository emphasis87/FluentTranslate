using Microsoft.Extensions.Logging;

using FluentTranslate.Common;
using FluentTranslate.Domain;
using FluentTranslate.Serialization.Fluent;
using FluentTranslate.Services;

namespace FluentTranslate.Readers
{
    public interface IFluentEmdbeddedResourceReader
    {

    }

    public class FluentEmdbeddedResourceReader : FluentService<FluentEmdbeddedResourceReader>, IFluentEmdbeddedResourceReader
    {
        private IFluentDeserializer _deserializer;
        public IFluentDeserializer Deserializer
        {
            get => _deserializer ??= FluentServices.Deserializer;
            set => _deserializer = value;
        }

        public FluentEmdbeddedResourceReader(IFluentDeserializer deserializer = null, ILogger<FluentEmdbeddedResourceReader> logger = null)
            : base(logger)
        {
            Deserializer = deserializer;
        }

        public FluentResource Read(Assembly assembly, string path)
        {
            try
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
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unable to read a fluent file: {Assembly}, {Path}", assembly, path);
                return null;
            }
        }

        public IEnumerable<(FluentResource Resource, string Path)> Read(Assembly assembly)
        {
            var names = assembly.GetManifestResourceNames();
            var fluent = names.Where(x => x.EndsWith(FluentConstants.Extension, StringComparison.OrdinalIgnoreCase));
            foreach (var name in fluent)
            {
                var info = assembly.GetManifestResourceInfo(name);
                var resource = Read(assembly, name);
                if (resource is not null)
                    yield return (resource, name);
            }
        }
    }
}
