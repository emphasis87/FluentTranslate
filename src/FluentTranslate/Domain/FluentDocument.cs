using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
	public class FluentDocument : FluentElement, 
		IEnumerable<IFluentDocumentItem>
	{
		public List<IFluentDocumentItem> Content { get; init; } = new();

		public FluentDocument()
		{
			
		}

        public void Add(IFluentDocumentItem entry) => Content.Add(entry);
        public IEnumerator<IFluentDocumentItem> GetEnumerator() => Content.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
	