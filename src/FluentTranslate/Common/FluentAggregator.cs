using FluentTranslate.Domain;
using System.Collections.Generic;
using System.Linq;

using static FluentTranslate.Common.EqualityHelper;

namespace FluentTranslate.Common
{
    public class FluentAggregator
    {
        public static bool CanAggregate(IFluentElement e1, IFluentElement e2)
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

        public static bool CanAggregate(IEnumerable<IFluentElement> elements!!)
        {
            return ZipOrDefault(elements, elements.Skip(1), CanAggregate).All(r => r);
        }

        public static IFluentElement Aggregate(IEnumerable<IFluentElement> elements!!)
        {
            var all = elements.Where(c => c is not null).ToArray();
            if (all.Length == 0) return default;
            if (all.Length == 1) return all[0];

            var first = all[0];
            var last = all[all.Length - 1];

            switch (first)
            {
                case FluentEmptyLines empty:
                    empty.Count += all.Cast<FluentEmptyLines>().Sum(x => x.Count);
                    return empty;
                case FluentText text:
                    var texts = all.Cast<FluentText>().Select(x => x.Value).Where(v => v is not null).ToArray();
                    var value = string.Join("\r\n", texts);
                    text.Value = value;
                    return text;
                case FluentComment comment1:
                    var comments = all.OfType<FluentComment>().Select(x => x.Value);
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
                        comment1.Value = comment;
                        return comment1;
                    }
                default:
                    return default;
            }
        }
    }
}
