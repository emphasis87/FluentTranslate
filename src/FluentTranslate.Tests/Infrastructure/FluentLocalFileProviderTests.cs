using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using FluentTranslate.Domain;
using FluentTranslate.Infrastructure;
using NUnit.Framework;

namespace FluentTranslate.Tests.Infrastructure
{
	[TestFixture]
	[Parallelizable(ParallelScope.Self)]
	public class FluentLocalFileProviderTests
	{
		private string WorkingDirectory { get; set; }

		[SetUp]
		public void Setup()
		{
			WorkingDirectory = Path.Combine(
				TestContext.CurrentContext.WorkDirectory,
				TestContext.CurrentContext.Random.GetString(10));
		}

		[TearDown]
		public void TearDown()
		{
			if (Directory.Exists(WorkingDirectory))
				Directory.Delete(WorkingDirectory, true);
		}

		[Test]
		public async Task Should_handle_empty_file_and_file_change()
		{
			var configuration = FluentConfiguration.Default;
			var options = configuration.Options.Get<FluentProviderOptions>();
			options.FilePollingInterval = TimeSpan.FromSeconds(1);

			var fn = Path.Combine(WorkingDirectory, "translations.ftl");

			var provider = new TestFluentLocalFileProvider(fn, configuration);

			// When file is missing should return an empty resource
			var r0 = await provider.GetResourceAsync();
			r0.Should().Equal(new FluentResource());
			provider.CheckLastModifiedCount.Should().Be(1);
			provider.FindFileAsyncCount.Should().Be(1);

			// Consecutive calls should be cached based on polling interval
			var r1 = await provider.GetResourceAsync();
			r0.Should().BeSameAs(r1);
			provider.CheckLastModifiedCount.Should().Be(1);
			provider.FindFileAsyncCount.Should().Be(1);

			await Task.Delay(2000);

			// Should return the same object if no changes detected
			var r2 = await provider.GetResourceAsync();
			r0.Should().BeSameAs(r2);
			provider.CheckLastModifiedCount.Should().Be(2);
			provider.FindFileAsyncCount.Should().Be(1);

			Directory.CreateDirectory(WorkingDirectory);
			await File.WriteAllTextAsync(fn, Resources.Hello);
			await Task.Delay(2000);

			// Should return a new resource
			var r3 = await provider.GetResourceAsync();
			r3.Should().Equal(
				new FluentResource
				{
					new FluentMessage("hello")
					{
						new FluentText("Hello, world!")
					}
				}
			);
			provider.CheckLastModifiedCount.Should().Be(3);
			provider.FindFileAsyncCount.Should().Be(2);
		}
	}

	internal class TestFluentLocalFileProvider : FluentLocalFileProvider
	{
		public TestFluentLocalFileProvider(string path, IFluentConfiguration configuration) : base(path, configuration)
		{
		}

		
		public int CheckLastModifiedCount { get; set; }
		protected override bool CheckLastModified(Context context, CultureInfo culture = null)
		{
			CheckLastModifiedCount++;
			return base.CheckLastModified(context, culture);
		}

		public int FindFileAsyncCount { get; set; }
		protected override Task<string> FindFileAsync(Context context, CultureInfo culture = null)
		{
			FindFileAsyncCount++;
			return base.FindFileAsync(context, culture);
		}
	}
}
