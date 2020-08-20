grammar Fluent;

/*
 * Parser Rules
 */

message_list		: message ( NEWLINE message )* ;
message				: name WS? EQ WS? ;
chat                : line line EOF ;
line                : name SAYS opinion NEWLINE;
name                : WORD ;
opinion             : TEXT ;

/*
 * Lexer Rules
 */

fragment A          : ('A'|'a') ;
fragment S          : ('S'|'s') ;
fragment Y          : ('Y'|'y') ;
fragment LOWERCASE  : [a-z] ;
fragment UPPERCASE  : [A-Z] ;

WS					: (' '|'\t')+ -> skip ;
EQ					: '=' ;
SAYS                : S A Y S ;
WORD                : (LOWERCASE | UPPERCASE)+ ;
TEXT                : '"' .*? '"' ;
WHITESPACE          : (' '|'\t')+ -> skip ;
NEWLINE             : ('\r'? '\n' | '\r')+ ;