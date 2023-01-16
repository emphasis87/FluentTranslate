using Microsoft.Extensions.Logging;

using FluentTranslate.Services;

namespace FluentTranslate.Providers
{
    public class FluentFileProvider : FluentProvider<FluentFileProvider>
    {
        public override string Name => "file";

        public FluentFileProvider(ILogger<FluentFileProvider> logger = null)
            : base(logger)
        {

        }
    }
}
