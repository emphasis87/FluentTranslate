lexer grammar FluentLexer;

fragment InlineChar	: ~( '{' | '}' | '\r' | '\n' ) ;

CMT3				: '###' ;
CMT2				: '##' ;
CMT1				: '#' ;

IDENTIFIER			: [a-zA-Z] ([a-zA-Z0-9] | '_' | '-')* ;

SPACES				: ' '+ ;
EQUALS				: SPACES? '=' SPACES? ;

mode CONTENT;
INLINE_TEXT			: InlineChar+ ;

INLINE_CHAR			: ~( '{' | '}' | '\r' | '\n' ) ;
INDENTED_CHAR		: ~( '{' | '}' | '[' | '*' | '.' | '\r' | '\n' ) ;

SPECIAL_TEXT_CHAR	: '{' | '}' ;
QUOTED_CHAR			: '\\' SPECIAL_CHAR ;
SPECIAL_CHAR		: '"' | '\\' ;

STRING_LITERAL		: '"' QUOTED_CHAR* '"' ;
NUMBER_LITERAL		: '-'? [0-9]+ ('.' [0-9]+)? ;

IDENTIFIER			: [a-zA-Z] ([a-zA-Z0-9] | '_' | '-')* ;

EQUALS				: SPACES? '=' SPACES? ;

WS					: (SPACES? | LINE_END)+ ;
SPACES				: ' '+ ;

LINE_END			: NEWLINE | EOF ;
NEWLINE				: ('\r'? '\n' | '\r') ;
