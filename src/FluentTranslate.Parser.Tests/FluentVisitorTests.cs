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
			return (FluentResource)visitor.Visit(parser.resource()).FirstOrDefault();
		}

		[Test]
		public void Hello()
		{
			var resource = Act(Resources.Hello);

			StructuralComparisons.StructuralEqualityComparer.Equals(resource,
				new FluentResource
				{
					Entries = new List<IFluentEntry>()
					{
						new FluentMessage
						{
							Id = "hello",
							Content = new List<IFluentContent>
							{
								new FluentText
								{
									Value = "Hello, world!"
								}
							}
						}
					}
				}).Should().BeTrue();
		}

		[Test]
		public void Attributes()
		{
			var resource = Act(Resources.Attributes);

			StructuralComparisons.StructuralEqualityComparer.Equals(resource,
				new FluentResource
				{
					Entries = new List<IFluentEntry>()
					{
						new FluentMessage
						{
							Id = "login-input",
							Content = new List<IFluentContent>
							{
								new FluentText {Value = "Predefined value"}
							},
							Attributes = new List<FluentAttribute>
							{
								new FluentAttribute
								{
									Id = "placeholder",
									Content = new List<IFluentContent>
									{
										new FluentText {Value = "email@example.com"}
									},
								},
								new FluentAttribute
								{
									Id = "aria-label",
									Content = new List<IFluentContent>
									{
										new FluentText {Value = "Login input value"}
									},
								},
								new FluentAttribute
								{
									Id = "title",
									Content = new List<IFluentContent>
									{
										new FluentText {Value = "Type your login email"}
									},
								}
							}
						}
					}
				}).Should().BeTrue();
		}

		[Test]
		public void Comments()
		{
			var resource = Act(Resources.Comments);

			StructuralComparisons.StructuralEqualityComparer.Equals(resource,
				new FluentResource
				{
					Entries = new List<IFluentEntry>()
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
						new FluentMessage
						{
							Id = "my-shots",
							Content = new List<IFluentContent>
							{
								new FluentText {Value = "My Shots"}
							}
						},
						new FluentMessage
						{
							Id = "home-link",
							Content = new List<IFluentContent>
							{
								new FluentText {Value = "Home"}
							}
						},
						new FluentMessage
						{
							Id = "screenshots-description",
							Content = new List<IFluentContent>
							{
								new FluentText
								{
									Value = "Screenshots made simple. Take, save, and"
										+ "\r\nshare screenshots without leaving Firefox."
								}
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
						new FluentMessage
						{
							Id = "creating-page-title",
							Content = new List<IFluentContent>
							{
								new FluentText {Value = "Creating "},
								new FluentPlaceable {Content = new FluentVariableReference {Id = "title"}}
							}
						},
						new FluentMessage
						{
							Id = "creating-page-title-default",
							Content = new List<IFluentContent>
							{
								new FluentText {Value = "page"}
							}
						},
						new FluentMessage
						{
							Id = "creating-page-wait-message",
							Content = new List<IFluentContent>
							{
								new FluentText {Value = "Saving your shot…"}
							}
						},
					}
				}).Should().BeTrue();

		}

		[Test]
		public void Functions()
		{
			var resource = Act(Resources.Functions);
			
			resource.Entries.Should().HaveCount(4);
			
			var e0 = (FluentMessage) resource.Entries[0];
			e0.Id.Should().Be("emails");
			((FluentText) e0.Content[0]).Value.Should().Be("You have ");
			var p0 = (FluentPlaceable) e0.Content[1];
			((FluentVariableReference) p0.Content).Id.Should().Be("unreadEmails");
			((FluentText) e0.Content[2]).Value.Should().Be(" unread emails.");
			
			var e1 = (FluentMessage) resource.Entries[1];
			e1.Id.Should().Be("emails2");
			((FluentText) e1.Content[0]).Value.Should().Be("You have ");
			var p1 = (FluentPlaceable) e1.Content[1];
			var f0 = (FluentFunctionCall) p1.Content;
			f0.Id.Should().Be("NUMBER");
			f0.Arguments[0].Id.Should().BeNull();
			((FluentVariableReference) f0.Arguments[0].Value).Id.Should().Be("unreadEmails");
			((FluentText) e0.Content[2]).Value.Should().Be(" unread emails.");

			var e2 = (FluentEmptyLines) resource.Entries[2];

			var e3 = (FluentMessage) resource.Entries[3];
			e3.Id.Should().Be("last-notice");
			((FluentText) e3.Content[0]).Value.Should().Be("Last checked: ");
			var f1 = (FluentFunctionCall) ((FluentPlaceable) e3.Content[1]).Content;
			f1.Id.Should().Be("DATETIME");
			f1.Arguments[0].Id.Should().BeNull();
			((FluentVariableReference) f1.Arguments[0].Value).Id.Should().Be("lastChecked");
			f1.Arguments[1].Id.Should().Be("day");
			((FluentStringLiteral)f1.Arguments[1].Value).Value.Should().Be("numeric");
			f1.Arguments[2].Id.Should().Be("month");
			((FluentStringLiteral)f1.Arguments[2].Value).Value.Should().Be("long");
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
