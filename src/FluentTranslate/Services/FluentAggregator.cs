using FluentTranslate.Domain;
using FluentTranslate.Domain.Common;

using static FluentTranslate.Common.Helpers;

namespace FluentTranslate.Services
{
    public interface IFluentAggregator
    {
        bool CanAggregate(IFluentElement e1, IFluentElement e2);
        bool CanAggregate(IEnumerable<IFluentElement> elements);
        IFluentElement Aggregate(IEnumerable<IFluentElement> elements);
    }

    public class FluentAggregator : IFluentAggregator
    {
        public virtual bool CanAggregate(IFluentElement e1, IFluentElement e2)
        {
            if (e1 is null || e2 is null) return true;
            if (ReferenceEquals(e1, e2)) return true;
            return (e1, e2) switch
            {
                (FluentComment c1, FluentComment c2) when c1.Level == c2.Level => true,
                (FluentComment c1, FluentRecord) when c1.Level == 1 => true,
                (FluentEmptyLines, FluentEmptyLines) => true,
                (FluentText, FluentText) => true,
                _ => false
            };
        }

        public virtual bool CanAggregate(IEnumerable<IFluentElement> elements)
        {
            return ZipOrDefault(elements, elements.Skip(1), CanAggregate).All(r => r);
        }

        public virtual IFluentElement Aggregate(IEnumerable<IFluentElement> elements)
        {
            var all = elements.Where(c => c is not null).ToArray();
            if (all.Length == 0) return default;
            if (all.Length == 1) return all[0];

            var first = all[0];
            var last = all[all.Length - 1];

            switch (first)
            {
                case FluentEmptyLines empty:
                    return AggregateEmptyLines(all, empty);
                case FluentText text:
                    return AggregateText(all, text);
                case FluentComment comment1:
                    return AggregateComments(all, comment1);
                default:
                    return default;
            }
        }

        public virtual IFluentElement AggregateEmptyLines(IFluentElement[] elements, FluentEmptyLines first)
        {
            first.Count = elements.Cast<FluentEmptyLines>().Sum(x => x.Count);
            return first;
        }

        public virtual IFluentElement AggregateText(IFluentElement[] elements, FluentText first)
        {
            var texts = elements.Cast<FluentText>().Select(x => x.Value).Where(v => v is not null);
            var value = string.Join("\r\n", texts);
            first.Value = value;
            return first;
        }

        public virtual IFluentElement AggregateComments(IFluentElement[] elements, FluentComment first)
        {
            var last = elements[elements.Length - 1];
            var comments = elements.OfType<FluentComment>().Select(x => x.Value);
            if (last is FluentRecord r1)
                comments = comments.Concat(new[] { r1.Comment });
            var comment = string.Join("\r\n", comments.Where(x => x is not null));
            if (last is FluentRecord r2)
            {
                r2.Comment = comment;
                return r2;
            }
            else
            {
                first.Value = comment;
                return first;
            }
        }
    }
}
