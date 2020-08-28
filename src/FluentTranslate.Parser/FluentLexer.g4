lexer grammar FluentLexer;

tokens { NL, INDENT, IDENTIFIER, TEXT, ATTRIBUTE, PLACEABLE_OPEN, PLACEABLE_CLOSE, STRING_OPEN, NUMBER_LITERAL, VARIABLE_REF, TERM_REF, ATTRIBUTE_REF, MESSAGE_REF, FUNCTION_CALL, SELECTOR }

fragment InlineChar		: ~( '{' | '}' | '\r' | '\n' ) ;
fragment IndentedChar	: ~( '{' | '}' | '[' | '*' | '.' | '\r' | '\n' | ' ' ) ;
fragment QuotedChar		: ~( '"' | '\\' | '\r' | '\n' ) ;
fragment Newline		: '\r\n' | '\n' ;
fragment Identifier		: [a-zA-Z] ([a-zA-Z0-9] | '_' | '-')* ;
fragment CommentPrefix	: '###' | '##' | '#' ;
fragment CommentText	: ~[\r\n]* ;
fragment IndentChar		: [' '] ;
fragment Indent			: IndentChar+ ;
fragment Whitespace		: (IndentChar | Newline)+ ;
fragment NumberLiteral	: '-'? [0-9]+ ( '.' [0-9]+ )? ;

COMMENT_OPEN		: Prefix=(CommentPrefix) -> pushMode(COMMENTS), more ;
TERM				: '-' Name=(Identifier) Indent? '=' Indent? -> pushMode(SINGLELINE) ;
MESSAGE				: Name=(Identifier) Indent? '=' Indent? -> pushMode(SINGLELINE) ;
NL					: (Indent? Newline)* Newline ;

mode COMMENTS;
COMMENT				: Text=(CommentText) Newline? -> popMode ;

mode SINGLELINE;
SL_PLACEABLE_OPEN	: '{' Indent? -> type(PLACEABLE_OPEN), pushMode(PLACEABLES);
SL_MULTILINE		: Newline+ Indent -> mode(MULTILINE), more ;
SL_NL				: Newline -> type(NL), popMode ;
SL_TEXT				: InlineChar+ -> type(TEXT) ;

mode MULTILINE;
ML_PLACEABLE_OPEN	: '{' Indent? -> type(PLACEABLE_OPEN), mode(SINGLELINE), pushMode(PLACEABLES);
ML_NEXT				: Newline+ Indent -> more ;
ML_NL				: Newline -> type(NL), popMode ;
ML_TEXT				: IndentedChar InlineChar* -> type(TEXT) ;
ML_ATTRIBUTE		: '.' Name=(Identifier) Indent '=' -> type(ATTRIBUTE), mode(SINGLELINE) ;
ML_INDENT			: Indent -> more ;

mode PLACEABLES;
PL_PLACEABLE_OPEN	: '{' Whitespace? -> type(PLACEABLE_OPEN), pushMode(PLACEABLES);
PL_PLACEABLE_CLOSE	: Whitespace? '}' -> type(PLACEABLE_CLOSE), popMode ;
PL_STRING_OPEN		: '"' -> type(STRING_OPEN), pushMode(STRINGS) ;
PL_NUMBER_LITERAL	: NumberLiteral -> type(NUMBER_LITERAL) ;
PL_VARIABLE_REF		: '$' PL_IDENTIFIER -> type(VARIABLE_REF) ;
PL_TERM_CALL		: PL_TERM_REF Whitespace? '(' -> type(TERM_REF), pushMode(CALLS) ;
PL_TERM_REF		: '-' PL_IDENTIFIER PL_ATTRIBUTE_REF? -> type(TERM_REF) ;
PL_FUNCTION_CALL	: PL_IDENTIFIER Whitespace? '(' Whitespace? -> type(FUNCTION_CALL), pushMode(CALLS) ;
PL_MESSAGE_REF		: PL_IDENTIFIER PL_ATTRIBUTE_REF? -> type(MESSAGE_REF) ;
PL_ATTRIBUTE_REF	: '.' PL_IDENTIFIER -> type(ATTRIBUTE_REF) ;
PL_IDENTIFIER		: Name=(Identifier) -> type(IDENTIFIER) ;
PL_SELECTOR			: Whitespace? '->' Whitespace -> type(SELECTOR), pushMode(VARIANTS) ;
PL_INDENT			: Indent -> type(INDENT) ;
PL_NL				: Newline -> type(NL) ;

mode CALLS;
CALL_CLOSE			: Whitespace? ')' -> popMode ;
CALL_ARG_SEP		: Whitespace? ',' Whitespace? ;
CALL_NAMED_ARG		: CALL_IDENTIFIER Whitespace? ':' Whitespace? ;
CALL_STRING_OPEN	: '"' -> type(STRING_OPEN), pushMode(STRINGS) ;
CALL_NUMBER_LITERAL	: NumberLiteral -> type(NUMBER_LITERAL) ;
CALL_VARIABLE_REF	: '$' CALL_IDENTIFIER -> type(VARIABLE_REF) ;
CALL_TERM_CALL		: CALL_TERM_REF Whitespace? '(' -> type(TERM_REF), pushMode(CALLS) ;
CALL_TERM_REF		: '-' CALL_IDENTIFIER CALL_ATTRIBUTE_REF? -> type(TERM_REF) ;
CALL_FUNCTION_CALL	: CALL_IDENTIFIER Whitespace? '(' Whitespace? -> type(FUNCTION_CALL) ;
CALL_MESSAGE_CALL	: CALL_IDENTIFIER CALL_ATTRIBUTE_REF? -> type(MESSAGE_REF) ;
CALL_ATTRIBUTE_REF	: '.' CALL_IDENTIFIER -> type(ATTRIBUTE_REF) ;
CALL_IDENTIFIER		: Name=(Identifier) -> type(IDENTIFIER) ;
CALL_SELECTOR		: Whitespace? '->' Whitespace -> type(SELECTOR), pushMode(VARIANTS) ;
CALL_INDENT			: Indent -> type(INDENT) ;
CALL_NL				: Newline -> type(NL) ;

mode VARIANTS;
VARIANT_DEFAULT		: '*' ;
VARIANT_KEY			: '[' Whitespace? (NumberLiteral | Identifier) Whitespace? ']' -> pushMode(SINGLELINE) ;
VA_PLACEABLE_CLOSE	: Whitespace? '}' -> type(PLACEABLE_CLOSE), popMode, popMode ;
VA_INDENT			: Indent -> type(INDENT) ;
VA_NL				: Newline -> type(NL) ;

mode VAR_SINGLELINE;
VSL_PLACEABLE_OPEN	: '{' Indent? -> type(PLACEABLE_OPEN), pushMode(PLACEABLES);
VSL_MULTILINE		: Newline+ Indent -> mode(VSL_MULTILINE), more ;
VSL_NL				: Newline -> type(NL), popMode ;
VSL_TEXT			: InlineChar+ -> type(TEXT) ;

mode STRINGS;
ESCAPED_CHAR		: '\\' ('"' | '\\') ;
UNICODE_ESCAPE		: '\\u' [0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]
					| '\\U' [0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]
					;
STRING_CLOSE		: '"' -> popMode ;
QUOTED_STRING		: QuotedChar+ ;