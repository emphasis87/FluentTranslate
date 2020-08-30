﻿using System.Collections.Generic;

namespace FluentTranslate.Common.Domain
{
	public class FluentAttribute : IFluentElement
	{
		public string Id { get; set; }
		public IList<IFluentContent> Content { get; set; }

		public FluentAttribute()
		{
			Content = new List<IFluentContent>();
		}
	}
}