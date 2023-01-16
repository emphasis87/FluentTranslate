namespace FluentTranslate.Options
{
    public class FluentProfileOptions
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public List<string> Parents { get; set; } = new();
    }
}
