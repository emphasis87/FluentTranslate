lexer grammar FluentLexer;

fragment InlineChar	: ~( '{' | '}' | '\r' | '\n' ) ;
fragment Newline	: '\r\n' | '\n' ;

COMMENT_OPEN		: '#'   -> mode(IN_COMMENT) ;

IDENTIFIER			: [a-zA-Z] ([a-zA-Z0-9] | '_' | '-')* ;
EQUALS				: SPACES? '=' SPACES? -> mode(IN_CONTENT) ;

SPACES				: ' '+ ;

NL					: Newline ;

mode IN_COMMENT;
COMMENT_NL			: Newline -> popMode ;
COMMENT_CONTENT		: ~[\r\n]*? ;

mode IN_CONTENT;
TEXT_INLINE			: InlineChar+ ;
CONTENT_NL			: Newline -> mode(MAYBE_CONTENT) ;

mode MAYBE_CONTENT;
INDENT				: ' '+   -> mode(IN_CONTENT) ;
OTHER				: ~(' ') -> more, popMode ;

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