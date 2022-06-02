using System.Collections;
using System.Collections.Generic;

namespace FluentTranslate.Domain.Common
{
	public interface IFluentRecord : IFluentContainer, IFluentAttributable, IFluentResourceEntry, IFluentReference
	{
		string Comment { get; }
		string Identifier { get; }
	}

	public abstract class FluentRecord : FluentContainer, IFluentRecord, IEnumerable<IFluentContent>, IEnumerable<FluentAttribute>
	{
		public string Comment { get; set; }
		public string Identifier { get; set; }
		public abstract string Reference { get; }

		public List<FluentAttribute> Attributes { get; }

        protected FluentRecord()
		{
			Attributes = new List<FluentAttribute>();
		}

        public void Add(FluentAttribute attribute) => Attributes.Add(attribute);
		IEnumerator<FluentAttribute> IEnumerable<FluentAttribute>.GetEnumerator() => Attributes.GetEnumerator();
    }
}