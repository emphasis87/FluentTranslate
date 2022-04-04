using System.Collections;
using System.Collections.Generic;

namespace FluentTranslate.Domain
{
	public abstract class FluentRecord : FluentElement, IFluentContainer, IFluentEntry, IFluentReference, IEnumerable<IFluentContent>, IEnumerable<FluentAttribute>
	{
        public string Comment { get; set; }
		public string Id { get; set; }
		public List<IFluentContent> Content { get; set; }
		public List<FluentAttribute> Attributes { get; set; }

		public abstract string Reference { get; }

		protected FluentRecord()
		{
			Content = new List<IFluentContent>();
			Attributes = new List<FluentAttribute>();
		}

		public IEnumerator<IFluentContent> GetEnumerator()
		{
			return Content.GetEnumerator();
		}

		IEnumerator<FluentAttribute> IEnumerable<FluentAttribute>.GetEnumerator()
		{
			return Attributes.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(IFluentContent content)
		{
			Content.Add(content);
		}

		public void Add(FluentAttribute attribute)
		{
			Attributes.Add(attribute);
		}
	}

}