namespace FluentTranslate.Infrastructure
{
    public class FluentEngineContext
	{
		public Dictionary<string, object> Variables { get; init; } = new();

		public object this[string name]
		{
			get { return Variables[name]; }
			set { Variables[name] = value; }
		}

		public FluentEngineContext? Parent { get; set; }
	}
}