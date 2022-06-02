using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FluentTranslate.Domain.Domain
{
    public abstract class FluentContainer : IFluentContainer
    {
        public abstract string Type { get; }
        public List<IFluentContent> Content { get; set; }

        public void Add(IFluentContent content)
        {
            Content.Add(content);
        }

        public IEnumerator<IFluentContent> GetEnumerator() => Content.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
