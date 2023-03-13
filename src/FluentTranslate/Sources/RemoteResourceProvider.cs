using Microsoft.Extensions.Logging;

namespace FluentTranslate.Sources
{
    public class RemoteResourceProvider : IResourceProvider
    {
        public string? Path { get; init; }
        public string? ApiKey { get; init; }
        public TimeSpan? PollingInterval { get; init; }
        public string Id { get; init; } = $"{Guid.NewGuid()}";

        private readonly HttpClient _client;
        private readonly ILogger<RemoteResourceProvider>? _logger;

        public RemoteResourceProvider(HttpClient client, ILogger<RemoteResourceProvider>? logger = null)
        {
            _client = client;
            _logger = logger;
        }

        private IResource[] _resources = Array.Empty<IResource>();
        public IReadOnlyCollection<IResource> Resources => _resources;
        public event CollectionChangeEventHandler ResourcesChanged = delegate { };

        private bool _enableRaisingEvents;
        public bool EnableRaisingEvents
        {
            get => _enableRaisingEvents;
            set
            {
                if (value)
                    Enable();
                else
                    Disable();
            }
        }

        private void Enable()
        {
            if (string.IsNullOrWhiteSpace(Path))
                return;

            _client.BaseAddress = new Uri(Path);
            _client.GetAsync("/");
        }

        private void Disable()
        {
            throw new NotImplementedException();
        }
    }
}
