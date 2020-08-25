using System;
using System.Collections.Generic;
using System.Text;

namespace FluentTranslate.Common.Domain
{
    public class FtlResource : IFtlElement
    {
        public IList<IFtlEntry> Entries { get; set; }
    }
}
