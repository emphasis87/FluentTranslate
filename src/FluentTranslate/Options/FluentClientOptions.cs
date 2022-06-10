using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace FluentTranslate.Options
{
    public class FluentClientOptions
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public List<FluentProfileOptions> Profiles { get; set; } = new();
        public List<FluentSourceOptions> Sources { get; set; } = new();

        public FluentClientOptions()
        {
            
        }
    }
}
