using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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
            return Id == attribute.Id &&
                   comparer.Equals(Content, attribute.Content);
        }

        public int GetHashCode(IEqualityComparer comparer)
        {
            return RuntimeHelpers.GetHashCode(Id);
        }
    }
}