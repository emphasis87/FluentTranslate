using System.Collections;
using System.Collections.Generic;

namespace FluentTranslate.Common.Domain
{
	public abstract class FluentRecord : IFluentContainer, IEnumerable<IFluentContent>, IEnumerable<FluentAttribute>
	{
		public string Comment { get; set; }
		public string Id { get; set; }
		public IList<IFluentContent> Content { get; set; }
		public IList<FluentAttribute> Attributes { get; set; }

		public abstract string Reference { get; }

		protected FluentRecord()
		{
			Content = new List<IFluentContent>();
			Attributes = new List<FluentAttribute>();
		}

        public abstract bool Equals(object other, IEqualityComparer comparer);
        public abstract int GetHashCode(IEqualityComparer comparer);

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