using FluentTranslate.Infrastructure;

namespace FluentTranslate.Tests.Infrastructure
{
	[Parallelizable(ParallelScope.All)]
	public class FluentEngineTests
	{
		private readonly FluentEngine _engine = new();

		[Test]
		public void Can_evaluate_message_0()
		{
			var engine = new FluentEngine();
			
			engine.Add(
				new FluentResource()
				{
					new FluentMessage("hello")
					{
						new FluentText("Hello world!")
					}
				});

			var result = engine.GetString("hello");

			result.Should().Be("Hello world!");
		}

        [Test]
        public void Can_evaluate_message_1()
        {
            var engine = new FluentEngine();
            
			engine.Add(
                new FluentResource()
                {
                    new FluentMessage("apples")
                    {
                        new FluentText("There is a "),
                        new FluentPlaceable(
							new FluentStringLiteral("red")),
                        new FluentText(" apple on the table.")
                    }
                });

            var result = engine.GetString("apples");

            result.Should().Be("There is a red apple on the table.");
        }

        [Test]
        public void Can_evaluate_message_2()
        {
            var engine = new FluentEngine();

            engine.Add(
                new FluentResource()
                {
                    new FluentMessage("apples")
                    {
                        new FluentText("There are "),
                        new FluentPlaceable(
                            new FluentNumberLiteral("2")),
                        new FluentText(" apples on the table.")
                    }
                });

            var result = engine.GetString("apples");

            result.Should().Be("There are 2 apples on the table.");
        }

        [Test]
        public void Can_evaluate_message_3()
        {
            var engine = new FluentEngine();

            engine.Add(
                new FluentResource()
                {
                    new FluentMessage("logged-in")
                    {
                        new FluentText("The user "),
                        new FluentPlaceable(
                            new FluentVariableReference("user")),
                        new FluentText(" is logged in!")
                    }
                });

            var result = engine.GetString("logged-in", new() { ["user"] = "xHero" });

            result.Should().Be("The user xHero is logged in!");
        }

        [Test]
        public void Can_evaluate_message_4()
        {
            var engine = new FluentEngine();

            engine.Add(
                new FluentResource()
                {
                    new FluentMessage("pizza")
                    {
                        new FluentText("The is exactly "),
                        new FluentPlaceable(
                            new FluentFunctionCall("FORMAT")
                            {
                                new FluentCallArgument(
                                    new FluentVariableReference("count")),
                                new FluentCallArgument("D2",
                                    new FluentNumberLiteral("2"))
                            }),
                        new FluentText(" slices of pizza left!")
                    }
                });

            var result = engine.GetString("pizza", new() { ["count"] = "1.6" });

            result.Should().Be("The is exactly 1.60 slices of pizza left!");
        }

        [Test]
        public void Can_evaluate_term_1()
        {
            var engine = new FluentEngine();

            engine.Add(
                new FluentResource()
                {
                    new FluentTerm("apples")
                    {
                        new FluentText("There are "),
                        new FluentPlaceable(
                            new FluentNumberLiteral("2")),
                        new FluentText(" apples on the table.")
                    }
                });

            var result = engine.GetString("-apples");

            result.Should().Be("There are 2 apples on the table.");
        }

        [Test]
        public void Can_evaluate_attribute_1()
        {
            var engine = new FluentEngine();

            engine.Add(
                new FluentResource()
                {
                    new FluentMessage("apples")
                    {
                        new FluentAttribute("count")
                        {
                            new FluentText("There are "),
                            new FluentPlaceable(
                                new FluentNumberLiteral("2")),
                            new FluentText(" apples on the table.")
                        }
                    }
                });

            var result = engine.GetString("apples.count");

            result.Should().Be("There are 2 apples on the table.");
        }

        [Test]
        public void Can_evaluate_attribute_2()
        {
            var engine = new FluentEngine();

            engine.Add(
                new FluentResource()
                {
                    new FluentTerm("apples")
                    {
                        new FluentAttribute("count")
                        {
                            new FluentText("There are "),
                            new FluentPlaceable(
                                new FluentNumberLiteral("2")),
                            new FluentText(" apples on the table.")
                        }
                    }
                });

            var result = engine.GetString("-apples.count");

            result.Should().Be("There are 2 apples on the table.");
        }

        [Test]
        public void Can_evaluate_term_reference_1()
        {
            var engine = new FluentEngine();

            engine.Add(
                new FluentResource()
                {
                    new FluentTerm("fruit")
                    {
                        new FluentText("There is a green "),
                        new FluentPlaceable(
                            new FluentVariableReference("fruitType")),
                        new FluentText(" on the table.")
                    }
                });

            var result = engine.GetString("-fruit(fruitType: apple)");

            result.Should().Be("There is a green apple on the table.");
        }

        [Test]
        public void Can_evaluate_term_reference_2()
        {
            var engine = new FluentEngine();

            engine.Add(
                new FluentResource()
                {
                    new FluentMessage("apples")
                    {
                        new FluentPlaceable(
                            new FluentTermReference("fruit")
                            {
                                new FluentCallArgument("fruitType",
                                    new FluentStringLiteral("apple"))
                            })
                    },
                    new FluentTerm("fruit")
                    {
                        new FluentText("There is a green "),
                        new FluentPlaceable(
                            new FluentVariableReference("fruitType")),
                        new FluentText(" on the table.")
                    }
                });

            var result = engine.GetString("apples", new() { ["fruit"] = "broccoli" });
            
            // The fruit argument in the context should be ignored
            result.Should().Be("There is a green apple on the table.");
        }

        [Test]
        [SetCulture("en-US")]
        public void Can_evaluate_NUMBER_function_1()
        {
            var engine = new FluentEngine();

            engine.Add(new FluentFunction_NUMBER());

            engine.Add(
                new FluentResource()
                {
                    new FluentMessage("apples")
                    {
                        new FluentText("There are "),
                        new FluentPlaceable(
                            new FluentFunctionCall("NUMBER")
                            {
                                new FluentCallArgument(
                                    new FluentVariableReference("count"))
                            }),
                        new FluentText(" apples on the table.")
                    },
                });

            var result = engine.GetString("apples", new() { ["count"] = "1.5" });

            result.Should().Be("There are 1.5 apples on the table.");
        }
    }
}
