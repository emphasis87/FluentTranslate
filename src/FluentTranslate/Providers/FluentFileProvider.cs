using FluentTranslate.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

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
