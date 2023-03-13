using System;
using System.Collections.Generic;
using System.Text;
using FluentTranslate.Options;

namespace FluentTranslate
{
    public class Configuration
    {
        public Client Options { get; init; } = new();

        public void AddDefaultConfiguration()
        {
            AddDefaultProfile();
            AddEmbeddedResources("./**/*.ftl");
            AddLocalFiles("./**/*.ftl");
        }

        public void AddDefaultProfile()
        {
            var profile = new Profile
            {
                Id = "default",
                Sources = { "*" },
            };

            Options.Profiles.Add(profile);
        }

        public void AddLocalFiles(string pattern)
        {
            var remoteSouceOptions = new Source
            {
                Provider = Providers.File,
                Path = pattern,
            };

            Options.Sources.Add(remoteSouceOptions);
        }

        public void AddEmbeddedResources(string pattern)
        {
            var remoteSouceOptions = new Source
            {
                Provider = Providers.EmbeddedFile,
                Path = pattern,
            };

            Options.Sources.Add(remoteSouceOptions);
        }

        public void AddRemote(string apiPath)
        {
            var remoteSouceOptions = new Source
            {
                Provider = Providers.RemoteApi,
                Path = apiPath,
            };

            Options.Sources.Add(remoteSouceOptions);
        }
    }
}
