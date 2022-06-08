using FluentTranslate.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

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
