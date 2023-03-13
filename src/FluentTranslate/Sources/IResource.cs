using FluentTranslate.Domain;

namespace FluentTranslate.Sources
{
    public interface IResource
    {
        string Id { get; }
        IReadOnlyCollection<IFluentEntry> Entries { get; }
        event CollectionChangeEventHandler EntriesChanged;
    }

    public class Resource : IResource
    {
        public string Id { get; init; } = $"{Guid.NewGuid()}";

        private readonly FluentResource _resource;

        public Resource(FluentResource resource)
        {
            _resource = resource;
            _entries = _resource.Content.ToArray();
        }

        public Resource(string id, FluentResource resource)
            : this(resource)
        {
            Id = id;
        }

        private IFluentEntry[] _entries = Array.Empty<IFluentEntry>();
        public IReadOnlyCollection<IFluentEntry> Entries => _entries;
        public event CollectionChangeEventHandler EntriesChanged = delegate { };

        public readonly static Resource Empty = default;
    }
}
