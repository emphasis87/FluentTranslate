using System;
using System.Collections.Generic;

namespace FluentTranslate.Service
{
	public class TranslationRecord
	{
		public string Name { get; set; }
		public string Path { get; set; }
		public string Content { get; set; }
		public DateTime LastModified { get; set; }
		public IDictionary<string, string> Attributes { get; set; }

		public TranslationRecord()
		{
			Attributes = new Dictionary<string, string>();
		}

        public void RemovedAt(DateTime lastModified)
        {
            Content = null;
            LastModified = lastModified;
			Attributes.Clear();
        }
	}
}