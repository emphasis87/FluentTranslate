grammar Fluent;

/*
 * Parser Rules
 */

message_list		: message ( NEWLINE message )* NEWLINE? EOF ;
message				: name WS? EQ WS? ;
comment_l3			: WS? COMMNET_L3 WS? COMMENT (NEWLINE | EOF) ;
comment_l2			: WS? COMMNET_L2 WS? COMMENT (NEWLINE | EOF) ;
comment_l1			: WS? COMMNET_L1 WS? COMMENT (NEWLINE | EOF) ;
name                : WORD ;
opinion             : TEXT ;

/*
 * Lexer Rules
 */

fragment LOWERCASE  : [a-z] ;
fragment UPPERCASE  : [A-Z] ;
fragment SHARP		: '#' ;
fragment ANY		: .*? ;

COMMNET_L3			: SHARP SHARP SHARP ;
COMMENT_L2			: SHARP SHARP ;
COMMENT_L1			: SHARP ;
COMMENT				: ANY ;
WS					: (' '|'\t')+ -> skip ;
EQ					: '=' ;
WORD                : (LOWERCASE | UPPERCASE)+ ;
TEXT                : '"' .*? '"' ;
WHITESPACE          : (' '|'\t')+ -> skip ;
NEWLINE             : ('\r'? '\n' | '\r')+ ;