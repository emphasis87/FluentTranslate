namespace FluentTranslate.Sources
{
    public interface IResourceProvider
    {
        string Id { get; init; }
        IReadOnlyCollection<IResource> Resources { get; }
        event CollectionChangeEventHandler ResourcesChanged;
        bool EnableRaisingEvents { get; set; }
    }
}
