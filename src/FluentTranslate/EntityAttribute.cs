using System;
using System.Collections.Generic;
using System.Text;

namespace FluentTranslate
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EntityAttribute : Attribute
    {
        public string Type { get; init; } 

        public EntityAttribute(string type)
        {
            Type = type;
        }
    }
}
