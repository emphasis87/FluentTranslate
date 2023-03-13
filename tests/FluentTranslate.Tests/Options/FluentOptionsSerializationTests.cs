namespace FluentTranslate.Tests.Options
{
    public class FluentOptionsSerializationTests
    {
        [Test]
        public void Can_deserialize_FluentProfileOptions()
        {
            var source = """
                { "name": "desktop", "parents": [ "common" ] }
                """;

            var serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            var context = new FluentClientOptionsJsonContext(serializerOptions);

            var fluentOptions = JsonSerializer.Deserialize(source, context.FluentProfileOptions);

            fluentOptions.Name.Should().Be("desktop");
            fluentOptions.Parents.Should().Equal("common");
        }

        [Test]
        public void Can_deserialize_FluentSourceOptions()
        {
            var source = """
                { "provider": "remoteApi", "path": "localhost:5000", "user": "guest", "password": "123" }
                """;

            var serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            var context = new FluentClientOptionsJsonContext(serializerOptions);

            var fluentOptions = JsonSerializer.Deserialize(source, context.FluentSourceOptions);

            fluentOptions.Provider.Should().Be("remoteApi");
            fluentOptions.Path.Should().Be("localhost:5000");
        }

        [Test]
        public void Can_deserialize_FluentClientOptions()
        {
            var source = """
                {
                    "name": "my-client",
                    "profiles": 
                    [
                        { "name": "common" },
                        { "name": "desktop", "parents": [ "common" ] }
                    ],
                    "sources": 
                    [
                        { "provider": "embeddedResource" },
                        { "provider": "file" },
                        { "provider": "remoteApi", "path": "localhost:5000", "user": "guest" }
                    ]
                }
                """;

            var serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            var context = new FluentClientOptionsJsonContext(serializerOptions);

            var fluentOptions = JsonSerializer.Deserialize(source, context.FluentClientOptions);

            fluentOptions.Name.Should().Be("my-client");
            fluentOptions.Profiles[0].Name.Should().Be("common");
            fluentOptions.Profiles[1].Name.Should().Be("desktop");
            fluentOptions.Profiles[1].Parents.Should().Equal("common");
            fluentOptions.Sources[0].Provider.Should().Be("embeddedResource");
            fluentOptions.Sources[1].Provider.Should().Be("file");
            fluentOptions.Sources[2].Provider.Should().Be("remoteApi");
            fluentOptions.Sources[2].Path.Should().Be("localhost:5000");
            fluentOptions.Sources[2].AdditionalProperties["user"].GetString().Should().Be("guest");
        }
    }
}
