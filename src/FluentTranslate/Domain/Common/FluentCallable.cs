namespace FluentTranslate.Domain.Common
{
    public abstract class FluentCallable : FluentElement, IEnumerable<FluentAttribute>
    {
        public List<FluentAttribute> Attributes { get; }

        protected FluentCallable()
        {
            Attributes = new List<FluentAttribute>();
        }

        public void Add(FluentAttribute attribute) => Attributes.Add(attribute);
        IEnumerator<FluentAttribute> IEnumerable<FluentAttribute>.GetEnumerator() => Attributes.GetEnumerator();

        public abstract IEnumerator GetEnumerator();
    }
}
