namespace FluentTranslate.Domain
{
	public class FluentTerm : FluentRecord
    {
        public override string Type => "term";
		public override string Reference => $"-{Id}";

		public FluentTerm()
		{
		}

		public FluentTerm(string id) : this()
		{
			Id = id;
		}

		public FluentTerm(string id, string comment) : this()
		{
			Id = id;
			Comment = comment;
		}
	}
}