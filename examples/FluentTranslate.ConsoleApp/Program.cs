using FluentAssertions;
using FluentTranslate;
using FluentTranslate.Services;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection()
    .AddFluentTranslate();

var sp = services.BuildServiceProvider();

var engineFactory = sp.GetRequiredService<IEngineFactory>();
var engine = engineFactory.Create();

engine.Language = "en";
var m1 = engine.GetValue("hello");
m1.Should().Be("Hello!", "language is set to 'cs'");
Console.WriteLine(m1);

engine.Language = "cs";
var m2 = engine.GetValue("hello");
m2.Should().Be("Ahoj!", "language is set to 'cs'");
Console.WriteLine(m2);