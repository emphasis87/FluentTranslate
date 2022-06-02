using System;
using System.Collections.Generic;
using System.Text;

namespace FluentTranslate.Domain
{
    public interface IFluentAttributable : IFluentElement, IEnumerable<FluentAttribute>
    {
        List<FluentAttribute> Attributes { get; }
        void Add(FluentAttribute attribute);
    }
}
