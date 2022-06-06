using FluentTranslate.Serialization.Fluent;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentTranslate.Readers.Fluent
{
    public interface IFluentReader
    {

    }

    public class FluentReader : IFluentReader
    {
        public IFluentDeserializer Deserializer { get; }

        public FluentReader(IFluentDeserializer deserializer!!)
        {
            Deserializer = deserializer;
        }

        public FluentReader Read(string path)
        {
            
        }
    }
}
