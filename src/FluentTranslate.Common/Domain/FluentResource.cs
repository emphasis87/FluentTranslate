using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace FluentTranslate.Common.Domain
{
    public class FluentResource : IFluentElement, IEnumerable<IFluentEntry>
    {
        public string Type => "resource";
        public IList<IFluentEntry> Entries { get; set; }

		public FluentResource()
		{
			Entries = new List<IFluentEntry>();
		}

        public bool Equals(object other, IEqualityComparer comparer)
        {
            if (ReferenceEquals(other, this)) return true;
            if (other is null) return false;
            if (!(other is FluentResource resource)) return false;
            return comparer.Equals(Entries?.ToImmutableArray(), resource.Entries?.ToImmutableArray());
        }

        public int GetHashCode(IEqualityComparer comparer)
        {
            return comparer.GetHashCode(Entries?.ToImmutableArray());
        }

		public IEnumerator<IFluentEntry> GetEnumerator()
		{
			return Entries.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(IFluentEntry entry)
		{
			Entries.Add(entry);
		}
	}
}
