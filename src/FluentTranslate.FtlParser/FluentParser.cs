//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.8
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from d:\projects\FluentTranslate\src\FluentTranslate.FtlParser\Fluent.g4 by ANTLR 4.8

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.8")]
[System.CLSCompliant(false)]
public partial class FluentParser : Parser {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		COMMNET_L3=1, COMMENT_L2=2, COMMENT_L1=3, COMMENT=4, WS=5, EQ=6, WORD=7, 
		TEXT=8, WHITESPACE=9, NEWLINE=10, COMMNET_L2=11, COMMNET_L1=12;
	public const int
		RULE_message_list = 0, RULE_message = 1, RULE_comment_l3 = 2, RULE_comment_l2 = 3, 
		RULE_comment_l1 = 4, RULE_name = 5, RULE_opinion = 6;
	public static readonly string[] ruleNames = {
		"message_list", "message", "comment_l3", "comment_l2", "comment_l1", "name", 
		"opinion"
	};

	private static readonly string[] _LiteralNames = {
		null, null, null, null, null, null, "'='"
	};
	private static readonly string[] _SymbolicNames = {
		null, "COMMNET_L3", "COMMENT_L2", "COMMENT_L1", "COMMENT", "WS", "EQ", 
		"WORD", "TEXT", "WHITESPACE", "NEWLINE", "COMMNET_L2", "COMMNET_L1"
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

	public override string GrammarFileName { get { return "Fluent.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string SerializedAtn { get { return new string(_serializedATN); } }

	static FluentParser() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}

		public FluentParser(ITokenStream input) : this(input, Console.Out, Console.Error) { }

		public FluentParser(ITokenStream input, TextWriter output, TextWriter errorOutput)
		: base(input, output, errorOutput)
	{
		Interpreter = new ParserATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	public partial class Message_listContext : ParserRuleContext {
		public MessageContext[] message() {
			return GetRuleContexts<MessageContext>();
		}
		public MessageContext message(int i) {
			return GetRuleContext<MessageContext>(i);
		}
		public ITerminalNode Eof() { return GetToken(FluentParser.Eof, 0); }
		public ITerminalNode[] NEWLINE() { return GetTokens(FluentParser.NEWLINE); }
		public ITerminalNode NEWLINE(int i) {
			return GetToken(FluentParser.NEWLINE, i);
		}
		public Message_listContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_message_list; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IFluentVisitor<TResult> typedVisitor = visitor as IFluentVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitMessage_list(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public Message_listContext message_list() {
		Message_listContext _localctx = new Message_listContext(Context, State);
		EnterRule(_localctx, 0, RULE_message_list);
		int _la;
		try {
			int _alt;
			EnterOuterAlt(_localctx, 1);
			{
			State = 14; message();
			State = 19;
			ErrorHandler.Sync(this);
			_alt = Interpreter.AdaptivePredict(TokenStream,0,Context);
			while ( _alt!=2 && _alt!=global::Antlr4.Runtime.Atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					{
					{
					State = 15; Match(NEWLINE);
					State = 16; message();
					}
					} 
				}
				State = 21;
				ErrorHandler.Sync(this);
				_alt = Interpreter.AdaptivePredict(TokenStream,0,Context);
			}
			State = 23;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if (_la==NEWLINE) {
				{
				State = 22; Match(NEWLINE);
				}
			}

			State = 25; Match(Eof);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class MessageContext : ParserRuleContext {
		public NameContext name() {
			return GetRuleContext<NameContext>(0);
		}
		public ITerminalNode EQ() { return GetToken(FluentParser.EQ, 0); }
		public ITerminalNode[] WS() { return GetTokens(FluentParser.WS); }
		public ITerminalNode WS(int i) {
			return GetToken(FluentParser.WS, i);
		}
		public MessageContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_message; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IFluentVisitor<TResult> typedVisitor = visitor as IFluentVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitMessage(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public MessageContext message() {
		MessageContext _localctx = new MessageContext(Context, State);
		EnterRule(_localctx, 2, RULE_message);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 27; name();
			State = 29;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if (_la==WS) {
				{
				State = 28; Match(WS);
				}
			}

			State = 31; Match(EQ);
			State = 33;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if (_la==WS) {
				{
				State = 32; Match(WS);
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class Comment_l3Context : ParserRuleContext {
		public ITerminalNode COMMNET_L3() { return GetToken(FluentParser.COMMNET_L3, 0); }
		public ITerminalNode COMMENT() { return GetToken(FluentParser.COMMENT, 0); }
		public ITerminalNode NEWLINE() { return GetToken(FluentParser.NEWLINE, 0); }
		public ITerminalNode Eof() { return GetToken(FluentParser.Eof, 0); }
		public ITerminalNode[] WS() { return GetTokens(FluentParser.WS); }
		public ITerminalNode WS(int i) {
			return GetToken(FluentParser.WS, i);
		}
		public Comment_l3Context(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_comment_l3; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IFluentVisitor<TResult> typedVisitor = visitor as IFluentVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitComment_l3(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public Comment_l3Context comment_l3() {
		Comment_l3Context _localctx = new Comment_l3Context(Context, State);
		EnterRule(_localctx, 4, RULE_comment_l3);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 36;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if (_la==WS) {
				{
				State = 35; Match(WS);
				}
			}

			State = 38; Match(COMMNET_L3);
			State = 40;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if (_la==WS) {
				{
				State = 39; Match(WS);
				}
			}

			State = 42; Match(COMMENT);
			State = 43;
			_la = TokenStream.LA(1);
			if ( !(_la==Eof || _la==NEWLINE) ) {
			ErrorHandler.RecoverInline(this);
			}
			else {
				ErrorHandler.ReportMatch(this);
			    Consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class Comment_l2Context : ParserRuleContext {
		public ITerminalNode COMMNET_L2() { return GetToken(FluentParser.COMMNET_L2, 0); }
		public ITerminalNode COMMENT() { return GetToken(FluentParser.COMMENT, 0); }
		public ITerminalNode NEWLINE() { return GetToken(FluentParser.NEWLINE, 0); }
		public ITerminalNode Eof() { return GetToken(FluentParser.Eof, 0); }
		public ITerminalNode[] WS() { return GetTokens(FluentParser.WS); }
		public ITerminalNode WS(int i) {
			return GetToken(FluentParser.WS, i);
		}
		public Comment_l2Context(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_comment_l2; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IFluentVisitor<TResult> typedVisitor = visitor as IFluentVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitComment_l2(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public Comment_l2Context comment_l2() {
		Comment_l2Context _localctx = new Comment_l2Context(Context, State);
		EnterRule(_localctx, 6, RULE_comment_l2);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 46;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if (_la==WS) {
				{
				State = 45; Match(WS);
				}
			}

			State = 48; Match(COMMNET_L2);
			State = 50;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if (_la==WS) {
				{
				State = 49; Match(WS);
				}
			}

			State = 52; Match(COMMENT);
			State = 53;
			_la = TokenStream.LA(1);
			if ( !(_la==Eof || _la==NEWLINE) ) {
			ErrorHandler.RecoverInline(this);
			}
			else {
				ErrorHandler.ReportMatch(this);
			    Consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class Comment_l1Context : ParserRuleContext {
		public ITerminalNode COMMNET_L1() { return GetToken(FluentParser.COMMNET_L1, 0); }
		public ITerminalNode COMMENT() { return GetToken(FluentParser.COMMENT, 0); }
		public ITerminalNode NEWLINE() { return GetToken(FluentParser.NEWLINE, 0); }
		public ITerminalNode Eof() { return GetToken(FluentParser.Eof, 0); }
		public ITerminalNode[] WS() { return GetTokens(FluentParser.WS); }
		public ITerminalNode WS(int i) {
			return GetToken(FluentParser.WS, i);
		}
		public Comment_l1Context(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_comment_l1; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IFluentVisitor<TResult> typedVisitor = visitor as IFluentVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitComment_l1(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public Comment_l1Context comment_l1() {
		Comment_l1Context _localctx = new Comment_l1Context(Context, State);
		EnterRule(_localctx, 8, RULE_comment_l1);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 56;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if (_la==WS) {
				{
				State = 55; Match(WS);
				}
			}

			State = 58; Match(COMMNET_L1);
			State = 60;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			if (_la==WS) {
				{
				State = 59; Match(WS);
				}
			}

			State = 62; Match(COMMENT);
			State = 63;
			_la = TokenStream.LA(1);
			if ( !(_la==Eof || _la==NEWLINE) ) {
			ErrorHandler.RecoverInline(this);
			}
			else {
				ErrorHandler.ReportMatch(this);
			    Consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class NameContext : ParserRuleContext {
		public ITerminalNode WORD() { return GetToken(FluentParser.WORD, 0); }
		public NameContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_name; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IFluentVisitor<TResult> typedVisitor = visitor as IFluentVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitName(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public NameContext name() {
		NameContext _localctx = new NameContext(Context, State);
		EnterRule(_localctx, 10, RULE_name);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 65; Match(WORD);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class OpinionContext : ParserRuleContext {
		public ITerminalNode TEXT() { return GetToken(FluentParser.TEXT, 0); }
		public OpinionContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_opinion; } }
		public override TResult Accept<TResult>(IParseTreeVisitor<TResult> visitor) {
			IFluentVisitor<TResult> typedVisitor = visitor as IFluentVisitor<TResult>;
			if (typedVisitor != null) return typedVisitor.VisitOpinion(this);
			else return visitor.VisitChildren(this);
		}
	}

	[RuleVersion(0)]
	public OpinionContext opinion() {
		OpinionContext _localctx = new OpinionContext(Context, State);
		EnterRule(_localctx, 12, RULE_opinion);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 67; Match(TEXT);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	private static char[] _serializedATN = {
		'\x3', '\x608B', '\xA72A', '\x8133', '\xB9ED', '\x417C', '\x3BE7', '\x7786', 
		'\x5964', '\x3', '\xE', 'H', '\x4', '\x2', '\t', '\x2', '\x4', '\x3', 
		'\t', '\x3', '\x4', '\x4', '\t', '\x4', '\x4', '\x5', '\t', '\x5', '\x4', 
		'\x6', '\t', '\x6', '\x4', '\a', '\t', '\a', '\x4', '\b', '\t', '\b', 
		'\x3', '\x2', '\x3', '\x2', '\x3', '\x2', '\a', '\x2', '\x14', '\n', '\x2', 
		'\f', '\x2', '\xE', '\x2', '\x17', '\v', '\x2', '\x3', '\x2', '\x5', '\x2', 
		'\x1A', '\n', '\x2', '\x3', '\x2', '\x3', '\x2', '\x3', '\x3', '\x3', 
		'\x3', '\x5', '\x3', ' ', '\n', '\x3', '\x3', '\x3', '\x3', '\x3', '\x5', 
		'\x3', '$', '\n', '\x3', '\x3', '\x4', '\x5', '\x4', '\'', '\n', '\x4', 
		'\x3', '\x4', '\x3', '\x4', '\x5', '\x4', '+', '\n', '\x4', '\x3', '\x4', 
		'\x3', '\x4', '\x3', '\x4', '\x3', '\x5', '\x5', '\x5', '\x31', '\n', 
		'\x5', '\x3', '\x5', '\x3', '\x5', '\x5', '\x5', '\x35', '\n', '\x5', 
		'\x3', '\x5', '\x3', '\x5', '\x3', '\x5', '\x3', '\x6', '\x5', '\x6', 
		';', '\n', '\x6', '\x3', '\x6', '\x3', '\x6', '\x5', '\x6', '?', '\n', 
		'\x6', '\x3', '\x6', '\x3', '\x6', '\x3', '\x6', '\x3', '\a', '\x3', '\a', 
		'\x3', '\b', '\x3', '\b', '\x3', '\b', '\x2', '\x2', '\t', '\x2', '\x4', 
		'\x6', '\b', '\n', '\f', '\xE', '\x2', '\x3', '\x3', '\x3', '\f', '\f', 
		'\x2', 'J', '\x2', '\x10', '\x3', '\x2', '\x2', '\x2', '\x4', '\x1D', 
		'\x3', '\x2', '\x2', '\x2', '\x6', '&', '\x3', '\x2', '\x2', '\x2', '\b', 
		'\x30', '\x3', '\x2', '\x2', '\x2', '\n', ':', '\x3', '\x2', '\x2', '\x2', 
		'\f', '\x43', '\x3', '\x2', '\x2', '\x2', '\xE', '\x45', '\x3', '\x2', 
		'\x2', '\x2', '\x10', '\x15', '\x5', '\x4', '\x3', '\x2', '\x11', '\x12', 
		'\a', '\f', '\x2', '\x2', '\x12', '\x14', '\x5', '\x4', '\x3', '\x2', 
		'\x13', '\x11', '\x3', '\x2', '\x2', '\x2', '\x14', '\x17', '\x3', '\x2', 
		'\x2', '\x2', '\x15', '\x13', '\x3', '\x2', '\x2', '\x2', '\x15', '\x16', 
		'\x3', '\x2', '\x2', '\x2', '\x16', '\x19', '\x3', '\x2', '\x2', '\x2', 
		'\x17', '\x15', '\x3', '\x2', '\x2', '\x2', '\x18', '\x1A', '\a', '\f', 
		'\x2', '\x2', '\x19', '\x18', '\x3', '\x2', '\x2', '\x2', '\x19', '\x1A', 
		'\x3', '\x2', '\x2', '\x2', '\x1A', '\x1B', '\x3', '\x2', '\x2', '\x2', 
		'\x1B', '\x1C', '\a', '\x2', '\x2', '\x3', '\x1C', '\x3', '\x3', '\x2', 
		'\x2', '\x2', '\x1D', '\x1F', '\x5', '\f', '\a', '\x2', '\x1E', ' ', '\a', 
		'\a', '\x2', '\x2', '\x1F', '\x1E', '\x3', '\x2', '\x2', '\x2', '\x1F', 
		' ', '\x3', '\x2', '\x2', '\x2', ' ', '!', '\x3', '\x2', '\x2', '\x2', 
		'!', '#', '\a', '\b', '\x2', '\x2', '\"', '$', '\a', '\a', '\x2', '\x2', 
		'#', '\"', '\x3', '\x2', '\x2', '\x2', '#', '$', '\x3', '\x2', '\x2', 
		'\x2', '$', '\x5', '\x3', '\x2', '\x2', '\x2', '%', '\'', '\a', '\a', 
		'\x2', '\x2', '&', '%', '\x3', '\x2', '\x2', '\x2', '&', '\'', '\x3', 
		'\x2', '\x2', '\x2', '\'', '(', '\x3', '\x2', '\x2', '\x2', '(', '*', 
		'\a', '\x3', '\x2', '\x2', ')', '+', '\a', '\a', '\x2', '\x2', '*', ')', 
		'\x3', '\x2', '\x2', '\x2', '*', '+', '\x3', '\x2', '\x2', '\x2', '+', 
		',', '\x3', '\x2', '\x2', '\x2', ',', '-', '\a', '\x6', '\x2', '\x2', 
		'-', '.', '\t', '\x2', '\x2', '\x2', '.', '\a', '\x3', '\x2', '\x2', '\x2', 
		'/', '\x31', '\a', '\a', '\x2', '\x2', '\x30', '/', '\x3', '\x2', '\x2', 
		'\x2', '\x30', '\x31', '\x3', '\x2', '\x2', '\x2', '\x31', '\x32', '\x3', 
		'\x2', '\x2', '\x2', '\x32', '\x34', '\a', '\r', '\x2', '\x2', '\x33', 
		'\x35', '\a', '\a', '\x2', '\x2', '\x34', '\x33', '\x3', '\x2', '\x2', 
		'\x2', '\x34', '\x35', '\x3', '\x2', '\x2', '\x2', '\x35', '\x36', '\x3', 
		'\x2', '\x2', '\x2', '\x36', '\x37', '\a', '\x6', '\x2', '\x2', '\x37', 
		'\x38', '\t', '\x2', '\x2', '\x2', '\x38', '\t', '\x3', '\x2', '\x2', 
		'\x2', '\x39', ';', '\a', '\a', '\x2', '\x2', ':', '\x39', '\x3', '\x2', 
		'\x2', '\x2', ':', ';', '\x3', '\x2', '\x2', '\x2', ';', '<', '\x3', '\x2', 
		'\x2', '\x2', '<', '>', '\a', '\xE', '\x2', '\x2', '=', '?', '\a', '\a', 
		'\x2', '\x2', '>', '=', '\x3', '\x2', '\x2', '\x2', '>', '?', '\x3', '\x2', 
		'\x2', '\x2', '?', '@', '\x3', '\x2', '\x2', '\x2', '@', '\x41', '\a', 
		'\x6', '\x2', '\x2', '\x41', '\x42', '\t', '\x2', '\x2', '\x2', '\x42', 
		'\v', '\x3', '\x2', '\x2', '\x2', '\x43', '\x44', '\a', '\t', '\x2', '\x2', 
		'\x44', '\r', '\x3', '\x2', '\x2', '\x2', '\x45', '\x46', '\a', '\n', 
		'\x2', '\x2', '\x46', '\xF', '\x3', '\x2', '\x2', '\x2', '\f', '\x15', 
		'\x19', '\x1F', '#', '&', '*', '\x30', '\x34', ':', '>',
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
