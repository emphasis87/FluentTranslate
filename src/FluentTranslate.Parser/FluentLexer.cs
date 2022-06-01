//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.10.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from c:\projects\FluentTranslate\src\FluentTranslate.Parser\grammars\FluentLexer.g4 by ANTLR 4.10.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace FluentTranslate.Parser {
using System;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.10.1")]
[System.CLSCompliant(false)]
public partial class FluentLexer : Lexer {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		NL=1, NL_INDENT=2, INDENT=3, TERM=4, IDENTIFIER=5, IDENTIFIER_REF=6, EQUALS=7, 
		TEXT=8, ATTRIBUTE=9, PLACEABLE_OPEN=10, PLACEABLE_CLOSE=11, STRING_OPEN=12, 
		NUMBER_LITERAL=13, VARIABLE_REF=14, TERM_REF=15, ATTRIBUTE_REF=16, SELECTOR=17, 
		CALL_OPEN=18, CALL_CLOSE=19, CALL_ARG_SEP=20, CALL_ARG_NAME_SEP=21, VARIANT_DEFAULT=22, 
		VARIANT_OPEN=23, VARIANT_CLOSE=24, COMMENT_OPEN=25, COMMENT=26, ESCAPED_CHAR=27, 
		UNICODE_ESCAPE=28, STRING_CLOSE=29, QUOTED_STRING=30, ML_VARIANT_DEFAULT=31, 
		ML_VARIANT_OPEN=32, PL_VARIABLE_REF=33, PL_TERM_REF=34, PL_ATTRIBUTE_REF=35, 
		CL_PLACEABLE_CLOSE=36, CL_CLOSE=37, CL_ARG_SEP=38, CL_ARG_NAME_SEP=39;
	public const int
		COMMENTS=1, RECORDS=2, SINGLELINE=3, MULTILINE=4, PLACEABLES=5, CALLS=6, 
		VARIANTS=7, STRINGS=8;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE", "COMMENTS", "RECORDS", "SINGLELINE", "MULTILINE", "PLACEABLES", 
		"CALLS", "VARIANTS", "STRINGS"
	};

	public static readonly string[] ruleNames = {
		"InlinePrintable", "InlineChar", "IndentedChar", "QuotedChar", "Newline", 
		"Identifier", "CommentPrefix", "CommentChar", "IndentChar", "Indent", 
		"Whitespace", "NumberLiteral", "COMMENT_OPEN", "PLACEABLE_OPEN", "TERM", 
		"ATTRIBUTE", "IDENTIFIER", "NL_INDENT", "NL", "INDENT", "COMMENT", "CM_NL", 
		"RC_EQUALS", "RC_IDENTIFIER", "RC_INDENT", "SL_TEXT", "SL_PLACEABLE_OPEN", 
		"SL_PLACEABLE_CLOSE", "SL_NL_INDENT", "SL_NL", "SL_INDENT", "ML_TEXT", 
		"ML_PLACEABLE_OPEN", "ML_PLACEABLE_CLOSE", "ML_VARIANT_DEFAULT", "ML_VARIANT_OPEN", 
		"ML_ATTRIBUTE", "ML_NL_INDENT", "ML_NL", "PL_PLACEABLE_OPEN", "PL_PLACEABLE_CLOSE", 
		"PL_CALL_OPEN", "PL_STRING_OPEN", "PL_VARIABLE_REF", "PL_NUMBER_LITERAL", 
		"PL_TERM_REF", "PL_ATTRIBUTE_REF", "PL_SELECTOR", "PL_IDENTIFIER", "PL_NL_INDENT", 
		"PL_NL", "PL_INDENT", "CL_PLACEABLE_OPEN", "CL_PLACEABLE_CLOSE", "CL_OPEN", 
		"CL_CLOSE", "CL_ARG_SEP", "CL_ARG_NAME_SEP", "CL_STRING_OPEN", "CL_VARIABLE_REF", 
		"CL_NUMBER_LITERAL", "CL_TERM_REF", "CL_ATTRIBUTE_REF", "CL_SELECTOR", 
		"CL_IDENTIFIER", "CL_NL_INDENT", "CL_NL", "CL_INDENT", "VA_VARIANT_DEFAULT", 
		"VA_VARIANT_OPEN", "VA_VARIANT_CLOSE", "VA_PLACEABLE_OPEN", "VA_PLACEABLE_CLOSE", 
		"VA_IDENTIFIER", "VA_NUMBER_LITERAL", "VA_NL_INDENT", "VA_NL", "VA_INDENT", 
		"ESCAPED_CHAR", "UNICODE_ESCAPE", "STRING_CLOSE", "QUOTED_STRING"
	};


	public FluentLexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public FluentLexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, null, null, null, "'\"'", null, null, null, null, null, null, 
		null, "')'", "','", "':'"
	};
	private static readonly string[] _SymbolicNames = {
		null, "NL", "NL_INDENT", "INDENT", "TERM", "IDENTIFIER", "IDENTIFIER_REF", 
		"EQUALS", "TEXT", "ATTRIBUTE", "PLACEABLE_OPEN", "PLACEABLE_CLOSE", "STRING_OPEN", 
		"NUMBER_LITERAL", "VARIABLE_REF", "TERM_REF", "ATTRIBUTE_REF", "SELECTOR", 
		"CALL_OPEN", "CALL_CLOSE", "CALL_ARG_SEP", "CALL_ARG_NAME_SEP", "VARIANT_DEFAULT", 
		"VARIANT_OPEN", "VARIANT_CLOSE", "COMMENT_OPEN", "COMMENT", "ESCAPED_CHAR", 
		"UNICODE_ESCAPE", "STRING_CLOSE", "QUOTED_STRING", "ML_VARIANT_DEFAULT", 
		"ML_VARIANT_OPEN", "PL_VARIABLE_REF", "PL_TERM_REF", "PL_ATTRIBUTE_REF", 
		"CL_PLACEABLE_CLOSE", "CL_CLOSE", "CL_ARG_SEP", "CL_ARG_NAME_SEP"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "FluentLexer.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ChannelNames { get { return channelNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override int[] SerializedAtn { get { return _serializedATN; } }

	static FluentLexer() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}
	private static int[] _serializedATN = {
		4,0,39,579,6,-1,6,-1,6,-1,6,-1,6,-1,6,-1,6,-1,6,-1,6,-1,2,0,7,0,2,1,7,
		1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,6,2,7,7,7,2,8,7,8,2,9,7,9,2,10,
		7,10,2,11,7,11,2,12,7,12,2,13,7,13,2,14,7,14,2,15,7,15,2,16,7,16,2,17,
		7,17,2,18,7,18,2,19,7,19,2,20,7,20,2,21,7,21,2,22,7,22,2,23,7,23,2,24,
		7,24,2,25,7,25,2,26,7,26,2,27,7,27,2,28,7,28,2,29,7,29,2,30,7,30,2,31,
		7,31,2,32,7,32,2,33,7,33,2,34,7,34,2,35,7,35,2,36,7,36,2,37,7,37,2,38,
		7,38,2,39,7,39,2,40,7,40,2,41,7,41,2,42,7,42,2,43,7,43,2,44,7,44,2,45,
		7,45,2,46,7,46,2,47,7,47,2,48,7,48,2,49,7,49,2,50,7,50,2,51,7,51,2,52,
		7,52,2,53,7,53,2,54,7,54,2,55,7,55,2,56,7,56,2,57,7,57,2,58,7,58,2,59,
		7,59,2,60,7,60,2,61,7,61,2,62,7,62,2,63,7,63,2,64,7,64,2,65,7,65,2,66,
		7,66,2,67,7,67,2,68,7,68,2,69,7,69,2,70,7,70,2,71,7,71,2,72,7,72,2,73,
		7,73,2,74,7,74,2,75,7,75,2,76,7,76,2,77,7,77,2,78,7,78,2,79,7,79,2,80,
		7,80,2,81,7,81,1,0,1,0,1,1,1,1,1,2,1,2,1,3,1,3,1,4,1,4,1,4,3,4,185,8,4,
		1,5,1,5,5,5,189,8,5,10,5,12,5,192,9,5,1,6,1,6,1,6,1,6,1,6,1,6,3,6,200,
		8,6,1,7,1,7,1,8,1,8,1,9,4,9,207,8,9,11,9,12,9,208,1,10,1,10,4,10,213,8,
		10,11,10,12,10,214,1,11,3,11,218,8,11,1,11,4,11,221,8,11,11,11,12,11,222,
		1,11,1,11,4,11,227,8,11,11,11,12,11,228,3,11,231,8,11,1,12,1,12,3,12,235,
		8,12,1,12,1,12,1,13,1,13,1,13,1,13,1,13,1,14,1,14,1,14,1,14,1,14,1,15,
		1,15,1,15,1,15,1,15,1,16,1,16,1,16,1,16,1,17,1,17,1,17,1,17,1,17,1,18,
		1,18,1,19,1,19,1,20,5,20,268,8,20,10,20,12,20,271,9,20,1,21,3,21,274,8,
		21,1,21,1,21,1,21,1,22,1,22,1,22,1,22,1,22,1,23,1,23,1,23,1,23,1,24,1,
		24,1,24,1,24,1,25,1,25,5,25,294,8,25,10,25,12,25,297,9,25,1,25,1,25,1,
		26,1,26,1,26,1,26,1,26,1,27,1,27,1,27,1,27,1,27,1,27,1,27,1,28,1,28,1,
		28,1,28,1,28,1,28,1,29,1,29,1,29,1,29,1,29,1,30,1,30,1,30,1,30,1,31,1,
		31,5,31,330,8,31,10,31,12,31,333,9,31,1,31,1,31,1,32,1,32,1,32,1,32,1,
		32,1,33,1,33,1,33,1,33,1,33,1,33,1,34,1,34,1,34,1,34,1,34,1,35,1,35,1,
		35,1,35,1,35,1,36,1,36,1,36,1,36,1,36,1,37,1,37,1,37,1,37,1,37,1,38,1,
		38,1,38,1,38,1,38,1,39,1,39,1,39,1,39,1,39,1,40,1,40,1,40,1,40,1,40,1,
		41,1,41,1,41,1,41,1,41,1,42,1,42,1,42,1,42,1,42,1,43,1,43,1,43,1,43,1,
		44,1,44,1,44,1,44,1,45,1,45,1,45,1,45,1,46,1,46,1,46,1,46,1,47,1,47,1,
		47,1,47,1,47,1,47,1,48,1,48,1,48,1,48,1,49,1,49,1,49,1,49,1,49,1,49,1,
		50,1,50,1,50,1,50,1,51,1,51,1,51,1,51,1,52,1,52,1,52,1,52,1,52,1,53,1,
		53,1,53,1,53,1,53,1,54,1,54,1,54,1,54,1,54,1,55,1,55,1,55,1,55,1,55,1,
		56,1,56,1,56,1,56,1,57,1,57,1,57,1,57,1,58,1,58,1,58,1,58,1,58,1,59,1,
		59,1,59,1,59,1,60,1,60,1,60,1,60,1,61,1,61,1,61,1,61,1,62,1,62,1,62,1,
		62,1,63,1,63,1,63,1,63,1,63,1,63,1,64,1,64,1,64,1,64,1,65,1,65,1,65,1,
		65,1,65,1,65,1,66,1,66,1,66,1,66,1,67,1,67,1,67,1,67,1,68,1,68,1,68,1,
		68,1,69,1,69,1,69,1,69,1,70,1,70,1,70,1,70,1,70,1,71,1,71,1,71,1,71,1,
		71,1,72,1,72,1,72,1,72,1,72,1,73,1,73,1,73,1,73,1,74,1,74,1,74,1,74,1,
		75,1,75,1,75,1,75,1,75,1,76,1,76,1,76,1,76,1,77,1,77,1,77,1,77,1,78,1,
		78,1,78,1,79,1,79,1,79,1,79,1,79,1,79,1,79,1,79,1,79,1,79,1,79,1,79,1,
		79,1,79,1,79,1,79,3,79,569,8,79,1,80,1,80,1,80,1,80,1,81,4,81,576,8,81,
		11,81,12,81,577,0,0,82,9,0,11,0,13,0,15,0,17,0,19,0,21,0,23,0,25,0,27,
		0,29,0,31,0,33,25,35,10,37,4,39,9,41,5,43,2,45,1,47,3,49,26,51,0,53,0,
		55,0,57,0,59,0,61,0,63,0,65,0,67,0,69,0,71,0,73,0,75,0,77,31,79,32,81,
		0,83,0,85,0,87,0,89,0,91,0,93,0,95,33,97,0,99,34,101,35,103,0,105,0,107,
		0,109,0,111,0,113,0,115,36,117,0,119,37,121,38,123,39,125,0,127,0,129,
		0,131,0,133,0,135,0,137,0,139,0,141,0,143,0,145,0,147,0,149,0,151,0,153,
		0,155,0,157,0,159,0,161,0,163,0,165,27,167,28,169,29,171,30,9,0,1,2,3,
		4,5,6,7,8,10,5,0,10,10,13,13,32,32,123,123,125,125,4,0,10,10,13,13,123,
		123,125,125,7,0,10,10,13,13,42,42,46,46,91,91,123,123,125,125,4,0,10,10,
		13,13,34,34,92,92,2,0,65,90,97,122,5,0,45,45,48,57,65,90,95,95,97,122,
		2,0,10,10,13,13,1,0,48,57,2,0,34,34,92,92,3,0,48,57,65,70,97,102,576,0,
		33,1,0,0,0,0,35,1,0,0,0,0,37,1,0,0,0,0,39,1,0,0,0,0,41,1,0,0,0,0,43,1,
		0,0,0,0,45,1,0,0,0,0,47,1,0,0,0,1,49,1,0,0,0,1,51,1,0,0,0,2,53,1,0,0,0,
		2,55,1,0,0,0,2,57,1,0,0,0,3,59,1,0,0,0,3,61,1,0,0,0,3,63,1,0,0,0,3,65,
		1,0,0,0,3,67,1,0,0,0,3,69,1,0,0,0,4,71,1,0,0,0,4,73,1,0,0,0,4,75,1,0,0,
		0,4,77,1,0,0,0,4,79,1,0,0,0,4,81,1,0,0,0,4,83,1,0,0,0,4,85,1,0,0,0,5,87,
		1,0,0,0,5,89,1,0,0,0,5,91,1,0,0,0,5,93,1,0,0,0,5,95,1,0,0,0,5,97,1,0,0,
		0,5,99,1,0,0,0,5,101,1,0,0,0,5,103,1,0,0,0,5,105,1,0,0,0,5,107,1,0,0,0,
		5,109,1,0,0,0,5,111,1,0,0,0,6,113,1,0,0,0,6,115,1,0,0,0,6,117,1,0,0,0,
		6,119,1,0,0,0,6,121,1,0,0,0,6,123,1,0,0,0,6,125,1,0,0,0,6,127,1,0,0,0,
		6,129,1,0,0,0,6,131,1,0,0,0,6,133,1,0,0,0,6,135,1,0,0,0,6,137,1,0,0,0,
		6,139,1,0,0,0,6,141,1,0,0,0,6,143,1,0,0,0,7,145,1,0,0,0,7,147,1,0,0,0,
		7,149,1,0,0,0,7,151,1,0,0,0,7,153,1,0,0,0,7,155,1,0,0,0,7,157,1,0,0,0,
		7,159,1,0,0,0,7,161,1,0,0,0,7,163,1,0,0,0,8,165,1,0,0,0,8,167,1,0,0,0,
		8,169,1,0,0,0,8,171,1,0,0,0,9,173,1,0,0,0,11,175,1,0,0,0,13,177,1,0,0,
		0,15,179,1,0,0,0,17,184,1,0,0,0,19,186,1,0,0,0,21,199,1,0,0,0,23,201,1,
		0,0,0,25,203,1,0,0,0,27,206,1,0,0,0,29,212,1,0,0,0,31,217,1,0,0,0,33,232,
		1,0,0,0,35,238,1,0,0,0,37,243,1,0,0,0,39,248,1,0,0,0,41,253,1,0,0,0,43,
		257,1,0,0,0,45,262,1,0,0,0,47,264,1,0,0,0,49,269,1,0,0,0,51,273,1,0,0,
		0,53,278,1,0,0,0,55,283,1,0,0,0,57,287,1,0,0,0,59,291,1,0,0,0,61,300,1,
		0,0,0,63,305,1,0,0,0,65,312,1,0,0,0,67,318,1,0,0,0,69,323,1,0,0,0,71,327,
		1,0,0,0,73,336,1,0,0,0,75,341,1,0,0,0,77,347,1,0,0,0,79,352,1,0,0,0,81,
		357,1,0,0,0,83,362,1,0,0,0,85,367,1,0,0,0,87,372,1,0,0,0,89,377,1,0,0,
		0,91,382,1,0,0,0,93,387,1,0,0,0,95,392,1,0,0,0,97,396,1,0,0,0,99,400,1,
		0,0,0,101,404,1,0,0,0,103,408,1,0,0,0,105,414,1,0,0,0,107,418,1,0,0,0,
		109,424,1,0,0,0,111,428,1,0,0,0,113,432,1,0,0,0,115,437,1,0,0,0,117,442,
		1,0,0,0,119,447,1,0,0,0,121,452,1,0,0,0,123,456,1,0,0,0,125,460,1,0,0,
		0,127,465,1,0,0,0,129,469,1,0,0,0,131,473,1,0,0,0,133,477,1,0,0,0,135,
		481,1,0,0,0,137,487,1,0,0,0,139,491,1,0,0,0,141,497,1,0,0,0,143,501,1,
		0,0,0,145,505,1,0,0,0,147,509,1,0,0,0,149,513,1,0,0,0,151,518,1,0,0,0,
		153,523,1,0,0,0,155,528,1,0,0,0,157,532,1,0,0,0,159,536,1,0,0,0,161,541,
		1,0,0,0,163,545,1,0,0,0,165,549,1,0,0,0,167,568,1,0,0,0,169,570,1,0,0,
		0,171,575,1,0,0,0,173,174,8,0,0,0,174,10,1,0,0,0,175,176,8,1,0,0,176,12,
		1,0,0,0,177,178,8,2,0,0,178,14,1,0,0,0,179,180,8,3,0,0,180,16,1,0,0,0,
		181,182,5,13,0,0,182,185,5,10,0,0,183,185,5,10,0,0,184,181,1,0,0,0,184,
		183,1,0,0,0,185,18,1,0,0,0,186,190,7,4,0,0,187,189,7,5,0,0,188,187,1,0,
		0,0,189,192,1,0,0,0,190,188,1,0,0,0,190,191,1,0,0,0,191,20,1,0,0,0,192,
		190,1,0,0,0,193,194,5,35,0,0,194,195,5,35,0,0,195,200,5,35,0,0,196,197,
		5,35,0,0,197,200,5,35,0,0,198,200,5,35,0,0,199,193,1,0,0,0,199,196,1,0,
		0,0,199,198,1,0,0,0,200,22,1,0,0,0,201,202,8,6,0,0,202,24,1,0,0,0,203,
		204,5,32,0,0,204,26,1,0,0,0,205,207,3,25,8,0,206,205,1,0,0,0,207,208,1,
		0,0,0,208,206,1,0,0,0,208,209,1,0,0,0,209,28,1,0,0,0,210,213,3,25,8,0,
		211,213,3,17,4,0,212,210,1,0,0,0,212,211,1,0,0,0,213,214,1,0,0,0,214,212,
		1,0,0,0,214,215,1,0,0,0,215,30,1,0,0,0,216,218,5,45,0,0,217,216,1,0,0,
		0,217,218,1,0,0,0,218,220,1,0,0,0,219,221,7,7,0,0,220,219,1,0,0,0,221,
		222,1,0,0,0,222,220,1,0,0,0,222,223,1,0,0,0,223,230,1,0,0,0,224,226,5,
		46,0,0,225,227,7,7,0,0,226,225,1,0,0,0,227,228,1,0,0,0,228,226,1,0,0,0,
		228,229,1,0,0,0,229,231,1,0,0,0,230,224,1,0,0,0,230,231,1,0,0,0,231,32,
		1,0,0,0,232,234,3,21,6,0,233,235,5,32,0,0,234,233,1,0,0,0,234,235,1,0,
		0,0,235,236,1,0,0,0,236,237,6,12,0,0,237,34,1,0,0,0,238,239,5,123,0,0,
		239,240,1,0,0,0,240,241,6,13,1,0,241,242,6,13,2,0,242,36,1,0,0,0,243,244,
		5,45,0,0,244,245,1,0,0,0,245,246,6,14,3,0,246,247,6,14,4,0,247,38,1,0,
		0,0,248,249,5,46,0,0,249,250,1,0,0,0,250,251,6,15,5,0,251,252,6,15,4,0,
		252,40,1,0,0,0,253,254,3,19,5,0,254,255,1,0,0,0,255,256,6,16,4,0,256,42,
		1,0,0,0,257,258,3,17,4,0,258,259,3,27,9,0,259,260,1,0,0,0,260,261,6,17,
		6,0,261,44,1,0,0,0,262,263,3,17,4,0,263,46,1,0,0,0,264,265,3,27,9,0,265,
		48,1,0,0,0,266,268,3,23,7,0,267,266,1,0,0,0,268,271,1,0,0,0,269,267,1,
		0,0,0,269,270,1,0,0,0,270,50,1,0,0,0,271,269,1,0,0,0,272,274,3,17,4,0,
		273,272,1,0,0,0,273,274,1,0,0,0,274,275,1,0,0,0,275,276,6,21,7,0,276,277,
		6,21,8,0,277,52,1,0,0,0,278,279,5,61,0,0,279,280,1,0,0,0,280,281,6,22,
		9,0,281,282,6,22,10,0,282,54,1,0,0,0,283,284,3,19,5,0,284,285,1,0,0,0,
		285,286,6,23,11,0,286,56,1,0,0,0,287,288,3,27,9,0,288,289,1,0,0,0,289,
		290,6,24,12,0,290,58,1,0,0,0,291,295,3,9,0,0,292,294,3,11,1,0,293,292,
		1,0,0,0,294,297,1,0,0,0,295,293,1,0,0,0,295,296,1,0,0,0,296,298,1,0,0,
		0,297,295,1,0,0,0,298,299,6,25,13,0,299,60,1,0,0,0,300,301,5,123,0,0,301,
		302,1,0,0,0,302,303,6,26,1,0,303,304,6,26,14,0,304,62,1,0,0,0,305,306,
		5,125,0,0,306,307,1,0,0,0,307,308,6,27,15,0,308,309,6,27,8,0,309,310,6,
		27,8,0,310,311,6,27,16,0,311,64,1,0,0,0,312,313,3,17,4,0,313,314,3,27,
		9,0,314,315,1,0,0,0,315,316,6,28,17,0,316,317,6,28,18,0,317,66,1,0,0,0,
		318,319,3,17,4,0,319,320,1,0,0,0,320,321,6,29,7,0,321,322,6,29,8,0,322,
		68,1,0,0,0,323,324,3,27,9,0,324,325,1,0,0,0,325,326,6,30,12,0,326,70,1,
		0,0,0,327,331,3,13,2,0,328,330,3,11,1,0,329,328,1,0,0,0,330,333,1,0,0,
		0,331,329,1,0,0,0,331,332,1,0,0,0,332,334,1,0,0,0,333,331,1,0,0,0,334,
		335,6,31,13,0,335,72,1,0,0,0,336,337,5,123,0,0,337,338,1,0,0,0,338,339,
		6,32,1,0,339,340,6,32,14,0,340,74,1,0,0,0,341,342,5,125,0,0,342,343,1,
		0,0,0,343,344,6,33,15,0,344,345,6,33,8,0,345,346,6,33,10,0,346,76,1,0,
		0,0,347,348,5,42,0,0,348,349,1,0,0,0,349,350,6,34,19,0,350,351,6,34,8,
		0,351,78,1,0,0,0,352,353,5,91,0,0,353,354,1,0,0,0,354,355,6,35,20,0,355,
		356,6,35,8,0,356,80,1,0,0,0,357,358,5,46,0,0,358,359,1,0,0,0,359,360,6,
		36,5,0,360,361,6,36,21,0,361,82,1,0,0,0,362,363,3,17,4,0,363,364,3,27,
		9,0,364,365,1,0,0,0,365,366,6,37,17,0,366,84,1,0,0,0,367,368,3,17,4,0,
		368,369,1,0,0,0,369,370,6,38,7,0,370,371,6,38,8,0,371,86,1,0,0,0,372,373,
		5,123,0,0,373,374,1,0,0,0,374,375,6,39,1,0,375,376,6,39,2,0,376,88,1,0,
		0,0,377,378,5,125,0,0,378,379,1,0,0,0,379,380,6,40,15,0,380,381,6,40,10,
		0,381,90,1,0,0,0,382,383,5,40,0,0,383,384,1,0,0,0,384,385,6,41,22,0,385,
		386,6,41,23,0,386,92,1,0,0,0,387,388,5,34,0,0,388,389,1,0,0,0,389,390,
		6,42,24,0,390,391,6,42,25,0,391,94,1,0,0,0,392,393,5,36,0,0,393,394,1,
		0,0,0,394,395,6,43,26,0,395,96,1,0,0,0,396,397,3,31,11,0,397,398,1,0,0,
		0,398,399,6,44,27,0,399,98,1,0,0,0,400,401,5,45,0,0,401,402,1,0,0,0,402,
		403,6,45,28,0,403,100,1,0,0,0,404,405,5,46,0,0,405,406,1,0,0,0,406,407,
		6,46,29,0,407,102,1,0,0,0,408,409,5,45,0,0,409,410,5,62,0,0,410,411,1,
		0,0,0,411,412,6,47,30,0,412,413,6,47,31,0,413,104,1,0,0,0,414,415,3,19,
		5,0,415,416,1,0,0,0,416,417,6,48,32,0,417,106,1,0,0,0,418,419,3,17,4,0,
		419,420,3,27,9,0,420,421,1,0,0,0,421,422,6,49,17,0,422,423,6,49,6,0,423,
		108,1,0,0,0,424,425,3,17,4,0,425,426,1,0,0,0,426,427,6,50,7,0,427,110,
		1,0,0,0,428,429,3,27,9,0,429,430,1,0,0,0,430,431,6,51,12,0,431,112,1,0,
		0,0,432,433,5,123,0,0,433,434,1,0,0,0,434,435,6,52,1,0,435,436,6,52,2,
		0,436,114,1,0,0,0,437,438,5,125,0,0,438,439,1,0,0,0,439,440,6,53,15,0,
		440,441,6,53,8,0,441,116,1,0,0,0,442,443,5,40,0,0,443,444,1,0,0,0,444,
		445,6,54,22,0,445,446,6,54,23,0,446,118,1,0,0,0,447,448,5,41,0,0,448,449,
		1,0,0,0,449,450,6,55,33,0,450,451,6,55,8,0,451,120,1,0,0,0,452,453,5,44,
		0,0,453,454,1,0,0,0,454,455,6,56,34,0,455,122,1,0,0,0,456,457,5,58,0,0,
		457,458,1,0,0,0,458,459,6,57,35,0,459,124,1,0,0,0,460,461,5,34,0,0,461,
		462,1,0,0,0,462,463,6,58,24,0,463,464,6,58,25,0,464,126,1,0,0,0,465,466,
		5,36,0,0,466,467,1,0,0,0,467,468,6,59,26,0,468,128,1,0,0,0,469,470,3,31,
		11,0,470,471,1,0,0,0,471,472,6,60,27,0,472,130,1,0,0,0,473,474,5,45,0,
		0,474,475,1,0,0,0,475,476,6,61,28,0,476,132,1,0,0,0,477,478,5,46,0,0,478,
		479,1,0,0,0,479,480,6,62,29,0,480,134,1,0,0,0,481,482,5,45,0,0,482,483,
		5,62,0,0,483,484,1,0,0,0,484,485,6,63,30,0,485,486,6,63,36,0,486,136,1,
		0,0,0,487,488,3,19,5,0,488,489,1,0,0,0,489,490,6,64,32,0,490,138,1,0,0,
		0,491,492,3,17,4,0,492,493,3,27,9,0,493,494,1,0,0,0,494,495,6,65,17,0,
		495,496,6,65,6,0,496,140,1,0,0,0,497,498,3,17,4,0,498,499,1,0,0,0,499,
		500,6,66,7,0,500,142,1,0,0,0,501,502,3,27,9,0,502,503,1,0,0,0,503,504,
		6,67,12,0,504,144,1,0,0,0,505,506,5,42,0,0,506,507,1,0,0,0,507,508,6,68,
		19,0,508,146,1,0,0,0,509,510,5,91,0,0,510,511,1,0,0,0,511,512,6,69,20,
		0,512,148,1,0,0,0,513,514,5,93,0,0,514,515,1,0,0,0,515,516,6,70,37,0,516,
		517,6,70,16,0,517,150,1,0,0,0,518,519,5,123,0,0,519,520,1,0,0,0,520,521,
		6,71,1,0,521,522,6,71,2,0,522,152,1,0,0,0,523,524,5,125,0,0,524,525,1,
		0,0,0,525,526,6,72,15,0,526,527,6,72,8,0,527,154,1,0,0,0,528,529,3,19,
		5,0,529,530,1,0,0,0,530,531,6,73,32,0,531,156,1,0,0,0,532,533,3,31,11,
		0,533,534,1,0,0,0,534,535,6,74,27,0,535,158,1,0,0,0,536,537,3,17,4,0,537,
		538,3,27,9,0,538,539,1,0,0,0,539,540,6,75,17,0,540,160,1,0,0,0,541,542,
		3,17,4,0,542,543,1,0,0,0,543,544,6,76,7,0,544,162,1,0,0,0,545,546,3,27,
		9,0,546,547,1,0,0,0,547,548,6,77,12,0,548,164,1,0,0,0,549,550,5,92,0,0,
		550,551,7,8,0,0,551,166,1,0,0,0,552,553,5,92,0,0,553,554,5,117,0,0,554,
		555,1,0,0,0,555,556,7,9,0,0,556,557,7,9,0,0,557,558,7,9,0,0,558,569,7,
		9,0,0,559,560,5,92,0,0,560,561,5,85,0,0,561,562,1,0,0,0,562,563,7,9,0,
		0,563,564,7,9,0,0,564,565,7,9,0,0,565,566,7,9,0,0,566,567,7,9,0,0,567,
		569,7,9,0,0,568,552,1,0,0,0,568,559,1,0,0,0,569,168,1,0,0,0,570,571,5,
		34,0,0,571,572,1,0,0,0,572,573,6,80,8,0,573,170,1,0,0,0,574,576,3,15,3,
		0,575,574,1,0,0,0,576,577,1,0,0,0,577,575,1,0,0,0,577,578,1,0,0,0,578,
		172,1,0,0,0,27,0,1,2,3,4,5,6,7,8,184,188,190,199,208,212,214,217,222,228,
		230,234,269,273,295,331,568,577,38,5,1,0,7,10,0,5,5,0,7,4,0,5,2,0,7,9,
		0,5,4,0,7,1,0,4,0,0,7,7,0,2,3,0,7,5,0,7,3,0,7,8,0,2,5,0,7,11,0,5,3,0,7,
		2,0,2,4,0,7,22,0,7,23,0,2,2,0,7,18,0,5,6,0,7,12,0,5,8,0,7,14,0,7,13,0,
		7,15,0,7,16,0,7,17,0,2,7,0,7,6,0,7,19,0,7,20,0,7,21,0,5,7,0,7,24,0
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
} // namespace FluentTranslate.Parser
