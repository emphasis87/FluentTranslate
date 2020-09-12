using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentAssertions;
using FluentTranslate.Common.Domain;
using FluentTranslate.Service.Domain;
using LiteDB;
using Newtonsoft.Json;
using NUnit.Framework;
using Utf8Json;
using JsonReader = Newtonsoft.Json.JsonReader;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;
using JsonWriter = Newtonsoft.Json.JsonWriter;

namespace FluentTranslate.Service.Tests
{
	public class TranslationFileMonitorTests
	{
		[Test]
		public void Can_parse_files()
		{
			var wd = TestContext.CurrentContext.WorkDirectory;
			foreach (var file in Directory.GetFiles(wd, "*.ftl"))
				File.Delete(file);

			var dbFile = Path.Combine(wd, "translations.db");
			if (File.Exists(dbFile))
				File.Delete(dbFile);

			File.WriteAllText(Path.Combine(wd, "main.ftl"), Resources.Hello);

			using var db = new LiteDatabase("translations.db");
			var monitor = new TranslationFileMonitor(db);
			
			monitor.Add(wd);

			var entries = db.GetCollection<TranslationEntry>().FindAll();
			entries.Single().Name.Should().Be("hello");
		}

		[Test]
		public void Serialization()
		{
			var message = new FluentMessage("hello")
			{
				new FluentText("Hello, world!"),
				new FluentAttribute("myAttribute")
				{
					new FluentText("attribute-text")
				}
			};

			var record = (FluentRecord) message;
			var result1 = Utf8Json.JsonSerializer.ToJsonString(record);
			var result2 = Newtonsoft.Json.JsonConvert.SerializeObject(message, new FluentElementJsonConverter());
			var serializer = Newtonsoft.Json.JsonSerializer.CreateDefault();
			var sb = new StringBuilder();
			serializer.Serialize(new StringWriter(sb), message);
			var result4 = sb.ToString();
			var result3 = System.Text.Json.JsonSerializer.Serialize(record);
		}
	}

	public class FluentElementJsonConverter : Newtonsoft.Json.JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return true;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			
			serializer.Serialize(writer, value);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}
	}
}
