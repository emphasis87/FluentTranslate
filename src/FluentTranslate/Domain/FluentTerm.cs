using System.Collections;
using System.Linq;

using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
	public class FluentTerm : FluentRecord
    {
		public override string Reference => $"-{Identifier}";

		public FluentTerm()
		{
		}

		public FluentTerm(string id) : this()
		{
			Identifier = id;
		}

		public FluentTerm(string id, string comment) : this()
		{
			Identifier = id;
			Comment = comment;
		}

		public override IEnumerator GetEnumerator() => Attributes.Cast<IFluentElement>().Concat(Content).GetEnumerator();
	}
}