using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
	public class FluentTermReference : FluentRecordReference, IFluentCallable, IFluentAggregable, IEnumerable<FluentCallArgument>
    {
        public override string TargetReference => TargetAttributeId switch
        {
            null => $"-{TargetId}",
            _ => $"-{TargetId}.{TargetAttributeId}"
        };
        public List<FluentCallArgument> Arguments { get; set; }

		public FluentTermReference()
		{
			Arguments = new List<FluentCallArgument>();
		}

		public FluentTermReference(string id) : this()
		{
			TargetId = id;
		}

		public FluentTermReference(string id, string attributeId) : this()
		{
			TargetId = id;
			TargetAttributeId = attributeId;
		}

        public void Add(FluentCallArgument argument)
		{
			Arguments.Add(argument);
		}

        public IEnumerator<FluentCallArgument> GetEnumerator() => Arguments.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

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
                    TargetId ??= reference.TargetId;
                    Arguments = Arguments.Concat(reference.Arguments).ToList();
                    return this;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(other));
            }
        }
    }
}