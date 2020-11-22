using System;
using System.Collections.Generic;
using System.Text;
using FluentTranslate.Serialization;

namespace FluentTranslate.Infrastructure
{
	public interface IFluentDeserializerContainer
	{
		IFluentDeserializer Get(string extension);
		void Add(string extension, IFluentDeserializer deserializer);
		void Remove(string extension);
	}

	public class FluentDeserializerContainer : IFluentDeserializerContainer
	{
		private readonly Dictionary<string, IFluentDeserializer> _deserializers =
			new Dictionary<string, IFluentDeserializer>();

		public static FluentDeserializerContainer Default
		{
			get
			{
				var container = new FluentDeserializerContainer();
				container.Add("ftl", new FluentFormatDeserializer());
				return container;
			}
		}

		public IFluentDeserializer Get(string extension)
		{
			var ext = extension.TrimStart('.');
			return _deserializers.TryGetValue(ext, out var deserializer) ? deserializer : default;
		}

		public void Add(string extension, IFluentDeserializer deserializer)
		{
			var ext = extension.TrimStart('.');
			_deserializers[ext] = deserializer;
		}

		public void Remove(string extension)
		{
			var ext = extension.TrimStart('.');
			_deserializers.Remove(ext);
		}
	}
}
