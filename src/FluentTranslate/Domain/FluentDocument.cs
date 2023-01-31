using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
	public class FluentDocument : FluentElement, IEnumerable<IFluentResourceEntry>
	{
        public List<IFluentResourceEntry> Content { get; }

		public FluentDocument()
		{
			Content = new List<IFluentResourceEntry>();
		}

        public void Add(IFluentResourceEntry entry) => Content.Add(entry);
        public IEnumerator<IFluentResourceEntry> GetEnumerator() => Content.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
	