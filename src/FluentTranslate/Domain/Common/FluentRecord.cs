namespace FluentTranslate.Domain.Common
{
	public interface IFluentRecord : IFluentContainer, IFluentAttributable, IFluentEntry, IFluentReferencable
	{
		string? Comment { get; set; }
		string Id { get; set; }
	}

	public abstract partial class FluentRecord : FluentContainer, IFluentRecord, 
		IEnumerable<IFluentContent>, 
		IEnumerable<FluentAttribute>
    {
		public string? Comment { get; set; }
		public string Id { get; set; } = default!;

		public List<FluentAttribute> Attributes { get; init; } = new();

		public virtual string Reference => Id;

        protected FluentRecord()
		{
			
		}

        public void Add(FluentAttribute attribute) => Attributes.Add(attribute);
		IEnumerator<FluentAttribute> IEnumerable<FluentAttribute>.GetEnumerator() => Attributes.GetEnumerator();
    }
}