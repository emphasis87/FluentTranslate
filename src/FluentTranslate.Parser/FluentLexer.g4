lexer grammar FluentLexer;

tokens { NL, INDENT, PLACEABLE_OPEN, TEXT }

fragment InlineChar		: ~( '{' | '}' | '\r' | '\n' ) ;
fragment IndentedChar	: ~( '{' | '}' | '[' | '*' | '.' | '\r' | '\n' | ' ' ) ;
fragment QuotedChar		: ~( '"' | '\\' | '\r' | '\n' ) ;
fragment Newline		: '\r\n' | '\n' ;
fragment Identifier		: [a-zA-Z] ([a-zA-Z0-9] | '_' | '-')* ;
fragment Comment		: '###' | '##' | '#' ;
fragment Whitespace		: [' '] ;
fragment Indent			: Whitespace+ ;

COMMENT_OPEN		: Comment -> pushMode(COMMENTS) ;
IDENTIFIER			: Identifier ;
EQUALS				: '=' -> pushMode(SINGLELINE) ;
INDENT				: Indent ;
NL					: Newline ;

mode COMMENTS;
COMMENT_NL			: Newline -> type(NL), popMode ;
COMMENT_TEXT		: ~[\r\n]* ;

mode SINGLELINE;
SL_PLACEABLE		: '{' -> type(PLACEABLE_OPEN), mode(PLACEABLES);
SL_MULTILINE		: Newline+ Whitespace -> mode(MULTILINE), more ;
SL_NL				: Newline -> type(NL), popMode ;
SL_TEXT				: InlineChar+ -> type(TEXT) ;

mode MULTILINE;
ML_PLACEABLE_OPEN	: '{' -> type(PLACEABLE_OPEN), mode(PLACEABLES);
ML_NEXT				: Newline+ Whitespace -> more ;
ML_NL				: Newline -> type(NL), popMode ;
ML_TEXT				: IndentedChar InlineChar* -> type(TEXT) ;
ML_ATTRIBUTE		: '.' Identifier ;
ML_INDENT			: Indent -> more ;

mode PLACEABLES;
PLACEABLE_CLOSE		: '}' -> popMode ;
STRING_OPEN			: '"' -> pushMode(STRINGS) ;
NUMBER_LITERAL		: '-'? [0-9]+ ( '.' [0-9]+ )? ;
VARIABLE_REFERENCE	: '$' SELECTOR_IDENTIFIER ;
ATTRIBUTE_REF		: '.' SELECTOR_IDENTIFIER ;
CALL_OPEN			: '(' ;
CALL_CLOSE			: ')' ;
CALL_ARG_SEP		: ',' ;
PL_INDENT			: ' '+ -> type(INDENT) ;
SELECTOR_IDENTIFIER	: Identifier ;
SELECTOR_NL			: Newline -> popMode, mode(MULTILINE), pushMode(PLACEABLES) ;

mode STRINGS;
ESCAPED_CHAR		: '\\' ('"' | '\\') ;
UNICODE_ESCAPE		: '\\u' [0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F] 
					| '\\U' [0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F][0-9a-fA-F]
					;
STRING_CLOSE		: '"' -> popMode ;
QUOTED_STRING		: QuotedChar+ ;


/*
INLINE_TEXT			: InlineChar+ ;

INLINE_CHAR			: ~( '{' | '}' | '\r' | '\n' ) ;
INDENTED_CHAR		: ~( '{' | '}' | '[' | '*' | '.' | '\r' | '\n' ) ;

SPECIAL_TEXT_CHAR	: '{' | '}' ;
QUOTED_CHAR			: '\\' SPECIAL_CHAR ;
SPECIAL_CHAR		: '"' | '\\' ;

STRING_LITERAL		: '"' QUOTED_CHAR* '"' ;
NUMBER_LITERAL		: '-'? [0-9]+ ('.' [0-9]+)? ;

WS					: (SPACES? | LINE_END)+ ;
INDENT				: ' '+ ;

LINE_END			: NEWLINE | EOF ;
NEWLINE				: ('\r'? '\n' | '\r') ;
*/