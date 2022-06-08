using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FluentTranslate.Services
{
    public abstract class FluentService<T>
    {
        private ILogger<T> _logger;
        public ILogger<T> Logger
        {
            get => _logger ??= FluentServices.Default.GetService<ILogger<T>>();
            set => _logger = value;
        }

        protected FluentService(ILogger<T> logger = null)
        {
            Logger = logger;
        }
    }
}
