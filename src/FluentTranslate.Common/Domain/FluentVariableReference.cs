using System.Collections;

namespace FluentTranslate.Common.Domain
{
	public class FluentVariableReference : IFluentExpression
	{
		public string Id { get; set; }

		public FluentVariableReference()
		{
		}

		public FluentVariableReference(string id) : this()
		{
			Id = id;
		}

		public bool Equals(object other, IEqualityComparer comparer)
		{
			if (ReferenceEquals(other, this)) return true;
			if (other is null) return false;
			if (!(other is FluentVariableReference variableReference)) return false;
			return comparer.Equals(Id, variableReference.Id);
		}

		public int GetHashCode(IEqualityComparer comparer)
		{
			return comparer.GetHashCode(Id);
		}
	}
}
