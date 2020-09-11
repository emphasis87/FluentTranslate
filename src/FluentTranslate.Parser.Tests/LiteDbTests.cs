using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentAssertions;
using FluentTranslate.Service.Domain;
using LiteDB;
using NUnit.Framework;

namespace FluentTranslate.Parser.Tests
{
	public class LiteDbTests
	{
		[Test]
		public void Test()
		{
			var wd = TestContext.CurrentContext.WorkDirectory;
			var file = Path.Combine(wd, "lite.db");
			if (File.Exists(file))
				File.Delete(file);

			using var db = new LiteDatabase(file);

			db.GetCollection<TranslationEntry>()
				.DeleteAll();

			var entry1 = new TranslationEntry()
			{
				Name = "entry"
			};
			entry1.Attributes.Add("language", "cs");
			entry1.UpdateId();

			var entry2 = new TranslationEntry()
			{
				Name = "entry"
			};
			entry2.Attributes.Add("language", "cs");
			entry2.Attributes.Add("app", "web");
			entry2.UpdateId();

			db.GetCollection<TranslationEntry>()
				.Insert(new[] {entry1, entry2});

			var missing = new HashSet<TranslationEntry>(new TranslationEntryEqualityComparer())
			{
				entry1
			};

			db.GetCollection<TranslationEntry>()
				.DeleteMany(x => missing.Contains(x));

			var result1 = db.GetCollection<TranslationEntry>().FindAll();
			result1.Should().HaveCount(1);

			var names = new HashSet<string> {"entry"};
			db.GetCollection<TranslationEntry>()
				.DeleteMany(x => names.Contains(x.Name));

			var result2 = db.GetCollection<TranslationEntry>().FindAll();
			result2.Should().HaveCount(0);
		}
	}
}
