using FluentTranslate.Common;
using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
    [Entity(FluentTranslateEntities.Resource)]
    public class FluentResource : FluentElement, 
		IEnumerable<IFluentEntry>
	{
		public List<IFluentEntry> Content { get; init; } = new();

		public FluentResource()
		{
			
		}

        public void Add(IFluentEntry entry) => Content.Add(entry);
        public IEnumerator<IFluentEntry> GetEnumerator() => Content.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
	