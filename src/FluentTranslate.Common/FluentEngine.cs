using System;
using System.Collections.Generic;
using FluentTranslate.Common.Domain;

namespace FluentTranslate.Common
{
    public interface IFluentEngine
    {
        IFluentContainer FindByReference(string reference);
        object Call(string reference, IList<(string name, object value)> arguments);
    }

    public class FluentEngine : IFluentEngine
    {
        public IFluentContainer FindByReference(string reference)
        {
            
        }

        public object Call(string reference, IList<(string name, object value)> arguments)
        {
            
        }
    }
}