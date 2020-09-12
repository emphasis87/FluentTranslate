using System.Collections;

namespace FluentTranslate.Common.Domain
{
	public class FluentPlaceable : IFluentContent, IFluentExpression
    {
        public string Type => "placeable";
        public IFluentExpression Content { get; set; }

		public FluentPlaceable()
		{
		}

		public FluentPlaceable(IFluentExpression content) : this()
		{
			Content = content;
		}

		public bool Equals(object other, IEqualityComparer comparer)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other is null) return false;
			if (!(other is FluentPlaceable placeable)) return false;
			return comparer.Equals(Content, placeable.Content);
		}

		public int GetHashCode(IEqualityComparer comparer)
		{
			return comparer.GetHashCode(Content);
		}
	}
}
