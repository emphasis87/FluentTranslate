namespace FluentTranslate.Domain.Common
{
    public abstract class FluentContainer : FluentElement, IFluentContainer
    {
        public List<IFluentContent> Content { get; }

        protected FluentContainer()
        {
            Content = new List<IFluentContent>();
        }

        public void Add(IFluentContent content) => Content.Add(content);
        IEnumerator<IFluentContent> IEnumerable<IFluentContent>.GetEnumerator() => Content.GetEnumerator();

        public abstract IEnumerator GetEnumerator();
    }
}
