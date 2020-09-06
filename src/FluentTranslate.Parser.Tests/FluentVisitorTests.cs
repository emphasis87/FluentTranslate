using System;
using System.Collections;
using System.Collections.Generic;
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
		private static FluentResource Act(string resource)
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
			return (FluentResource) visitor.Visit(parser.resource()).FirstOrDefault();
		}

		[Test]
		public void Hello()
		{
			var resource = Act(Resources.Hello);

			StructuralComparisons.StructuralEqualityComparer.Equals(resource,
				new FluentResource
				{
					new FluentMessage("hello")
					{
						new FluentText("Hello, world!")
					}
				}).Should().BeTrue();
		}

		[Test]
		public void Attributes()
		{
			var resource = Act(Resources.Attributes);

			StructuralComparisons.StructuralEqualityComparer.Equals(resource, new FluentResource
			{
				new FluentMessage("login-input")
				{
					new FluentText("Predefined value"),
					new FluentAttribute("placeholder")
					{
						new FluentText("email@example.com")
					},
					new FluentAttribute("aria-label")
					{
						new FluentText("Login input value")
					},
					new FluentAttribute("title")
					{
						new FluentText("Type your login email")
					}
				}
			}).Should().BeTrue();
		}

		[Test]
		public void Comments()
		{
			var resource = Act(Resources.Comments);

			StructuralComparisons.StructuralEqualityComparer.Equals(resource, new FluentResource
			{
				new FluentComment
				{
					Level = 1,
					Value = "This Source Code Form is subject to the terms of the Mozilla Public"
						+ "\r\nLicense, v. 2.0. If a copy of the MPL was not distributed with this"
						+ "\r\nfile, You can obtain one at http://mozilla.org/MPL/2.0/."
				},
				new FluentEmptyLines(),
				new FluentComment
				{
					Level = 3,
					Value = "Localization for Server-side strings of Firefox Screenshots"
				},
				new FluentEmptyLines(),
				new FluentComment
				{
					Level = 2,
					Value = "Global phrases shared across pages"
				},
				new FluentEmptyLines(),
				new FluentMessage("my-shots")
				{
					new FluentText("My Shots")
				},
				new FluentMessage("home-link")
				{
					new FluentText("Home")
				},
				new FluentMessage("screenshots-description")
				{
					new FluentText
					{
						Value = "Screenshots made simple. Take, save, and"
							+ "\r\nshare screenshots without leaving Firefox."
					}
				},
				new FluentEmptyLines(),
				new FluentComment
				{
					Level = 2,
					Value = "Creating page"
				},
				new FluentEmptyLines(),
				new FluentComment
				{
					Level = 1,
					Value = "Note: { $title } is a placeholder for the title of the web page"
						+ "\r\ncaptured in the screenshot. The default, for pages without titles, is"
						+ "\r\ncreating-page-title-default."
				},
				new FluentMessage("creating-page-title")
				{
					new FluentText("Creating "),
					new FluentPlaceable {Content = new FluentVariableReference("title")}
				},
				new FluentMessage("creating-page-title-default")
				{
					new FluentText("page")
				},
				new FluentMessage("creating-page-wait-message")
				{
					new FluentText("Saving your shot…")
				}
			}).Should().BeTrue();
		}

		[Test]
		public void Functions()
		{
			var resource = Act(Resources.Functions);

			StructuralComparisons.StructuralEqualityComparer.Equals(resource, new FluentResource
			{
				new FluentMessage("emails")
				{
					new FluentText("You have "),
					new FluentPlaceable
					{
						Content = new FluentVariableReference("unreadEmails")
					},
					new FluentText(" unread emails.")
				},
				new FluentMessage("emails2")
				{
					new FluentText("You have "),
					new FluentPlaceable
					{
						Content = new FluentFunctionCall("NUMBER")
						{
							new FluentCallArgument(new FluentVariableReference("unreadEmails"))
						}
					},
					new FluentText(" unread emails.")
				},
				new FluentEmptyLines(),
				new FluentMessage("last-notice")
				{
					new FluentText("Last checked: "),
					new FluentPlaceable
					{
						Content = new FluentFunctionCall("DATETIME")
						{
							new FluentCallArgument(new FluentVariableReference("lastChecked")),
							new FluentCallArgument("day", new FluentStringLiteral("numeric")),
							new FluentCallArgument("month", new FluentStringLiteral("long")),
						}
					},
					new FluentText(".")
				},
			}).Should().BeTrue();
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
