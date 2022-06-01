using System.Collections;
using System.Collections.Generic;

namespace FluentTranslate.Domain
{
	public interface IFluentRecord : IFluentContainer, IFluentAttributable, IFluentEntry, IFluentReference
	{
		string Comment { get; }
		string Id { get; }
		
	}

	public abstract class FluentRecord : FluentElement, IFluentRecord, IEnumerable<IFluentContent>, IEnumerable<FluentAttribute>
	{
		public abstract string Type { get; }
		public string Comment { get; set; }
		public string Id { get; set; }
		public abstract string Reference { get; }
		public List<IFluentContent> Content { get; set; }
		public List<FluentAttribute> Attributes { get; set; }

        protected FluentRecord()
		{
			Content = new List<IFluentContent>();
			Attributes = new List<FluentAttribute>();
		}

        public void Add(IFluentContent content)
		{
			Content.Add(content);
		}

		public void Add(FluentAttribute attribute)
		{
			Attributes.Add(attribute);
		}

		public IEnumerator<IFluentContent> GetEnumerator() => Content.GetEnumerator();
		IEnumerator<FluentAttribute> IEnumerable<FluentAttribute>.GetEnumerator() => Attributes.GetEnumerator();
		IEnumerator<IFluentContent> IEnumerable<IFluentContent>.GetEnumerator() => Content.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}