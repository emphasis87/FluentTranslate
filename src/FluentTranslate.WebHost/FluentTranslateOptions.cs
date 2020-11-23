namespace FluentTranslate.WebHost
{
	public class FluentTranslateOptions
	{
		public const string Section = "FluentTranslate";

		public string StaticFilesPath { get; set; }
		public string SourceFilesPath { get; set; }
		public string GeneratedFilesPath { get; set; }

		public string RequestPath { get; set; }

		public GenerateFileOptions[] GenerateFiles { get; set; }
	}

	public class GenerateFileOptions
	{
		public string Name { get; set; }
		public string[] Sources { get; set; }
	}
}
