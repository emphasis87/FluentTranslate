namespace FluentTranslate.Domain.Common
{
    public abstract class FluentContainer : FluentElement, IFluentContainer
    {
        public List<IFluentContent> Content { get; init; } = new();

        protected FluentContainer()
        {
            
        }

        public void Add(IFluentContent content) => Content.Add(content);
        IEnumerator<IFluentContent> IEnumerable<IFluentContent>.GetEnumerator() => Content.GetEnumerator();

        public virtual IEnumerator GetEnumerator() => Content.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
