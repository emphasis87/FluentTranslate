using Antlr4.Runtime;
using FluentAssertions;
using FluentTranslate.Common.Domain;
using NUnit.Framework;
using System;
using System.Collections;
using System.IO;
using FluentTranslate.Common;

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

			return FluentDeserializer.Deserialize(resource, lexer);
		}

		private static void Verify(FluentResource resource, FluentResource expected)
		{
			StructuralComparisons.StructuralEqualityComparer.Equals(resource, expected).Should().BeTrue();

			VerifySerialization(resource, expected);
		}

		private static void VerifySerialization(FluentResource resource, FluentResource expected)
		{
			IFluentSerializer fluentSerializer = new FluentSerializer();
			var serialized = fluentSerializer.Serialize(resource, null);
			var deserialized = Act(serialized);

			StructuralComparisons.StructuralEqualityComparer.Equals(deserialized, expected).Should().BeTrue();
		}

		[Test]
		public void Hello()
		{
			var resource = Act(Resources.Hello);

			var expected = new FluentResource
			{
				new FluentMessage("hello")
				{
					new FluentText("Hello, world!")
				}
			};

			Verify(resource, expected);
		}

		[Test]
		public void Attributes()
		{
			var resource = Act(Resources.Attributes);

			var expected = new FluentResource
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
			};

			Verify(resource, expected);
		}

		[Test]
		public void Comments()
		{
			var resource = Act(Resources.Comments);

			var expected = new FluentResource
			{
				new FluentComment
				{
					Level = 1,
					Value = "This Source Code Form is subject to the terms of the Mozilla Public"
						+ "\r\nLicense, v. 2.0. If a copy of the MPL was not distributed with this"
						+ "\r\nfile, You can obtain one at http://mozilla.org/MPL/2.0/."
				},
				new FluentComment
				{
					Level = 3,
					Value = "Localization for Server-side strings of Firefox Screenshots"
				},
				new FluentComment
				{
					Level = 2,
					Value = "Global phrases shared across pages"
				},
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
				new FluentComment
				{
					Level = 2,
					Value = "Creating page"
				},
				new FluentMessage("creating-page-title", comment:
					"Note: { $title } is a placeholder for the title of the web page"
					+ "\r\ncaptured in the screenshot. The default, for pages without titles, is"
					+ "\r\ncreating-page-title-default.")
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
			};

			Verify(resource, expected);
		}

		[Test]
		public void Functions()
		{
			var resource = Act(Resources.Functions);

			var expected = new FluentResource
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
			};

			Verify(resource, expected);
		}

		[Test]
		public void FunctionsDatetime()
		{
			var resource = Act(Resources.FunctionsDatetime);

			var expected = new FluentResource
			{
				new FluentMessage("today-is")
				{
					new FluentText("Today is "),
					new FluentPlaceable
					{
						Content = new FluentFunctionCall("DATETIME")
						{
							new FluentCallArgument(new FluentVariableReference("date")),
							new FluentCallArgument("month", new FluentStringLiteral("long")),
							new FluentCallArgument("year", new FluentStringLiteral("numeric")),
							new FluentCallArgument("day", new FluentStringLiteral("numeric")),
						}
					},
				},
			};

			Verify(resource, expected);
		}

		[Test]
		public void FunctionsNumber()
		{
			var resource = Act(Resources.FunctionsNumber);

			var expected = new FluentResource
			{
				new FluentMessage("dpi-ratio")
				{
					new FluentText("Your DPI ratio is "),
					new FluentPlaceable
					{
						Content = new FluentFunctionCall("NUMBER")
						{
							new FluentCallArgument(new FluentVariableReference("ratio")),
							new FluentCallArgument("minimumFractionDigits", new FluentNumberLiteral("2")),
						}
					},
				},
			};

			Verify(resource, expected);
		}

		[Test]
		public void MultilineText()
		{
			var resource = Act(Resources.MultilineText);

			var expected = new FluentResource
			{
				new FluentMessage("multi")
				{
					new FluentText(
						"Text can also span multiple lines as long as"
						+"\r\neach new line is indented by at least one space."
						+"\r\nBecause all lines in this message are indented"
						+"\r\nby the same amount, all indentation will be"
						+"\r\nremoved from the final value.")
				},
				new FluentMessage("indents")
				{
					new FluentText(
						"Indentation common to all indented lines is removed"
						+"\r\nfrom the final text value."
						+"\r\n  This line has 2 spaces in front of it.")
				},
				new FluentMessage("leading-spaces")
				{
					new FluentText("This message's value starts with the word \"This\".")
				},
				new FluentMessage("leading-lines")
				{
					new FluentText(
						"This message's value starts with the word \"This\"."
						+"\r\nThe blank lines under the identifier are ignored.")
				},
				new FluentMessage("blank-lines")
				{
					new FluentText(
						"The blank line above this line is ignored."
						+"\r\nThis is a second line of the value."
						+"\r\n"
						+"\r\nThe blank line above this line is preserved.")
				},
			};

			Verify(resource, expected);
		}

		[Test]
		public void Placeables()
		{
			var resource = Act(Resources.Placeables);

			var expected = new FluentResource
			{
				new FluentMessage("remove-bookmark", comment:
					"$title (String) - The title of the bookmark to remove.")
				{
					new FluentText("Are you sure you want to remove "),
					new FluentPlaceable(new FluentVariableReference("title")),
					new FluentText("?")
				},
			};

			StructuralComparisons.StructuralEqualityComparer.Equals(resource, expected).Should().BeTrue();
		}

		[Test]
		public void PlaceablesInner()
		{
			var resource = Act(Resources.PlaceablesInner);

			var expected = new FluentResource
			{
				new FluentMessage("remove-bookmark", comment:
					"$title (String) - The title of the bookmark to remove.")
				{
					new FluentText("Are you sure you want to remove "),
					new FluentPlaceable(new FluentVariableReference("title")),
					new FluentText(" "),
					new FluentPlaceable(new FluentStringLiteral("this bookmark")),
					new FluentText("?"),
				},
			};

			Verify(resource, expected);
		}

		[Test]
		public void PlaceablesInterpolation()
		{
			var resource = Act(Resources.PlaceablesInterpolation);

			var expected = new FluentResource
			{
				new FluentTerm("brand-name")
				{
					new FluentText("Firefox")
				},
				new FluentMessage("installing")
				{
					new FluentText("Installing "),
					new FluentPlaceable(new FluentTermReference("brand-name")),
					new FluentText("."),
				},
				new FluentMessage("menu-save")
				{
					new FluentText("Save")
				},
				new FluentMessage("help-menu-save")
				{
					new FluentText("Click "),
					new FluentPlaceable(new FluentMessageReference("menu-save")),
					new FluentText(" to save the file."),
				},
			};

			Verify(resource, expected);
		}

		[Test]
		public void PlaceablesSpecialCharacters()
		{
			var resource = Act(Resources.PlaceablesSpecialCharacters);

			var expected = new FluentResource
			{
				new FluentMessage("opening-brace")
				{
					new FluentText("This message features an opening curly brace: "),
					new FluentPlaceable(new FluentStringLiteral("{")),
					new FluentText("."),
				},
				new FluentMessage("closing-brace")
				{
					new FluentText("This message features a closing curly brace: "),
					new FluentPlaceable(new FluentStringLiteral("}")),
					new FluentText("."),
				},
			};

			StructuralComparisons.StructuralEqualityComparer.Equals(resource, expected).Should().BeTrue();
		}

		[Test]
		public void QuotedText()
		{
			var resource = Act(Resources.QuotedText);

			var expected = new FluentResource
			{
				new FluentMessage("blank-is-removed")
				{
					new FluentText("This message starts with no blanks."),
				},
				new FluentMessage("blank-is-preserved")
				{
					new FluentPlaceable(new FluentStringLiteral("    ")),
					new FluentText("This message starts with 4 spaces."),
				},
			};

			Verify(resource, expected);
		}

		[Test]
		public void QuotedTextEscape()
		{
			var resource = Act(Resources.QuotedTextEscape);

			var expected = new FluentResource
			{
				new FluentMessage("literal-quote1", comment:
					"This is OK, but cryptic and hard to read and edit.")
				{
					new FluentText("Text in "),
					new FluentPlaceable(new FluentStringLiteral("\\\"")),
					new FluentText("double quotes"),
					new FluentPlaceable(new FluentStringLiteral("\\\"")),
					new FluentText("."),
				},
				new FluentMessage("literal-quote2", comment:
					"This is preferred. Just use the actual double quote character.")
				{
					new FluentText("Text in \"double quotes\"."),
				},
			};

			StructuralComparisons.StructuralEqualityComparer.Equals(resource, expected).Should().BeTrue();
		}

		[Test]
		public void QuotedTextLeadingBracket()
		{
			var resource = Act(Resources.QuotedTextLeadingBracket);

			var expected = new FluentResource
			{
				new FluentMessage("leading-bracket")
				{
					new FluentText(
						"This message has an opening square bracket"
						+"\r\nat the beginning of the third line:"
						+"\r\n"),
					new FluentPlaceable(new FluentStringLiteral("[")),
					new FluentText("."),
				},
			};

			Verify(resource, expected);
		}

		[Test]
		public void QuotedTextLeadingDot()
		{
			var resource = Act(Resources.QuotedTextLeadingDot);

			var expected = new FluentResource
			{
				new FluentMessage("attribute-how-to")
				{
					new FluentText(
						"To add an attribute to this messages, write"
						+"\r\n"),
					new FluentPlaceable(new FluentStringLiteral(".attr = Value")),
					new FluentText(" on a new line."),
					new FluentAttribute("attr")
					{
						new FluentText("An actual attribute (not part of the text value above)")
					}
				},
			};

			Verify(resource, expected);
		}

		[Test]
		public void QuotedTextUnicodeDash()
		{
			var resource = Act(Resources.QuotedTextUnicodeDash);

			var expected = new FluentResource
			{
				new FluentMessage("which-dash1", comment:
					"The dash character is an EM DASH but depending on the font face,"
					+"\r\nit might look like an EN DASH.")
				{
					new FluentText("It's a dash—or is it?"),
				},
				new FluentMessage("which-dash2", comment:
					"Using a Unicode escape sequence makes the intent clear.")
				{
					new FluentText("It's a dash"),
					new FluentPlaceable(new FluentStringLiteral("\\u2014")),
					new FluentText("or is it?"),
				},
			};

			Verify(resource, expected);
		}

		[Test]
		public void QuotedTextUnicodeEscape()
		{
			var resource = Act(Resources.QuotedTextUnicodeEscape);

			var expected = new FluentResource
			{
				new FluentMessage("privacy-label")
				{
					new FluentText("Privacy"),
					new FluentPlaceable(new FluentStringLiteral("\\u00A0")),
					new FluentText("Policy"),
				},
			};

			Verify(resource, expected);
		}

		[Test]
		public void Selectors()
		{
			var resource = Act(Resources.Selectors);

			var expected = new FluentResource
			{
				new FluentMessage("emails")
				{
					new FluentPlaceable(
						new FluentSelection(
							new FluentVariableReference("unreadEmails"))
						{
							new FluentVariant(new FluentIdentifier("one"))
							{
								new FluentText("You have one unread email.")
							},
							new FluentVariant(new FluentIdentifier("other"), isDefault: true)
							{
								new FluentText("You have "),
								new FluentPlaceable(new FluentVariableReference("unreadEmails")),
								new FluentText(" unread emails."),
							}
						}),
				},
			};

			Verify(resource, expected);
		}

		[Test]
		public void SelectorsNumber()
		{
			var resource = Act(Resources.SelectorsNumber);

			var expected = new FluentResource
			{
				new FluentMessage("your-score")
				{
					new FluentPlaceable(
						new FluentSelection(
							new FluentFunctionCall("NUMBER")
							{
								new FluentCallArgument(new FluentVariableReference("score")),
								new FluentCallArgument("minimumFractionDigits", new FluentNumberLiteral("1"))
							})
						{
							new FluentVariant(new FluentNumberLiteral("0.0"))
							{
								new FluentText("You scored zero points. What happened?")
							},
							new FluentVariant(new FluentIdentifier("other"), isDefault: true)
							{
								new FluentText("You scored "),
								new FluentPlaceable(
									new FluentFunctionCall("NUMBER")
									{
										new FluentCallArgument(new FluentVariableReference("score")),
										new FluentCallArgument("minimumFractionDigits", new FluentNumberLiteral("1"))
									}),
								new FluentText(" points."),
							}
						}),
				},
			};

			Verify(resource, expected);
		}

		[Test]
		public void SelectorsOrdinal()
		{
			var resource = Act(Resources.SelectorsOrdinal);

			var expected = new FluentResource
			{
				new FluentMessage("your-rank")
				{
					new FluentPlaceable(
						new FluentSelection(
							new FluentFunctionCall("NUMBER")
							{
								new FluentCallArgument(new FluentVariableReference("pos")),
								new FluentCallArgument("type", new FluentStringLiteral("ordinal"))
							})
						{
							new FluentVariant(new FluentNumberLiteral("1"))
							{
								new FluentText("You finished first!")
							},
							new FluentVariant(new FluentIdentifier("one"))
							{
								new FluentText("You finished "),
								new FluentPlaceable(new FluentVariableReference("pos")),
								new FluentText("st"),
							},
							new FluentVariant(new FluentIdentifier("two"))
							{
								new FluentText("You finished "),
								new FluentPlaceable(new FluentVariableReference("pos")),
								new FluentText("nd"),
							},
							new FluentVariant(new FluentIdentifier("few"))
							{
								new FluentText("You finished "),
								new FluentPlaceable(new FluentVariableReference("pos")),
								new FluentText("rd"),
							},
							new FluentVariant(new FluentIdentifier("other"), isDefault: true)
							{
								new FluentText("You finished "),
								new FluentPlaceable(new FluentVariableReference("pos")),
								new FluentText("th"),
							},
						}),
				},
			};

			Verify(resource, expected);
		}

		[Test]
		public void TermsAttributes()
		{
			var resource = Act(Resources.TermsAttributes);

			var expected = new FluentResource
			{
				new FluentTerm("brand-name")
				{
					new FluentText("Aurora"),
					new FluentAttribute("gender")
					{
						new FluentText("feminine")
					}
				},
				new FluentMessage("update-successful")
				{
					new FluentPlaceable(
						new FluentSelection(new FluentTermReference("brand-name", "gender"))
						{
							new FluentVariant(new FluentIdentifier("masculine"))
							{
								new FluentPlaceable(new FluentTermReference("brand-name")),
								new FluentText(" został zaktualizowany."),
							},
							new FluentVariant(new FluentIdentifier("feminine"))
							{
								new FluentPlaceable(new FluentTermReference("brand-name")),
								new FluentText(" została zaktualizowana."),
							},
							new FluentVariant(new FluentIdentifier("other"), isDefault: true)
							{
								new FluentText("Program "),
								new FluentPlaceable(new FluentTermReference("brand-name")),
								new FluentText(" został zaktualizowany."),
							},
						})
				}
			};

			Verify(resource, expected);
		}

		[Test]
		public void TermsParameterized()
		{
			var resource = Act(Resources.TermsParameterized);

			var expected = new FluentResource
			{
				new FluentTerm("https", comment:
					"A contrived example to demonstrate how variables"
					+ "\r\ncan be passed to terms.")
				{
					new FluentText("https://"),
					new FluentPlaceable(new FluentVariableReference("host")),
				},
				new FluentMessage("visit")
				{
					new FluentText("Visit "),
					new FluentPlaceable(
						new FluentTermReference("https")
						{
							new FluentCallArgument("host", new FluentStringLiteral("example.com"))
						}),
					new FluentText(" for more information."),
				}
			};

			StructuralComparisons.StructuralEqualityComparer.Equals(resource, expected).Should().BeTrue();
		}

		[Test]
		public void TermsVariants()
		{
			var resource = Act(Resources.TermsVariants);

			var expected = new FluentResource
			{
				new FluentTerm("brand-name")
				{
					new FluentPlaceable(
						new FluentSelection(
							new FluentVariableReference("case"))
						{
							new FluentVariant(new FluentIdentifier("nominative"), isDefault:true)
							{
								new FluentText("Firefox")
							},
							new FluentVariant(new FluentIdentifier("locative"))
							{
								new FluentText("Firefoxa")
							}
						}),
				},
				new FluentMessage("about", comment:
					"\"About Firefox.\"")
				{
					new FluentText("Informacje o "),
					new FluentPlaceable(
						new FluentTermReference("brand-name")
						{
							new FluentCallArgument("case", new FluentStringLiteral("locative"))
						}),
					new FluentText("."),
				}
			};

			Verify(resource, expected);
		}

		[Test]
		public void Variables()
		{
			var resource = Act(Resources.Variables);

			var expected = new FluentResource
			{
				new FluentMessage("welcome")
				{
					new FluentText("Welcome, "),
					new FluentPlaceable(new FluentVariableReference("user")),
					new FluentText("!"),
				},
				new FluentMessage("unread-emails")
				{
					new FluentPlaceable(new FluentVariableReference("user")),
					new FluentText(" has "),
					new FluentPlaceable(new FluentVariableReference("email-count")),
					new FluentText(" unread emails."),
				}
			};

			StructuralComparisons.StructuralEqualityComparer.Equals(resource, expected).Should().BeTrue();
		}

		[Test]
		public void VariablesExplicitFormatting()
		{
			var resource = Act(Resources.VariablesExplicitFormatting);

			var expected = new FluentResource
			{
				new FluentMessage("time-elapsed", comment:
					"$duration (Number) - The duration in seconds.")
				{
					new FluentText("Time elapsed: "),
					new FluentPlaceable(
						new FluentFunctionCall("NUMBER")
						{
							new FluentCallArgument(new FluentVariableReference("duration")),
							new FluentCallArgument("maximumFractionDigits", new FluentNumberLiteral("0"))
						}),
					new FluentText("s."),
				},
			};

			Verify(resource, expected);
		}

		[Test]
		public void VariablesImplicitFormatting()
		{
			var resource = Act(Resources.VariablesImplicitFormatting);

			var expected = new FluentResource
			{
				new FluentMessage("time-elapsed", comment:
					"$duration (Number) - The duration in seconds.")
				{
					new FluentText("Time elapsed: "),
					new FluentPlaceable(new FluentVariableReference("duration")),
					new FluentText("s."),
				},
			};

			Verify(resource, expected);
		}
	}
}
