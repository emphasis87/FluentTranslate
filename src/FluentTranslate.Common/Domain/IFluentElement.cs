using System.Collections;

namespace FluentTranslate.Common.Domain
{
    public interface IFluentElement : IStructuralEquatable
    {
        string Type { get; }
    }
}