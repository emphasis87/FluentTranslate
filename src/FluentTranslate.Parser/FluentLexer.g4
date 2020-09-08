lexer grammar FluentLexer;

tokens { 
	NL,
	NL_INDENT,
	INDENT,
	TERM,
	IDENTIFIER,
	IDENTIFIER_REF,
	EQUALS,
	TEXT,
	ATTRIBUTE,
	PLACEABLE_OPEN,
	PLACEABLE_CLOSE,
	STRING_OPEN,
	NUMBER_LITERAL,
	VARIABLE_REF,
	TERM_REF,
	ATTRIBUTE_REF,
	SELECTOR,
	CALL_OPEN,
	CALL_CLOSE,
	CALL_ARG_SEP,
	CALL_ARG_NAME_SEP,
	VARIANT_DEFAULT,
	VARIANT_OPEN,
	VARIANT_CLOSE
}

fragment InlinePrintable: ~( '{' | '}' | '\r' | '\n' | ' ' ) ;
fragment InlineChar		: ~( '{' | '}' | '\r' | '\n' ) ;
fragment IndentedChar	: ~( '{' | '}' | '[' | '*' | '.' | '\r' | '\n' ) ;
fragment QuotedChar		: ~( '"' | '\\' | '\r' | '\n' ) ;
fragment Newline		: '\r\n' | '\n' ;
fragment Identifier		: [a-zA-Z] ([a-zA-Z0-9] | '_' | '-')* ;
fragment CommentPrefix	: '###' | '##' | '#' ;
fragment CommentChar	: ~('\r' | '\n') ;
fragment IndentChar		: ' ' ;
fragment Indent			: IndentChar+ ;
fragment Whitespace		: (IndentChar | Newline)+ ;
fragment NumberLiteral	: '-'? [0-9]+ ( '.' [0-9]+ )? ;

COMMENT_OPEN		: CommentPrefix ' '? -> pushMode(COMMENTS) ;
PLACEABLE_OPEN		: '{' -> type(PLACEABLE_OPEN), pushMode(PLACEABLES) ;
TERM				: '-' -> type(TERM), pushMode(RECORDS) ;
ATTRIBUTE			: '.' -> type(ATTRIBUTE), pushMode(RECORDS) ;
IDENTIFIER			: Identifier -> pushMode(RECORDS) ;
NL_INDENT			: Newline Indent -> pushMode(MULTILINE) ;
NL					: Newline ;
INDENT				: Indent ;

mode COMMENTS;
COMMENT				: CommentChar*;
CM_NL				: Newline? -> type(NL), popMode ;

mode RECORDS;
RC_EQUALS			: '=' -> type(EQUALS), mode(SINGLELINE) ;
RC_IDENTIFIER		: Identifier -> type(IDENTIFIER) ;
RC_INDENT			: Indent -> type(INDENT) ;

mode SINGLELINE;
SL_TEXT				: InlinePrintable InlineChar* -> type(TEXT) ;
SL_PLACEABLE_OPEN	: '{' -> type(PLACEABLE_OPEN), mode(PLACEABLES) ;
SL_PLACEABLE_CLOSE	: '}' -> type(PLACEABLE_CLOSE), popMode, popMode, pushMode(SINGLELINE) ;
SL_NL_INDENT		: Newline Indent -> type(NL_INDENT), mode(MULTILINE) ;
SL_NL				: Newline -> type(NL), popMode ;
SL_INDENT			: Indent -> type(INDENT) ;

mode MULTILINE;
ML_TEXT				: IndentedChar InlineChar* -> type(TEXT) ;
ML_PLACEABLE_OPEN	: '{' -> type(PLACEABLE_OPEN), mode(PLACEABLES) ;
ML_PLACEABLE_CLOSE	: '}' -> type(PLACEABLE_CLOSE), popMode, mode(SINGLELINE) ;
ML_VARIANT_DEFAULT	: '*' -> type(VARIANT_DEFAULT), popMode ;
ML_VARIANT_OPEN		: '[' -> type(VARIANT_OPEN), popMode ;
ML_ATTRIBUTE		: '.' -> type(ATTRIBUTE), mode(RECORDS) ;
ML_NL_INDENT		: Newline Indent -> type(NL_INDENT) ;
ML_NL				: Newline -> type(NL), popMode ;

mode PLACEABLES;
PL_PLACEABLE_OPEN	: '{' -> type(PLACEABLE_OPEN), pushMode(PLACEABLES) ;
PL_PLACEABLE_CLOSE	: '}' -> type(PLACEABLE_CLOSE), mode(SINGLELINE) ;
PL_CALL_OPEN		: '(' -> type(CALL_OPEN), pushMode(CALLS) ;
PL_STRING_OPEN		: '"' -> type(STRING_OPEN), pushMode(STRINGS) ;
PL_VARIABLE_REF		: '$' -> type(VARIABLE_REF) ;
PL_NUMBER_LITERAL	: NumberLiteral -> type(NUMBER_LITERAL) ;
PL_TERM_REF			: '-' -> type(TERM_REF) ;
PL_ATTRIBUTE_REF	: '.' -> type(ATTRIBUTE_REF) ;
PL_SELECTOR			: '->' -> type(SELECTOR), mode(VARIANTS) ;
PL_IDENTIFIER		: Identifier -> type(IDENTIFIER_REF);
PL_NL_INDENT		: Newline Indent -> type(NL_INDENT), pushMode(MULTILINE) ;
PL_NL				: Newline -> type(NL) ;
PL_INDENT			: Indent -> type(INDENT) ;

mode CALLS;
CL_PLACEABLE_OPEN	: '{' -> type(PLACEABLE_OPEN), pushMode(PLACEABLES) ;
CL_PLACEABLE_CLOSE	: '}' -> type(PLACEABLE_CLOSE), popMode ;
CL_OPEN				: '(' -> type(CALL_OPEN), pushMode(CALLS) ;
CL_CLOSE			: ')' -> type(CALL_CLOSE), popMode ;
CL_ARG_SEP			: ',' -> type(CALL_ARG_SEP) ;
CL_ARG_NAME_SEP		: ':' -> type(CALL_ARG_NAME_SEP) ;
CL_STRING_OPEN		: '"' -> type(STRING_OPEN), pushMode(STRINGS) ;
CL_VARIABLE_REF		: '$' -> type(VARIABLE_REF) ;
CL_NUMBER_LITERAL	: NumberLiteral -> type(NUMBER_LITERAL) ;
CL_TERM_REF			: '-' -> type(TERM_REF) ;
CL_ATTRIBUTE_REF	: '.' -> type(ATTRIBUTE_REF) ;
CL_SELECTOR			: '->' -> type(SELECTOR), pushMode(VARIANTS) ;
CL_IDENTIFIER		: Identifier -> type(IDENTIFIER_REF) ;
CL_NL_INDENT		: Newline Indent -> type(NL_INDENT), pushMode(MULTILINE) ;
CL_NL				: Newline -> type(NL) ;
CL_INDENT			: Indent -> type(INDENT) ;

mode VARIANTS;
VA_VARIANT_DEFAULT	: '*' -> type(VARIANT_DEFAULT) ;
VA_VARIANT_OPEN		: '[' -> type(VARIANT_OPEN) ;
VA_VARIANT_CLOSE	: ']' -> type(VARIANT_CLOSE), pushMode(SINGLELINE) ;
VA_PLACEABLE_OPEN	: '{' -> type(PLACEABLE_OPEN), pushMode(PLACEABLES) ;
VA_PLACEABLE_CLOSE	: '}' -> type(PLACEABLE_CLOSE), popMode ;
VA_IDENTIFIER		: Identifier -> type(IDENTIFIER_REF) ;
VA_NUMBER_LITERAL	: NumberLiteral -> type(NUMBER_LITERAL) ;
VA_NL_INDENT		: Newline Indent -> type(NL_INDENT) ;
VA_NL				: Newline -> type(NL) ;
VA_INDENT			: Indent -> type(INDENT) ;

mode STRINGS;
ESCAPED_CHAR		: '\\' ('"' | '\\') ;
UNICODE_ESCAPE		: '\\u' [0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]
					| '\\U' [0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]
					;
STRING_CLOSE		: '"' -> popMode ;
QUOTED_STRING		: QuotedChar+ ;