using FluentTranslate.Common;
using FluentTranslate.Domain.Common;

namespace FluentTranslate.Domain
{
    [Entity(FluentTranslateEntities.TermReference)]
    public class FluentTermReference : FluentRecordReference, IFluentCallable, IEnumerable<FluentCallArgument>
    {
        public override string Target => AttributeId switch
        {
            null => $"-{Id}",
            _ => $"-{Id}.{AttributeId}"
        };
        public List<FluentCallArgument> Arguments { get; set; }

		public FluentTermReference()
		{
			Arguments = new List<FluentCallArgument>();
		}

		public FluentTermReference(string id) : this()
		{
			Id = id;
		}

		public FluentTermReference(string id, string? attributeId) : this()
		{
			Id = id;
			AttributeId = attributeId;
		}

        public void Add(FluentCallArgument argument)
		{
			Arguments.Add(argument);
		}

        public IEnumerator<FluentCallArgument> GetEnumerator() => Arguments.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}