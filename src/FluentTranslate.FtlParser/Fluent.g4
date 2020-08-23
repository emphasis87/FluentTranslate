grammar Fluent;

/*
 * Parser Rules
 */

fluent				: ( comment | message )* EOF ;

message				: IDENTIFIER ' '? '=' ' '? messageContent ;

messageContent		: WHITESPACE ;

comment				: comment3 | comment2 | comment1 ;

comment3			: CommentLine3+ ;
comment2			: CommentLine2+ ;
comment1			: CommentLine1+ ;

commentLine3		: COMMENT_MARK3 ( ' ' COMMENT )? LINE_END ;
commentLine2		: COMMENT_MARK2 ( ' ' COMMENT )? LINE_END ;
commentLine1		: COMMENT_MARK1 ( ' ' COMMENT )? LINE_END ;

/*
 * Lexer Rules
 */

fragment LOWERCASE  : [a-z] ;
fragment UPPERCASE  : [A-Z] ;
fragment SHARP		: '#' ;
fragment ANY		: .*? ;
fragment NEWLINE	: ('\r'? '\n' | '\r') ;

STRING_LITERAL		: '\\' ;

IDENTIFIER			: [a-zA-Z] [a-zA-Z0-9_-]* ;

LINE_END			: NEWLINE | EOF ;
COMMENT_MARK3		: '###' ;
COMMENT_MARK2		: '##' ;
COMMENT_MARK1		: '#' ;
COMMENT_CONTENT		: [] ;
WS					: (' '|'\t')+ -> skip ;
EQUALS				: '=' ;
WORD                : (LOWERCASE | UPPERCASE)+ ;
TEXT                : '"' .*? '"' ;
WHITESPACE          : (' '|'\t')+ -> skip ;