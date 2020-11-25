using System;
using System.IO;
using System.Linq;
using Antlr4.Runtime;
using FluentTranslate.Parser;

namespace FluentTranslate.Tests.Support
{
	public class FluentDebugLexer : FluentLexer
	{
		public FluentDebugLexer(ICharStream input) : base(input)
		{
		}

		public FluentDebugLexer(ICharStream input, TextWriter output, TextWriter errorOutput) : base(input, output, errorOutput)
		{
		}

		public override IToken NextToken()
		{
			var modeStack0 = string.Join(",", new[] {CurrentMode}.Concat(ModeStack).Select(x => $"{x}"));

			var token = base.NextToken();

			var modeStack1 = string.Join(",", new[] {CurrentMode}.Concat(ModeStack).Select(x => $"{x}"));
			
			var displayName = DefaultVocabulary.GetDisplayName(token.Type);
			var symbolicName = DefaultVocabulary.GetSymbolicName(token.Type);

			Console.WriteLine($"{symbolicName,20} {modeStack0,12} ->{modeStack1,12} {token}");
			
			return token;
		}
	}
}
