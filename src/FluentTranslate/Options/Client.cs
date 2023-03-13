namespace FluentTranslate.Options
{
    public class Client
    {
        public IList<Source> Sources { get; init; } = new List<Source>();
        public IList<Profile> Profiles { get; init; } = new List<Profile>();
    }

    public class Profile
    {
        public string? Id { get; init; }
        public IList<string> Sources { get; init; } = new List<string>();
    }

    public class Source
    {
        public string? Path { get; init; }
        public string? Provider { get; init; }
        public IList<string> Arguments { get; init; } = new List<string>();
    }
}
