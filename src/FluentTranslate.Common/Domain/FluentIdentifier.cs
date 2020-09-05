﻿using System.Collections;

namespace FluentTranslate.Common.Domain
{
	public class FluentIdentifier : IFluentElement, IFluentVariantKey
	{
		public string Id { get; set; }

        public bool Equals(object other, IEqualityComparer comparer)
        {
            if (ReferenceEquals(other, this)) return true;
            if (other is null) return false;
            if (!(other is FluentIdentifier identifier)) return false;
            return comparer.Equals(Id, identifier.Id);
        }

        public int GetHashCode(IEqualityComparer comparer)
        {
            return comparer.GetHashCode(Id);
        }
    }
}