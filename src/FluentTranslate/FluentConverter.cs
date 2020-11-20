using FluentTranslate.Domain;
using FluentTranslate.Serialization;

namespace FluentTranslate
{
	public class FluentConverter
	{
		public static FluentResource Deserialize(string content)
		{
			return FluentFormatDeserializer.Default.Deserialize(content);
		}

		public static string Serialize(FluentResource resource)
		{
			return FluentFormatSerializer.Default.Serialize(resource);
		}
	}
}
