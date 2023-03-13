using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentTranslate.Services
{
    public class EngineFactory : IEngineFactory
    {
        private readonly FluentConfiguration _configuration;
        private readonly ILogger<EngineFactory>? _logger;

        public EngineFactory(
            FluentConfiguration configuration,
            ILogger<EngineFactory>? logger = null)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public IEngine Create(string profile = "default")
        {
            return default;
        }
    }
}
