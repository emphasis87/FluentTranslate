using System.Collections;
using System.Linq;

using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
	public class FluentMessage : FluentRecord
    {
        public override string Reference => Identifier;

        public FluentMessage()
		{
		}

		public FluentMessage(string id) : this()
		{
			Identifier = id;
		}

		public FluentMessage(string id, string comment) : this()
		{
			Identifier = id;
			Comment = comment;
		}

		public override IEnumerator GetEnumerator() => Attributes.Cast<IFluentElement>().Concat(Content).GetEnumerator();
    }
}
