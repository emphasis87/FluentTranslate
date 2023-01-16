using FluentTranslate.Common;
using FluentTranslate.Services;

namespace FluentTranslate.Domain.Common
{
	public abstract class FluentElement : IFluentElement, IEquatable<IFluentElement>
	{
		public string Type { get; }

		public FluentElement()
        {
			Type = FluentTypes.GetType(this);
		}

		public override bool Equals(object other)
		{
			if (other is null) return false;
			if (ReferenceEquals(other, this)) return true;
			if (other.GetType() != GetType()) return false;
			return Equals((IFluentElement) other);
		}

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