using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
	public class FluentMessage : FluentRecord
    {
        public string? Comment { get; set; }

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
