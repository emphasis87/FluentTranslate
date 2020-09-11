using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data.HashFunction;
using System.Data.HashFunction.xxHash;
using System.Linq;

namespace FluentTranslate.Service.Domain
{
	public class TranslationEntry : IStructuralEquatable
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Path { get; set; }
		public string Content { get; set; }
		public DateTime LastModified { get; set; }
		public IDictionary<string, object> Attributes { get; protected set; }
		public string ETag { get; set; }

		public TranslationEntry()
		{
			Attributes = new SortedDictionary<string, object>();
		}

		public TranslationEntry(IDictionary<string, object> attributes)
		{
			Attributes = attributes == null
				? new SortedDictionary<string, object>()
				: new SortedDictionary<string, object>(attributes);
		}

		public void RemovedAt(DateTime lastModified)
        {
			Content = null;
			Attributes.Clear();
			ETag = ComputeETag();
			LastModified = lastModified;
		}

		public string ComputeId()
		{
			var attributes = string.Join(",", Attributes.Select(x => $"{x.Key}:{x.Value}"));
			return $"{Name};{{{attributes}}}";
		}

		public string ComputeETag()
		{
			var hash = xxHashFactory.Instance.Create();
			var content = $"{ComputeId()};{Content}";
			return hash.ComputeHash(content).AsHexString();
		}

		public void UpdateId()
		{
			Id = ComputeId();
		}

		public void UpdateETag()
		{
			ETag = ComputeETag();
		}

		public bool Equals(object other, IEqualityComparer comparer)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other is null) return false;
			if (!(other is TranslationEntry entry)) return false;
			return comparer.Equals(Id, entry.Id);
		}

		public int GetHashCode(IEqualityComparer comparer)
		{
			return Id.GetHashCode();
		}
	}

	public class TranslationEntryEqualityComparer : IEqualityComparer<TranslationEntry>
	{
		public bool Equals(TranslationEntry x, TranslationEntry y)
		{
			if (ReferenceEquals(x, y)) return true;
			if (x is null) return false;
			if (y is null) return false;
			if (x.GetType() != y.GetType()) return false;
			return StructuralComparisons.StructuralEqualityComparer.Equals(x, y);
		}

		public int GetHashCode(TranslationEntry obj)
		{
			return StructuralComparisons.StructuralEqualityComparer.GetHashCode(obj);
		}
	}
}