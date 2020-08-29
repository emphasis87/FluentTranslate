﻿using System.Collections.Generic;

namespace FluentTranslate.Common.Domain
{
	public class FluentFunctionCall : IFluentExpression
	{
		public string Id { get; set; }
		public IList<FluentCallArgument> Arguments { get; set; }

		public FluentFunctionCall()
		{
			Arguments = new List<FluentCallArgument>();
		}
	}
}