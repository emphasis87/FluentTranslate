using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentTranslate.Service.Domain
{
	public class TranslationList
	{
		public string Id { get; set; }
		public DateTime LastModified { get; set; }
		public IDictionary<string, object> Attributes { get; protected set; }

		public TranslationList()
		{
			Attributes = new SortedDictionary<string, object>();
		}

		public TranslationList(IDictionary<string, object> attributes)
		{
			Attributes = attributes == null
				? new SortedDictionary<string, object>()
				: new SortedDictionary<string, object>(attributes);
		}

		public string ComputeId()
		{
			var attributes = string.Join(",", Attributes.Select(x => $"{x.Key}:{x.Value}"));
			return $"{{{attributes}}}";
		}

		public void UpdateId()
		{
			Id = ComputeId();
		}

	}
}
