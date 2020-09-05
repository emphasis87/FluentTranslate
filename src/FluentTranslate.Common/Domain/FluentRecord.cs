using System.Collections;
using System.Collections.Generic;

namespace FluentTranslate.Common.Domain
{
	public abstract class FluentRecord : IFluentContainer
	{
		public string Comment { get; set; }
		public string Id { get; set; }
		public IList<IFluentContent> Content { get; set; }
		public IList<FluentAttribute> Attributes { get; set; }

		protected FluentRecord()
		{
			Content = new List<IFluentContent>();
			Attributes = new List<FluentAttribute>();
		}

        public abstract bool Equals(object other, IEqualityComparer comparer);
        public abstract int GetHashCode(IEqualityComparer comparer);
    }
}