using FluentTranslate.Infrastructure;

namespace FluentTranslate.Tests.Infrastructure
{
	public class FluentEngineTests
	{
		[Test]
		public void Hello()
		{
			var resource = FluentConverter.Deserialize(Resources.Hello);
			var engine = new FluentEngine(resource);

			engine.Evaluate("hello").Should().Be("hello");
			engine.Evaluate("{hello}").Should().Be("Hello, world!");
			engine.Evaluate("{test}").Should().Be("{test}");
		}
	}
}
