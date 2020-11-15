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

namespace FluentTranslate.WebHost
{
	public class Startup
	{
		public Startup(IConfiguration configuration, IWebHostEnvironment hostEnvironment)
		{
			Configuration = configuration;
			HostEnvironment = hostEnvironment;
		}

		public IConfiguration Configuration { get; }
		public IWebHostEnvironment HostEnvironment { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddOptions<FluentTranslateOptions>()
				.Bind(Configuration.GetSection(FluentTranslateOptions.Section))
				.PostConfigure(options =>
				{
					options.DefaultLanguage ??= "en";
				});

			services.AddHostedService<FluentTranslateFileGenerator>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			var contentTypeProvider = new FileExtensionContentTypeProvider();
			contentTypeProvider.Mappings.Clear();
			contentTypeProvider.Mappings.Add(".ftl", "text/plain");

			var generatedFilesPath = Path.Combine(env.ContentRootPath, "translations", "generated");
			var staticFilesPath = Path.Combine(env.ContentRootPath, "translations", "static");
			var sourceFilesPath = Path.Combine(env.ContentRootPath, "translations", "source");

			Directory.CreateDirectory(generatedFilesPath);
			Directory.CreateDirectory(staticFilesPath);
			Directory.CreateDirectory(sourceFilesPath);

			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = new CompositeFileProvider(
					new PhysicalFileProvider(generatedFilesPath),
					new PhysicalFileProvider(staticFilesPath)),
				RequestPath = "/files",
				ContentTypeProvider = contentTypeProvider,
			});
		}
	}
}
