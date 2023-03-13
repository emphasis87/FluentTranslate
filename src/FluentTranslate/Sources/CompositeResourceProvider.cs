using Microsoft.Extensions.Logging;
using System.ComponentModel;

namespace FluentTranslate.Sources
{
    public class CompositeResourceProvider : IResourceProvider
    {
        private readonly IResourceProvider[] _providers;
        private readonly ILogger<CompositeResourceProvider>? _logger;

        public CompositeResourceProvider(
            IEnumerable<IResourceProvider>? providers, 
            ILogger<CompositeResourceProvider>? logger = null)
        {
            _providers = providers?.ToArray() ?? Array.Empty<IResourceProvider>();
            _logger = logger;
        }

        public string Id { get; init; } = $"{Guid.NewGuid()}";

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
            try
            {
                foreach (var provider in _providers)
                {
                    provider.ResourcesChanged -= OnResourcesChanged;
                    provider.ResourcesChanged += OnResourcesChanged;
                    provider.EnableRaisingEvents = true;
                }
                LoadResources();
                _enableRaisingEvents = true;
                return;
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "Error initializing resource providers");
            }
        }

        private void Disable()
        {
            try
            {
                foreach (var provider in _providers)
                {
                    provider.EnableRaisingEvents = false;
                    provider.ResourcesChanged -= OnResourcesChanged;
                }
                _enableRaisingEvents = false;
                return;
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "Error initializing resource providers");
            }
        }

        private void OnResourcesChanged(object sender, CollectionChangeEventArgs e)
        {
            LoadResources();
        }

        private void LoadResources()
        {
            _resources = _providers.SelectMany(p => p.Resources).ToArray();
            ResourcesChanged(this, new(CollectionChangeAction.Refresh, null));
        }
    }
}
