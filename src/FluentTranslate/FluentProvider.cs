using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using FluentTranslate.Domain;

namespace FluentTranslate
{
	public interface IFluentProvider
	{
		(DateTime LastModified, FluentResource Resource) GetResource(CultureInfo culture = null);
	}

	public class PhysicalFileProviderOptions
	{
		public TimeSpan PollingInterval { get; set; }

		public PhysicalFileProviderOptions()
		{
			PollingInterval = TimeSpan.FromSeconds(4);
		}
	}

	public abstract class FluentPhysicalFileProvider : IFluentProvider
	{
		private readonly string _path;
		private readonly IFluentConfiguration _configuration;
		private readonly IFluentCombinator _combinator;

		private readonly Dictionary<string, FileInfoResource> _resourceByPath =
			new Dictionary<string, FileInfoResource>();
		private readonly Dictionary<CultureInfo, CultureInfoResource> _resourceByCulture =
			new Dictionary<CultureInfo, CultureInfoResource>();

		protected FluentPhysicalFileProvider(string path, IFluentConfiguration configuration = null)
		{
			_path = path;
			_configuration = configuration;
			_combinator = configuration?.Combinator ?? FluentCombinator.Default;
		}

		public abstract FluentResource ParseFile(FileInfo file);

		public virtual IEnumerable<string> GetPaths(CultureInfo culture)
		{
			var extension = Path.GetExtension(_path);
			yield return Path.ChangeExtension(_path, $"{culture.Name}{extension}");
			yield return Path.ChangeExtension(_path, $"{culture.TwoLetterISOLanguageName}{extension}");
			yield return _path;
		}

		public virtual (DateTime LastModified, FluentResource Resource) GetResource(CultureInfo culture = null)
		{
			culture ??= CultureInfo.InvariantCulture;
			
			var options = _configuration?.GetOptions<PhysicalFileProviderOptions>();
			var pollingInterval = options?.PollingInterval ?? TimeSpan.FromSeconds(4);

			lock (_resourceByCulture)
			{
				if (!_resourceByCulture.TryGetValue(culture, out var info))
				{
					info = new CultureInfoResource {Culture = culture};
					_resourceByCulture[culture] = info;
				}

				var now = DateTime.Now;
				if (info.LastPolled + pollingInterval >= now) 
					return (info.LastModified, info.Resource);

				info.LastPolled = now;
				if (info.Files is null)
				{
					var paths = GetPaths(culture).ToArray();
					var files = new List<FileInfoResource>();
					foreach (var path in paths)
					{
						if (!_resourceByPath.TryGetValue(path, out var file))
						{
							file = new FileInfoResource() {Path = path};
							_resourceByPath[path] = file;
						}

						files.Add(file);
					}

					info.Files = files.ToArray();
				}

				var lastModified = DateTime.MinValue;
				var resources = new List<FluentResource>();
				foreach (var file in info.Files)
				{
					if (file.LastPolled + pollingInterval < now)
						PollFile(file, now);

					if (file.LastModified > lastModified)
						lastModified = file.LastModified;

					if (file.Resource != null)
						resources.Add(file.Resource);
				}

				var resource = _combinator.Combine(resources);
				info.LastModified = lastModified;
				info.Resource = resource;

				return (info.LastModified, info.Resource);
			}
		}

		private void PollFile(FileInfoResource info, DateTime now)
		{
			info.LastPolled = now;
			info.FileInfo ??= new FileInfo(info.Path);

			var file = info.FileInfo;
			if (file.Exists)
			{
				var lastModified = file.LastWriteTime;
				if (lastModified > info.LastModified)
				{
					info.LastModified = lastModified;
					info.Resource = ParseFile(file);
				}
			}
			else if (!(info.Resource is null))
			{
				info.LastModified = now;
				info.Resource = null;
			}
		}

		private class CultureInfoResource
		{
			public CultureInfo Culture { get; set; }
			public DateTime LastPolled { get; set; }
			public DateTime LastModified { get; set; }
			public FileInfoResource[] Files { get; set; }
			public FluentResource Resource { get; set; }
		}

		private class FileInfoResource
		{
			public string Path { get; set; }
			public DateTime LastPolled { get; set; }
			public FileInfo FileInfo { get; set; }
			public DateTime LastModified { get; set; }
			public FluentResource Resource { get; set; }
		}
	}
}
