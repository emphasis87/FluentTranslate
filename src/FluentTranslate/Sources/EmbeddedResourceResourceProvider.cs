using FluentTranslate.Domain;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace FluentTranslate.Sources
{
    public class EmbeddedResourceResourceProvider : IResourceProvider
    {
        public string? Path { get; init; }
        public Assembly? Assembly { get; init; }

        private readonly ILogger<EmbeddedResourceResourceProvider>? _logger;

        public EmbeddedResourceResourceProvider(ILogger<EmbeddedResourceResourceProvider>? logger = null)
        {
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
                    EnsureInitialized();
            }
        }

        private bool _isInitialized;

        private void EnsureInitialized()
        {
            if (_isInitialized)
                return;

            try
            {
                var assembly = Assembly ?? Assembly.GetEntryAssembly();
                //var satellite = assembly.GetSatelliteAssembly(CultureInfo.CurrentUICulture);
                var names = assembly.GetManifestResourceNames();

                var m = new Matcher(StringComparison.OrdinalIgnoreCase);
                m.AddInclude(Path ?? "*.ftl");

                var resources = new List<IResource>();
                foreach (var name in names)
                {
                    if (!m.Match(name).HasMatches)
                        continue;
                    
                    var info = assembly.GetManifestResourceInfo(name);
                    var resource = Read(assembly, name);
                    if (resource is not null)
                        resources.Add(new Resource(name, resource));
                }

                _resources = resources.ToArray();
                ResourcesChanged(this, new(CollectionChangeAction.Refresh, null));

                _isInitialized = true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error initializing resources.");
            }
        }

        public FluentResource? Read(Assembly assembly, string path)
        {
            try
            {
                using var stream = assembly.GetManifestResourceStream(path);
                if (stream is null)
                    return null;

                using var reader = new StreamReader(stream);
                var source = reader.ReadToEnd();
                var extension = System.IO.Path.GetExtension(path);
                var result = FluentSerializer.Deserialize(source);

                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Unable to read a fluent file: {Assembly}, {Path}", assembly, path);
            }

            return null;
        }
    }
}
