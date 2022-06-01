using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FluentTranslate.Domain
{
	public class FluentTermReference : FluentRecordReference, IFluentCallable, IFluentAggregable, IEnumerable<FluentCallArgument>
    {
        public override string Type { get; } = FluentElementTypes.TermReference;

        public List<FluentCallArgument> Arguments { get; set; }

		public FluentTermReference()
		{
			Arguments = new List<FluentCallArgument>();
		}

		public FluentTermReference(string id) : this()
		{
			Id = id;
		}

		public FluentTermReference(string id, string attributeId) : this()
		{
			Id = id;
			AttributeId = attributeId;
		}

        public override string Reference => AttributeId switch
        {
            null => $"-{Id}",
            _ => $"-{Id}.{AttributeId}"
        };

		public IEnumerator<FluentCallArgument> GetEnumerator()
		{
			return Arguments.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(FluentCallArgument argument)
		{
			Arguments.Add(argument);
		}

        public bool CanAggregate(object other)
        {
            if (ReferenceEquals(this, other)) return false;
            if (other is null) return false;
            return other is FluentTermReference;
        }

        public object Aggregate(object other)
        {
            switch (other)
            {
                case FluentTermReference reference:
                {
                    Id ??= reference.Id;
                    Arguments = Arguments.Concat(reference.Arguments).ToList();
                    return this;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(other));
            }
        }
    }
}