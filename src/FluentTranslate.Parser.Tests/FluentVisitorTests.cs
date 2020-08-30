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
		private IFluentElement Act(string resource)
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
			return visitor.Visit(parser.resource()).FirstOrDefault();
		}

		[Test]
		public void Hello()
		{
			var result = Act(Resources.Hello);
			//var entry = (FluentMessage)result.Entries.Single();
			//var text = (FluentText)entry.Content.Single();
			//text.Value.Should().Be(Resources.Hello[9..]);
		}

		[Test]
		public void Attributes()
		{
			Act(Resources.Attributes);
		}

		[Test]
		public void Comments()
		{
			Act(Resources.Comments);
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
			Act(Resources.MultilineText);
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
