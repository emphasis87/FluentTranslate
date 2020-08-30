using System.Collections.Generic;

namespace FluentTranslate.Common.Domain
{
	public abstract class FluentRecord : IFluentElement, IFluentContainer
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
	}
}