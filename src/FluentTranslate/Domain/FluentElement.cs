using System;
using FluentTranslate.Infrastructure;

namespace FluentTranslate.Domain
{
	public abstract class FluentElement : IFluentElement, IEquatable<IFluentElement>
	{
		public abstract string Type { get; }

		public bool Equals(IFluentElement other)
		{
			return FluentEqualityComparer.Default.Equals(this, other);
		}

		public override int GetHashCode()
		{
			return FluentEqualityComparer.Default.GetHashCode(this);
		}
	}
}