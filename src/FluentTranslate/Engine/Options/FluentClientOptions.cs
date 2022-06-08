using System;
using System.Collections.Generic;
using System.Text;

namespace FluentTranslate.Engine.Options
{
    public class FluentClientOptions
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<FluentProfileOptions> Profiles { get; }
        public List<FluentSourceOptions> Sources { get; }

        public FluentClientOptions()
        {
            Profiles = new List<FluentProfileOptions>();
            Sources = new List<FluentSourceOptions>();
        }
    }
}
