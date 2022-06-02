namespace FluentTranslate.Domain
{
	public class FluentMessage : FluentRecord
    {
        public override string Type => FluentElementTypes.Message;
        public override string Reference => Id;

        public FluentMessage()
		{
		}

		public FluentMessage(string id) : this()
		{
			Id = id;
		}

		public FluentMessage(string id, string comment) : this()
		{
			Id = id;
			Comment = comment;
		}
    }
}
