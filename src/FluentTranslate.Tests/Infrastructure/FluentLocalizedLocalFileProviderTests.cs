using System;
using System.Collections.Generic;
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
	public class FluentLocalizedLocalFileProviderTests
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
		[SetUICulture("iv")]
		public async Task Should_combine_files_based_on_culture_suffix()
		{
			var configuration = FluentConfiguration.Default;
			var options = configuration.Options.Get<FluentResourceProviderOptions>();
			options.FilePollingInterval = TimeSpan.FromSeconds(1);

			var fn = Path.Combine(WorkingDirectory, "translations.ftl");

			var provider = new TestFluentLocalizedLocalFileResourceProvider(fn, configuration);

			// When files are missing should return an empty resource
			var r0 = await provider.GetResourceAsync();
			r0.Should().Equal(new FluentResource());
			provider.FindResourceAsyncCount.Should().Be(1);
			provider.Providers.Should().BeEquivalentTo(fn, fn.Replace("ftl", "iv.ftl"));

			// Consecutive calls should be cached based on polling interval
			var r1 = await provider.GetResourceAsync();
			r0.Should().BeSameAs(r1);
			provider.FindResourceAsyncCount.Should().Be(1);

			await Task.Delay(2000);

			// Should return the same object if no changes detected
			var r2 = await provider.GetResourceAsync();
			r0.Should().BeSameAs(r2);
			provider.FindResourceAsyncCount.Should().Be(2);

			Directory.CreateDirectory(WorkingDirectory);
			await File.WriteAllTextAsync(fn, $"{Resources.Hello}\r\n{Resources.Hello.Replace("hello", "greeting")}");
			await Task.Delay(2000);

			// Should return a new resource
			var r3 = await provider.GetResourceAsync();
			r3.Should().Equal(
				new FluentResource
				{
					new FluentMessage("hello") {new FluentText("Hello, world!")},
					new FluentMessage("greeting") {new FluentText("Hello, world!")}
				});

			await File.WriteAllTextAsync(fn.Replace("ftl", "iv.ftl"), Resources.Hello.Replace("world", "everyone"));
			await Task.Delay(2000);

			// Should combine the resources with specific culture having priority
			var r4 = await provider.GetResourceAsync();
			r4.Should().Equal(
				new FluentResource
				{
					new FluentMessage("hello") {new FluentText("Hello, everyone!")},
					new FluentMessage("greeting") {new FluentText("Hello, world!")}
				});

			// Should ignore files for different cultures
			var en = CultureInfo.GetCultureInfo("en-US");
			var r5 = await provider.GetResourceAsync(en);
			r5.Should().Equal(r3);

			// Should create a single provider for each path
			provider.Providers.Should().BeEquivalentTo(
				fn, fn.Replace("ftl", "iv.ftl"), 
				fn.Replace("ftl", "en.ftl"), fn.Replace("ftl", "en-US.ftl"));
		}

		internal class TestFluentLocalizedLocalFileResourceProvider : FluentLocalizedLocalFileResourceProvider
		{
			public TestFluentLocalizedLocalFileResourceProvider(string path, IFluentConfiguration configuration) : base(path, configuration)
			{
			}

			public List<string> Providers { get; } = new List<string>();

			protected override IFluentResourceProvider CreateProvider(string path)
			{
				Providers.Add(path);
				return new TestFluentLocalFileResourceProvider(path, Configuration);
			}

			public int FindResourceAsyncCount { get; set; }
			protected override Task<FluentResource> FindResourceAsync(Context context, CultureInfo culture)
			{
				FindResourceAsyncCount++;
				return base.FindResourceAsync(context, culture);
			}
		}
	}
}