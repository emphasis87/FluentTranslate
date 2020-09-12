using System.Collections;

namespace FluentTranslate.Common.Domain
{
	public class FluentCallArgument : IFluentElement
    {
        public string Type => "argument";
        public string Id { get; set; }
		public IFluentExpression Value { get; set; }

		public FluentCallArgument()
		{
		}

		public FluentCallArgument(IFluentExpression value) : this()
		{
			Value = value;
		}

		public FluentCallArgument(string id, IFluentExpression value) : this()
		{
			Id = id;
			Value = value;
		}

		public bool Equals(object other, IEqualityComparer comparer)
        {
            if (ReferenceEquals(other, this)) return true;
            if (other is null) return false;
            if (!(other is FluentCallArgument argument)) return false;
            return comparer.Equals(Id, argument.Id)
				&& comparer.Equals(Value, argument.Value);
        }

        public int GetHashCode(IEqualityComparer comparer)
        {
            return comparer.GetHashCode(Id);
        }
    }
}