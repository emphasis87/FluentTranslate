using System.Collections.Generic;

namespace FluentTranslate.Common.Domain
{
    public class FluentResource : IFluentElement
    {
        public IList<IFluentEntry> Entries { get; set; }

		public FluentResource()
		{
			Entries = new List<IFluentEntry>();
		}
	}
}
