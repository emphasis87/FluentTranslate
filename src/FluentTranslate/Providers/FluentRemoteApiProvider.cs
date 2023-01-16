using Microsoft.Extensions.Logging;

using FluentTranslate.Services;

namespace FluentTranslate.Providers
{
    public class FluentRemoteApiProvider : FluentProvider<FluentRemoteApiProvider>
    {
        public override string Name => "remoteApi";

        public FluentRemoteApiProvider(ILogger<FluentRemoteApiProvider> logger = null) 
            : base(logger)
        {

        }
    }
}
