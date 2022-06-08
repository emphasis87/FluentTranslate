namespace FluentTranslate.Engine.Options
{
    public class FluentProfileOptions
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long[] ParentIds { get; set; }
        public string Parents { get; set; }
    }
}
