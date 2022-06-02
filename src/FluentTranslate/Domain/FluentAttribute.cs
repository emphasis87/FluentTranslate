using System.Collections;
using System.Collections.Generic;

using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
	public class FluentAttribute : FluentContainer, IEnumerable<IFluentContent>
    {
        public string Identifier { get; set; }

		public FluentAttribute()
		{
			
		}

		public FluentAttribute(string id) : this()
		{
			Identifier = id;
		}

        public override IEnumerator GetEnumerator() => Content.GetEnumerator();
    }
}