using System;
using System.Collections;
using System.IO;
using System.Linq;
using Antlr4.Runtime;
using FluentAssertions;
using FluentTranslate.Domain;
using FluentTranslate.Parser;
using FluentTranslate.Services;
using FluentTranslate.Tests.Support;
using NUnit.Framework;

namespace FluentTranslate.Tests.Parser
{
    [Parallelizable(ParallelScope.All)]
    public class FluentParserVisitorTests
    {
        private static FluentDocument Act(string content)
        {
            var stream = new AntlrInputStream(new StringReader(content));
            var lexer = new FluentDebugLexer(stream);
            var parser = new FluentParser(new CommonTokenStream(lexer));

            // Parse the result using deserialization visitor
            var visitor = new FluentParserVisitor();
            var document = visitor.Visit(parser.document()) as FluentDocument;

            var mode = 0;
            foreach (var modeName in lexer.ModeNames)
            {
                TestContext.WriteLine($"{mode++} {modeName}");
            }

            TestContext.WriteLine();

            return document;
        }

        private static void ShouldBeEqual(FluentDocument actual, FluentDocument expected)
        {
            // Verify result is expected
            FluentEqualityComparer.Default.Equals(actual, expected).Should().BeTrue();

            var serialized = FluentConverter.Serialize(actual);
            var deserialized = Act(serialized);

            // Verify that serialization and deserialization does not change the result
            FluentEqualityComparer.Default.Equals(deserialized, expected).Should().BeTrue();
        }

        [Test]
        public void Can_parse_basic_message()
        {
            var actual = Act(Resources.Hello);

            var expected = new FluentDocument
            {
                new FluentMessage("hello")
                {
                    new FluentText("Hello, world!")
                }
            };

            ShouldBeEqual(actual, expected);
        }

        [Test]
        public void Can_parse_attributes()
        {
            var actual = Act(Resources.Attributes);

            var expected = new FluentDocument
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

            ShouldBeEqual(actual, expected);
        }

        [Test]
        public void Can_parse_comments()
        {
            var actual = Act(Resources.Comments);

            var expected = new FluentDocument
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

            ShouldBeEqual(actual, expected);
        }

        [Test]
        public void Can_parse_function_calls()
        {
            var actual = Act(Resources.Functions);

            var expected = new FluentDocument
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
                    new FluentPlaceable(
                        new FluentFunctionCall("NUMBER")
                        {
                            new FluentCallArgument(new FluentVariableReference("unreadEmails"))
                        }),
                    new FluentText(" unread emails.")
                },
                new FluentMessage("last-notice")
                {
                    new FluentText("Last checked: "),
                    new FluentPlaceable(
                        new FluentFunctionCall("DATETIME")
                        {
                            new FluentCallArgument(new FluentVariableReference("lastChecked")),
                            new FluentCallArgument("day", new FluentStringLiteral("numeric")),
                            new FluentCallArgument("month", new FluentStringLiteral("long")),
                        }),
                    new FluentText(".")
                },
            };

            ShouldBeEqual(actual, expected);
        }

        [Test]
        public void Can_parse_function_DATETIME()
        {
            var actual = Act(Resources.FunctionsDatetime);

            var expected = new FluentDocument
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

            ShouldBeEqual(actual, expected);
        }

        [Test]
        public void Can_parse_function_NUMBER()
        {
            var actual = Act(Resources.FunctionsNumber);

            var expected = new FluentDocument
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

            ShouldBeEqual(actual, expected);
        }

        [Test]
        public void Can_parse_multiline_text()
        {
            var actual = Act(Resources.MultilineText);

            var expected = new FluentDocument
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

            ShouldBeEqual(actual, expected);
        }

        [Test]
        public void Can_parse_placeables()
        {
            var actual = Act(Resources.Placeables);

            var expected = new FluentDocument
            {
                new FluentMessage("remove-bookmark", comment:
                    "$title (String) - The title of the bookmark to remove.")
                {
                    new FluentText("Are you sure you want to remove "),
                    new FluentPlaceable(new FluentVariableReference("title")),
                    new FluentText("?")
                },
            };

            ShouldBeEqual(actual, expected);
        }

        [Test]
        public void PlaceablesInner()
        {
            var actual = Act(Resources.PlaceablesInner);

            var expected = new FluentDocument
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

            ShouldBeEqual(actual, expected);
        }

        [Test]
        public void PlaceablesInterpolation()
        {
            var actual = Act(Resources.PlaceablesInterpolation);

            var expected = new FluentDocument
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

            ShouldBeEqual(actual, expected);
        }

        [Test]
        public void PlaceablesSpecialCharacters()
        {
            var resource = Act(Resources.PlaceablesSpecialCharacters);

            var expected = new FluentDocument
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
            var actual = Act(Resources.QuotedText);

            var expected = new FluentDocument
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

            ShouldBeEqual(actual, expected);
        }

        [Test]
        public void QuotedTextEscape()
        {
            var actual = Act(Resources.QuotedTextEscape);

            var expected = new FluentDocument
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

            ShouldBeEqual(actual, expected);
        }

        [Test]
        public void QuotedTextLeadingBracket()
        {
            var actual = Act(Resources.QuotedTextLeadingBracket);

            var expected = new FluentDocument
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

            ShouldBeEqual(actual, expected);
        }

        [Test]
        public void QuotedTextLeadingDot()
        {
            var actual = Act(Resources.QuotedTextLeadingDot);

            var expected = new FluentDocument
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

            ShouldBeEqual(actual, expected);
        }

        [Test]
        public void QuotedTextUnicodeDash()
        {
            var actual = Act(Resources.QuotedTextUnicodeDash);

            var expected = new FluentDocument
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

            ShouldBeEqual(actual, expected);
        }

        [Test]
        public void QuotedTextUnicodeEscape()
        {
            var actual = Act(Resources.QuotedTextUnicodeEscape);

            var expected = new FluentDocument
            {
                new FluentMessage("privacy-label")
                {
                    new FluentText("Privacy"),
                    new FluentPlaceable(new FluentStringLiteral("\\u00A0")),
                    new FluentText("Policy"),
                },
            };

            ShouldBeEqual(actual, expected);
        }

        [Test]
        public void Selectors()
        {
            var actual = Act(Resources.Selectors);

            var expected = new FluentDocument
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

            ShouldBeEqual(actual, expected);
        }

        [Test]
        public void SelectorsNumber()
        {
            var actual = Act(Resources.SelectorsNumber);

            var expected = new FluentDocument
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

            ShouldBeEqual(actual, expected);
        }

        [Test]
        public void SelectorsOrdinal()
        {
            var actual = Act(Resources.SelectorsOrdinal);

            var expected = new FluentDocument
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

            ShouldBeEqual(actual, expected);
        }

        [Test]
        public void TermsAttributes()
        {
            var actual = Act(Resources.TermsAttributes);

            var expected = new FluentDocument
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

            ShouldBeEqual(actual, expected);
        }

        [Test]
        public void TermsParameterized()
        {
            var actual = Act(Resources.TermsParameterized);

            var expected = new FluentDocument
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

            ShouldBeEqual(actual, expected);
        }

        [Test]
        public void TermsVariants()
        {
            var actual = Act(Resources.TermsVariants);

            var expected = new FluentDocument
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

            ShouldBeEqual(actual, expected);
        }

        [Test]
        public void Variables()
        {
            var actual = Act(Resources.Variables);

            var expected = new FluentDocument
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

            ShouldBeEqual(actual, expected);
        }

        [Test]
        public void VariablesExplicitFormatting()
        {
            var actual = Act(Resources.VariablesExplicitFormatting);

            var expected = new FluentDocument
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

            ShouldBeEqual(actual, expected);
        }

        [Test]
        public void VariablesImplicitFormatting()
        {
            var actual = Act(Resources.VariablesImplicitFormatting);

            var expected = new FluentDocument
            {
                new FluentMessage("time-elapsed", comment:
                    "$duration (Number) - The duration in seconds.")
                {
                    new FluentText("Time elapsed: "),
                    new FluentPlaceable(new FluentVariableReference("duration")),
                    new FluentText("s."),
                },
            };

            ShouldBeEqual(actual, expected);
        }
    }
}
