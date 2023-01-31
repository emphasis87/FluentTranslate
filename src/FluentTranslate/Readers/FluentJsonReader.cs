using Microsoft.Extensions.Logging;

using FluentTranslate.Domain;
using FluentTranslate.Serialization.Json;
using FluentTranslate.Services;

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

        public FluentDocument Read(string path, JsonSerializerOptions options = null)
        {
            try
            {
                using var stream = File.OpenRead(path);
                FluentResourceJsonContext context = new(options);
                var resource = JsonSerializer.Deserialize(stream, context.FluentDocument);
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
