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

		public override bool Equals(object obj)
		{
			if (obj is null) return false;
			if (ReferenceEquals(obj, this)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((IFluentElement) obj);
		}

		public override int GetHashCode()
		{
			return FluentEqualityComparer.Default.GetHashCode(this);
		}
	}
}