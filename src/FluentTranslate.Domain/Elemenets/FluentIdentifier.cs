namespace FluentTranslate.Domain
{
    public interface IFluentIdentifier : IFluentElement, IFluentVariantKey
	{

    }

	public class FluentIdentifier : FluentElement, IFluentIdentifier
    {
        public override string Type => FluentElementTypes.Identifier;
        public string Id { get; set; }

		public FluentIdentifier()
		{
		}

		public FluentIdentifier(string id)
		{
			Id = id;
		}
    }
}