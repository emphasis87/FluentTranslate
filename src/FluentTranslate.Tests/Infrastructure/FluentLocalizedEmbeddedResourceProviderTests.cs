using System.Globalization;
using System.Threading.Tasks;
using FluentAssertions;
using FluentTranslate.Domain;
using FluentTranslate.Infrastructure;
using NUnit.Framework;

namespace FluentTranslate.Tests.Infrastructure
{
	[TestFixture]
	[Parallelizable(ParallelScope.All)]
	public class FluentLocalizedEmbeddedResourceProviderTests
	{
		[Test]
		[SetUICulture("en-US")]
		public async Task Should_load_embedded_resource()
		{
			var assembly = GetType().Assembly;
			var provider = new FluentLocalizedEmbeddedResourceProvider(assembly, "FluentTranslate.Tests.EmbeddedResources.Hello.ftl");

			var r0 = await provider.GetResourceAsync();
			r0.Should().Equal(new FluentDocument());
			
			var r1 = await provider.GetResourceAsync(CultureInfo.InvariantCulture);
			r1.Should().Equal(
				new FluentDocument()
				{
					new FluentMessage("hello")
					{
						new FluentText("Hello, everyone!")
					}
				});

			var r2 = await provider.GetResourceAsync();
			r2.Should().Equal(new FluentDocument());
		}
	}
}
