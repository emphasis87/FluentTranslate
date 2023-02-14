using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
	public class FluentDocument : FluentElement, 
		IEnumerable<IFluentEntry>
	{
		public List<IFluentEntry> Content { get; init; } = new();

		public FluentDocument()
		{
			
		}

        public void Add(IFluentEntry entry) => Content.Add(entry);
        public IEnumerator<IFluentEntry> GetEnumerator() => Content.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
	