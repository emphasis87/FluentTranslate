namespace FluentTranslate.Services
{
    public interface IEngine
    {
        string Profile { get; }
        string Language { get; set; }

        string GetValue(string key);
    }
}
