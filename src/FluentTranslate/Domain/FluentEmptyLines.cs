using FluentTranslate.Common;
using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
    [Entity(FluentTranslateEntities.EmptyLines)]
    public class FluentEmptyLines : FluentElement, IFluentEntry
	{
		public int Count { get; set; }
	}
}
