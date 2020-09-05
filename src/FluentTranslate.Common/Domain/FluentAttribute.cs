using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace FluentTranslate.Common.Domain
{
	public class FluentAttribute : IFluentElement, IFluentContainer
	{
		public string Id { get; set; }
		public IList<IFluentContent> Content { get; set; }

		public FluentAttribute()
		{
			Content = new List<IFluentContent>();
		}

        public bool Equals(object other, IEqualityComparer comparer)
        {
            if (ReferenceEquals(other, this)) return true;
            if (other is null) return false;
            if (!(other is FluentAttribute attribute)) return false;
			return comparer.Equals(Id, attribute.Id)
				&& comparer.Equals(Content?.ToImmutableArray(), attribute.Content?.ToImmutableArray());
		}

        public int GetHashCode(IEqualityComparer comparer)
        {
            return comparer.GetHashCode(Id);
        }
    }
}