using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.StaticFiles.Infrastructure;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;

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
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<FluentTranslateOptions> options)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			var contentTypeProvider = new FileExtensionContentTypeProvider();
			contentTypeProvider.Mappings.Clear();
			contentTypeProvider.Mappings.Add(".ftl", "text/plain");

			var opt = options.Value;

			var compositeProvider = new CompositeFileProvider(
				new PhysicalFileProvider(opt.GeneratedFilesPath) {UseActivePolling = true},
				new PhysicalFileProvider(opt.StaticFilesPath));
			var sharedOptions = new SharedOptions()
			{
				FileProvider = compositeProvider,
				RequestPath = opt.RequestPath,
			};
			app.UseStaticFiles(new StaticFileOptions(sharedOptions)
			{
				ContentTypeProvider = contentTypeProvider,
			});
			app.UseDirectoryBrowser(new DirectoryBrowserOptions(sharedOptions));
		}
	}

	internal static class PathHelper
	{
		public static string TrimOrNull(this string path) => string.IsNullOrWhiteSpace(path) ? null : path.Trim();
		public static string Rooted(this string path, string root) => !Path.IsPathRooted(path) ? Path.Combine(root, path) : path;
		public static string WithExtension(this string path, string extension) => !Path.HasExtension(path) ? Path.ChangeExtension(path, extension) : path;
	}
}
