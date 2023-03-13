using System;
using System.Collections.Generic;
using System.Text;

namespace FluentTranslate.Services
{
    public interface IEngineFactory
    {
        IEngine Create(string profile = "default");
    }
}
