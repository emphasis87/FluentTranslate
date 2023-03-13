using FluentTranslate.Common;
using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
	[Entity(FluentTranslateEntities.Attribute)]
	public class FluentAttribute : FluentContainer, 
		IEnumerable<IFluentContent>
    {
        public string Id { get; set; } = default!;

		public FluentAttribute()
		{
			
		}

		public FluentAttribute(string id) : this()
		{
			Id = id;
		}
    }
}