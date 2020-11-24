using System.Collections.Generic;
using static FluentTranslate.WebHost.Infrastructure.EqualityHelper;

namespace FluentTranslate.WebHost.Infrastructure
{
	public interface IFluentFileGeneratorOptionsEqualityComparer : 
		IEqualityComparer<FluentTranslateOptions>,
		IEqualityComparer<FluentGenerateFileOptions>
	{

	}

	public class FluentFileGeneratorOptionsEqualityComparer : IFluentFileGeneratorOptionsEqualityComparer
	{
		public static IFluentFileGeneratorOptionsEqualityComparer Default { get; } = new FluentFileGeneratorOptionsEqualityComparer();

		public bool Equals(FluentTranslateOptions x, FluentTranslateOptions y)
		{
			if (ReferenceEquals(x, y)) return true;
			if (x is null || y is null) return false;
			if (x.GetType() != y.GetType()) return false;
			return AreEqual(x.GenerateFiles, y.GenerateFiles, this);
		}

		public bool Equals(FluentGenerateFileOptions x, FluentGenerateFileOptions y)
		{
			if (x is null || y is null) return false;
			if (ReferenceEquals(x, y)) return true;
			if (x.GetType() != y.GetType()) return false;
			return AreEqual(x.Name, y.Name)
				&& AreEqual(x.Sources, y.Sources);
		}

		public int GetHashCode(FluentTranslateOptions obj)
		{
			return Hash(obj.GenerateFiles);
		}

		public int GetHashCode(FluentGenerateFileOptions obj)
		{
			return Combine(obj.Name, obj.Sources);
		}
	}
}
