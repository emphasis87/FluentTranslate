using System;
using System.Collections;
using System.Collections.Generic;

namespace FluentTranslate.Domain
{
    public class FluentResource : FluentElement, IEnumerable<IFluentEntry>
	{
        public override string Type => FluentElementTypes.Resource;
        public List<IFluentEntry> Entries { get; set; }

		public FluentResource()
		{
			Entries = new List<IFluentEntry>();
		}

        public void Add(IFluentEntry entry)
		{
			Entries.Add(entry);
		}

		public IEnumerator<IFluentEntry> GetEnumerator() => Entries.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
