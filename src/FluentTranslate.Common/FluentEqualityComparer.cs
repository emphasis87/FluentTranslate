using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using FluentTranslate.Common.Domain;

namespace FluentTranslate.Common
{
    public class FluentEqualityComparer : 
        IEqualityComparer<IFluentElement>
    {
        public bool Equals(IFluentElement x, IFluentElement y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x is null) return false;
            if (y is null) return false;
            if (x.GetType() != y.GetType()) return false;
            return StructuralComparisons.StructuralEqualityComparer.Equals(x, y);
        }

        public int GetHashCode(IFluentElement element)
        {
            return StructuralComparisons.StructuralEqualityComparer.GetHashCode(element);
        }
    }
}
