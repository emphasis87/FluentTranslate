using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
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