using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.StaticFiles.Infrastructure;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace FluentTranslate.WebHost
{
	public class Startup
	{
		public IConfiguration Configuration { get; }
		public IWebHostEnvironment HostEnvironment { get; }

		public Startup(IConfiguration configuration, IWebHostEnvironment hostEnvironment)
		{
			Configuration = configuration;
			HostEnvironment = hostEnvironment;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			var section = Configuration.GetSection(FluentTranslateOptions.Section);
			var options = section.Get<FluentTranslateOptions>() ?? new FluentTranslateOptions();
			
			var contentRootPath = HostEnvironment.ContentRootPath;

			var sourceFilesPath = (options.SourceFilesPath.TrimOrNull() ?? Path.Combine("translations", "source"))
				.Rooted(contentRootPath).ToLowerInvariant();
			var generatedFilesPath = (options.GeneratedFilesPath.TrimOrNull() ?? Path.Combine("translations", "generated"))
				.Rooted(contentRootPath).ToLowerInvariant();
			var staticFilesPath = (options.StaticFilesPath.TrimOrNull() ?? Path.Combine("translations", "static"))
				.Rooted(contentRootPath).ToLowerInvariant();

			Directory.CreateDirectory(generatedFilesPath);
			Directory.CreateDirectory(staticFilesPath);
			Directory.CreateDirectory(sourceFilesPath);

			services.AddOptions<FluentTranslateOptions>()
				.Bind(section)
				.PostConfigure(opt =>
				{
					opt.SourceFilesPath = sourceFilesPath;
					opt.GeneratedFilesPath = generatedFilesPath;
					opt.StaticFilesPath = staticFilesPath;

					if (!Configuration
						.GetSection(FluentTranslateOptions.Section)
						.GetSection(nameof(FluentTranslateOptions.UseCompression))
						.Exists())
						opt.UseCompression = true;

					opt.CacheControl ??= $"public,max-age={TimeSpan.FromMinutes(10).TotalSeconds}";

					// Add default extension to file names and change to lowercase
					var generateFiles = opt.GenerateFiles?
						.Where(generate => !string.IsNullOrWhiteSpace(generate?.Name))
						.Select(generate =>
						{
							generate.Name = generate.Name
								.Rooted(generatedFilesPath).WithExtension(".ftl").ToLowerInvariant();

							var sources = generate.Sources?
								.Where(source => !string.IsNullOrWhiteSpace(source))
								.Select(source => source.WithExtension(".ftl").ToLowerInvariant())
								.ToArray();
							generate.Sources = sources ?? new string[0];
							return generate;
						})
						.OrderBy(x => x.Name, StringComparer.InvariantCultureIgnoreCase)
						.ToArray();

					opt.GenerateFiles = generateFiles ?? new FluentGenerateFileOptions[0];
				});
			
			services.AddHostedService<FluentFileGeneratorService>();
			services.AddDirectoryBrowser();
			services.AddResponseCompression(opt =>
			{
				opt.Providers.Add<BrotliCompressionProvider>();
				opt.Providers.Add<GzipCompressionProvider>();
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptionsMonitor<FluentTranslateOptions> optionsMonitor)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			//app.UseHttpsRedirection();

			var contentTypeProvider = new FileExtensionContentTypeProvider();
			contentTypeProvider.Mappings.Clear();
			contentTypeProvider.Mappings.Add(".ftl", "text/plain");

			var options = optionsMonitor.CurrentValue;

			if (options.UseCompression)
				app.UseResponseCompression();

			var compositeProvider = new CompositeFileProvider(
				new PhysicalFileProvider(options.GeneratedFilesPath) {UseActivePolling = true},
				new PhysicalFileProvider(options.StaticFilesPath));
			var sharedOptions = new SharedOptions()
			{
				FileProvider = compositeProvider,
				RequestPath = options.RequestPath,
			};
			app.UseStaticFiles(new StaticFileOptions(sharedOptions)
			{
				ContentTypeProvider = contentTypeProvider,
				OnPrepareResponse = (ctx) => OnPrepareResponse(ctx, optionsMonitor)
			});
			app.UseDirectoryBrowser(new DirectoryBrowserOptions(sharedOptions));
		}

		private void OnPrepareResponse(StaticFileResponseContext context, IOptionsMonitor<FluentTranslateOptions> optionsMonitor)
		{
			var options = optionsMonitor.CurrentValue;
			var headers = context.Context.Response.Headers;
			headers[HeaderNames.CacheControl] = options.CacheControl;
		}
	}

	internal static class PathHelper
	{
		public static string TrimOrNull(this string path) => string.IsNullOrWhiteSpace(path) ? null : path.Trim();
		public static string Rooted(this string path, string root) => !Path.IsPathRooted(path) ? Path.Combine(root, path) : path;
		public static string WithExtension(this string path, string extension) => !Path.HasExtension(path) ? Path.ChangeExtension(path, extension) : path;
	}
}
