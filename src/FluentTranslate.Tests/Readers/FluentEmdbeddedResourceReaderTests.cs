using FluentAssertions;
using FluentTranslate.Domain;
using FluentTranslate.Providers.EmbeddedResource;
using FluentTranslate.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FluentTranslate.Tests.Readers
{
    public class FluentEmdbeddedResourceReaderTests
    {
        [Test]
        public void Can_read_assembly()
        {
            var reader = new FluentEmdbeddedResourceReader();
            var results = reader.Read(Assembly.GetExecutingAssembly()).ToArray();

            results.Should().HaveCount(1);

            var (resource, path) = results[0];

            var expected = new FluentResource
            {
                new FluentMessage("hello")
                {
                    new FluentText("Hello, everyone!")
                }
            };

            FluentEqualityComparer.Default.Equals(resource, expected).Should().BeTrue();
        }
    }
}
