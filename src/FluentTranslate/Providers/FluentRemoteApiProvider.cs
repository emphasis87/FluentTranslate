using FluentTranslate.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

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
