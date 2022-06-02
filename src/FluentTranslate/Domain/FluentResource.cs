using System.Collections;
using System.Collections.Generic;

using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
    public class FluentResource : FluentElement, IEnumerable<IFluentResourceEntry>
	{
        public List<IFluentResourceEntry> Entries { get; }

		public FluentResource()
		{
			Entries = new List<IFluentResourceEntry>();
		}

        public void Add(IFluentResourceEntry entry) => Entries.Add(entry);
        public IEnumerator<IFluentResourceEntry> GetEnumerator() => Entries.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
