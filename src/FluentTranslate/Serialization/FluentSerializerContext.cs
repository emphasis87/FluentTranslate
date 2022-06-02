using System;
using System.Text;

namespace FluentTranslate.Serialization
{
    public class FluentSerializerContext
	{
        protected StringBuilder Writer { get; } = new();

		public bool IsTextContinuation { get; set; }
		public string Indent { get; private set; } = "";

        public void AddIndent() => Indent = new string(' ', Indent.Length + 4);
        public void RemoveIndent() => Indent = new string(' ', Math.Max(0, Indent.Length - 4));
    }
}