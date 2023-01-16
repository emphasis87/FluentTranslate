using Microsoft.Extensions.Logging;

using FluentTranslate.Services;

namespace FluentTranslate.Providers
{
    public class FluentEmbeddedResourceProvider : FluentProvider<FluentEmbeddedResourceProvider>
    {
        public override string Name => "embeddedResource";

        public FluentEmbeddedResourceProvider(ILogger<FluentEmbeddedResourceProvider> logger = null) 
            : base(logger)
        {
        }
    }
}
