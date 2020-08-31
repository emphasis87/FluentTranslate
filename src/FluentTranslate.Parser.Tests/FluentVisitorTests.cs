using System;
using System.IO;
using System.Linq;
using Antlr4.Runtime;
using FluentAssertions;
using FluentTranslate.Common.Domain;
using NUnit.Framework;

namespace FluentTranslate.Parser.Tests
{
	public class FluentVisitorTests
	{
		private FluentResource Act(string resource)
		{
			var inputStream = new AntlrInputStream(new StringReader(resource));
			var lexer = new FluentDebugLexer(inputStream);

			var mode = 0;
			foreach (var modeName in lexer.ModeNames)
			{
				Console.WriteLine($"{mode++} {modeName}");
			}
			Console.WriteLine();

			var tokenStream = new CommonTokenStream(lexer);
			var parser = new FluentParser(tokenStream);
			
			var visitor = new FluentDeserializationVisitor();
			return (FluentResource)visitor.Visit(parser.resource()).FirstOrDefault();
		}

		[Test]
		public void Hello()
		{
			var resource = Act(Resources.Hello);
			var entry = (FluentMessage)resource.Entries.Single();
			var text = (FluentText)entry.Content.Single();
			text.Value.Should().Be(Resources.Hello[8..]);
		}

		[Test]
		public void Attributes()
		{
			var resource = Act(Resources.Attributes);
			var entry = (FluentMessage) resource.Entries.Single();
			entry.Id.Should().Be("login-input");
			entry.Content.Cast<FluentText>().Single().Value.Should().Be("Predefined value");
			entry.Attributes.Should().HaveCount(3);
			var attribute0 = entry.Attributes[0];
			attribute0.Id.Should().Be("placeholder");
			attribute0.Content.Cast<FluentText>().Single().Value.Should().Be("email@example.com");
			var attribute1 = entry.Attributes[1];
			attribute1.Id.Should().Be("aria-label");
			attribute1.Content.Cast<FluentText>().Single().Value.Should().Be("Login input value");
			var attribute2 = entry.Attributes[2];
			attribute2.Id.Should().Be("title");
			attribute2.Content.Cast<FluentText>().Single().Value.Should().Be("Type your login email");
		}

		[Test]
		public void Comments()
		{
			var resource = Act(Resources.Comments);
			resource.Entries.Should().HaveCount(16);
			var comment0 = (FluentComment) resource.Entries[0];
			comment0.Level.Should().Be(1);
			comment0.Value.Should()
				.Be(@"This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
file, You can obtain one at http://mozilla.org/MPL/2.0/.");
		}

		[Test]
		public void Functions()
		{
			Act(Resources.Functions);
		}

		[Test]
		public void FunctionsDatetime()
		{
			Act(Resources.FunctionsDatetime);
		}

		[Test]
		public void FunctionsNumber()
		{
			Act(Resources.FunctionsNumber);
		}

		[Test]
		public void MultilineText()
		{
			var resource = Act(Resources.MultilineText);
			resource.Entries.Should().HaveCount(8);
		}

		[Test]
		public void Placeables()
		{
			Act(Resources.Placeables);
		}

		[Test]
		public void PlaceablesInner()
		{
			Act(Resources.PlaceablesInner);
		}

		[Test]
		public void PlaceablesInterpolation()
		{
			Act(Resources.PlaceablesInterpolation);
		}

		[Test]
		public void PlaceablesSpecialCharacters()
		{
			Act(Resources.PlaceablesSpecialCharacters);
		}

		[Test]
		public void QuotedText()
		{
			Act(Resources.QuotedText);
		}

		[Test]
		public void QuotedTextEscape()
		{
			Act(Resources.QuotedTextEscape);
		}

		[Test]
		public void QuotedTextLeadingBracket()
		{
			Act(Resources.QuotedTextLeadingBracket);
		}

		[Test]
		public void QuotedTextLeadingDot()
		{
			Act(Resources.QuotedTextLeadingDot);
		}

		[Test]
		public void QuotedTextUnicodeDash()
		{
			Act(Resources.QuotedTextUnicodeDash);
		}

		[Test]
		public void QuotedTextUnicodeEscape()
		{
			Act(Resources.QuotedTextUnicodeEscape);
		}

		[Test]
		public void Selectors()
		{
			Act(Resources.Selectors);
		}

		[Test]
		public void SelectorsNumber()
		{
			Act(Resources.SelectorsNumber);
		}

		[Test]
		public void SelectorsOrdinal()
		{
			Act(Resources.SelectorsOrdinal);
		}

		[Test]
		public void TermsAttributes()
		{
			Act(Resources.TermsAttributes);
		}

		[Test]
		public void TermsParameterized()
		{
			Act(Resources.TermsParameterized);
		}

		[Test]
		public void TermsVariants()
		{
			Act(Resources.TermsVariants);
		}

		[Test]
		public void Variables()
		{
			Act(Resources.Variables);
		}

		[Test]
		public void VariablesExplicitFormatting()
		{
			Act(Resources.VariablesExplicitFormatting);
		}

		[Test]
		public void VariablesImplicitFormatting()
		{
			Act(Resources.VariablesImplicitFormatting);
		}
	}
}
