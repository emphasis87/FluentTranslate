using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Timers;

namespace FluentTranslate.Sources
{
    public class PhysicalResourceProvider : IResourceProvider
    {
        /// <summary>
        /// Path to a file or folder.
        /// </summary>
        public string Path { get; init; }

        public TimeSpan? PollingInterval { get; init; }

        public string Id { get; init; } = $"{Guid.NewGuid()}";

        private readonly IOptions<Options.PhysicalResourceProvider>? _options;
        private readonly ILogger<PhysicalResourceProvider>? _logger;

        public PhysicalResourceProvider(
            string path,
            IOptions<Options.PhysicalResourceProvider>? options = null,
            ILogger<PhysicalResourceProvider>? logger = null)
        {
            Path = path;

            _options = options;
            _logger = logger;
        }

        public PhysicalResourceProvider(
            string id, 
            string path,
            IOptions<Options.PhysicalResourceProvider>? options = null,
            ILogger<PhysicalResourceProvider>? logger = null)
            : this(path, options, logger)
        {
            Id = id;
        }

        private bool _enableRaisingEvents;
        public bool EnableRaisingEvents
        {
            get => _enableRaisingEvents;
            set
            {
                if (value)
                {
                    EnsureInitialized();
                }
                else
                {
                    _enableRaisingEvents = false;
                    _watcher.EnableRaisingEvents = false;
                    _timer.Enabled = false;
                }
            }
        }

        private readonly System.Timers.Timer _timer = new();
        private readonly FileSystemWatcher _watcher = new();

        private double GetPollingInterval()
        {
            return (
                PollingInterval
                ?? _options?.Value.PollingInterval
                ?? TimeSpan.FromSeconds(1)
                ).TotalMilliseconds;

        }

        private void EnsureInitialized()
        {
            try
            {
                _timer.Enabled = false;
                _timer.AutoReset = true;
                _timer.Interval = GetPollingInterval();
                _timer.Elapsed -= OnTimerElapsed;
                _timer.Elapsed += OnTimerElapsed;

                _watcher.Error -= OnWatcherError;
                _watcher.Error += OnWatcherError;
                _watcher.Path = Path;
                _watcher.IncludeSubdirectories = true;
                _watcher.Created -= OnFileCreated;
                _watcher.Created += OnFileCreated;
                _watcher.Changed -= OnFileChanged;
                _watcher.Changed += OnFileChanged;
                _watcher.Renamed -= OnFileRenamed;
                _watcher.Renamed += OnFileRenamed;
                _watcher.Deleted -= OnFileDeleted;
                _watcher.Deleted += OnFileDeleted;
                _watcher.EnableRaisingEvents = true;

                _enableRaisingEvents = true;
            }
            catch (Exception ex)
            {
                _timer.Enabled = true;
                _logger?.LogWarning(ex, $"Error initializing file system watcher.");
            }
        }

        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            LoadResources();
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            LoadResources();
        }

        private void OnFileRenamed(object sender, RenamedEventArgs e)
        {
            LoadResources();
        }

        private void OnFileDeleted(object sender, FileSystemEventArgs e)
        {
            LoadResources();
        }

        private void OnWatcherError(object sender, ErrorEventArgs e)
        {
            _timer.Enabled = true;
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            EnsureInitialized();
        }

        private void LoadResources()
        {
            try
            {
                var resources = new List<IResource>();
                var entries = Directory.GetFileSystemEntries(Path, $"*.{Constants.FileExtension}", SearchOption.AllDirectories);
                foreach (var entry in entries)
                {
                    if (!File.Exists(entry))
                        continue;

                    var file = new FileInfo(entry);
                    var path = file.FullName;
                    var lastWrite = file.LastWriteTimeUtc;
                    if (_resourceFiles.TryGetValue(path, out var r) && r.LastWrite == lastWrite)
                    {
                        resources.Add(r.Resource);
                    }
                    else
                    {
                        var resource = LoadResource(file);
                        resources.Add(resource);
                        _resourceFiles[path] = (lastWrite, resource);
                    }
                }
            }
            catch (Exception ex)
            {
                _timer.Enabled = true;
                _logger?.LogWarning(ex, $"Error loading files.");
            }
        }

        private IResource LoadResource(FileInfo file)
        {
            try
            {
                return new Resource();
            }
            catch(Exception ex)
            {
                _timer.Enabled = true;
                _logger?.LogWarning(ex, "Error loading file: {file}", file.FullName);
            }

            return Resource.Empty;
        }

        private readonly Dictionary<string, (DateTime LastWrite, IResource Resource)> _resourceFiles = new();
        private IResource[] _resources = Array.Empty<IResource>();
        public IReadOnlyCollection<IResource> Resources => _resources;

        public event CollectionChangeEventHandler ResourcesChanged = delegate { };

        public event EventHandler OnError = delegate { };
    }
}
