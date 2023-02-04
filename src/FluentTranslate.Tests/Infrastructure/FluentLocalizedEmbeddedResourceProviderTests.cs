using FluentTranslate.Infrastructure;

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

            Assert.AreEqual(r0, new FluentDocument());
			
			var r1 = await provider.GetResourceAsync(CultureInfo.InvariantCulture);

            Assert.AreEqual(r1,
				new FluentDocument()
				{
					new FluentMessage("hello")
					{
						new FluentText("Hello, everyone!")
					}
				});

			var r2 = await provider.GetResourceAsync();

            Assert.AreEqual(r2, new FluentDocument());
		}
	}
}
