using FluentTranslate.Infrastructure;

namespace FluentTranslate.Tests.Infrastructure
{
	[TestFixture]
	[Parallelizable(ParallelScope.All)]
	public class FluentEmbeddedResourceProviderTests
	{
		[Test]
		[SetUICulture("en-US")]
		public async Task Should_load_embedded_resource()
		{
			var assembly = GetType().Assembly;
			var provider = new FluentEmbeddedResourceProvider(assembly, "FluentTranslate.Tests.EmbeddedResources.Hello.iv.ftl");

			var r0 = await provider.GetResourceAsync();
            
			Assert.AreEqual(r0,
				new FluentResource()
				{
					new FluentMessage("hello")
					{
						new FluentText("Hello, everyone!")
					}
				});
			
			var r1 = await provider.GetResourceAsync(CultureInfo.InvariantCulture);

            Assert.AreEqual(r1, r0);
		}
	}
}
