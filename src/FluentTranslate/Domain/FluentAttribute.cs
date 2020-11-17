using System.Collections;
using System.Collections.Generic;

namespace FluentTranslate.Domain
{
	public class FluentAttribute : FluentElement, IFluentContainer, IEnumerable<IFluentContent>
    {
        public override string Type => "attribute";

        public string Id { get; set; }
		public List<IFluentContent> Content { get; set; }

		public FluentAttribute()
		{
			Content = new List<IFluentContent>();
		}

		public FluentAttribute(string id) : this()
		{
			Id = id;
		}

		public IEnumerator<IFluentContent> GetEnumerator()
		{
			return Content.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(IFluentContent content)
		{
			Content.Add(content);
		}
	}
}