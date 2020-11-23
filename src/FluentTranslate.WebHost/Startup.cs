using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.StaticFiles;
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
			var options = section.Get<FluentTranslateOptions>();

			var sourceFilesPath = options.SourceFilesPath;
			var generatedFilesPath = options.GeneratedFilesPath;
			var staticFilesPath = options.StaticFilesPath;

			if (string.IsNullOrWhiteSpace(sourceFilesPath))
				sourceFilesPath = Path.Combine("translations", "source");
			if (string.IsNullOrWhiteSpace(generatedFilesPath))
				generatedFilesPath = Path.Combine("translations", "generated");
			if (string.IsNullOrWhiteSpace(staticFilesPath))
				staticFilesPath = Path.Combine("translations", "static");

			if (!Path.IsPathRooted(sourceFilesPath))
				sourceFilesPath = Path.Combine(HostEnvironment.ContentRootPath, sourceFilesPath);
			if (!Path.IsPathRooted(generatedFilesPath))
				generatedFilesPath = Path.Combine(HostEnvironment.ContentRootPath, generatedFilesPath);
			if (!Path.IsPathRooted(staticFilesPath))
				staticFilesPath = Path.Combine(HostEnvironment.ContentRootPath, staticFilesPath);

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
					opt.RequestPath ??= "/translations";
				});
			
			services.AddHostedService<FluentTranslateFileGenerator>();
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
			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = new CompositeFileProvider(
					new PhysicalFileProvider(opt.GeneratedFilesPath),
					new PhysicalFileProvider(opt.StaticFilesPath)),
				RequestPath = opt.RequestPath,
				ContentTypeProvider = contentTypeProvider,
			});
		}
	}
}
