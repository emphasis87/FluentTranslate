parser grammar FluentParser;
options { 
	tokenVocab = FluentLexer;
	contextSuperClass = FluentTranslate.Parser.FluentContext; 
}

/*
 * Parser Rules
 */

resource			: entry+ EOF ;

entry				: ( message | comment | emptyLine ) ;

message				: IDENTIFIER INDENT? EQUALS ( expressionList );

emptyLine			: INDENT? NL ;

expressionList		: expression+ ;
expression			: textInline
//					| textBlock
					| placeable
					;

textBlock			: textInline ;
textInline			: TEXT NL? ;

comment				: COMMENT_OPEN COMMENT_TEXT? NL? ;

placeable			: PLACEABLE_OPEN INDENT? placeableExpression INDENT? PLACEABLE_CLOSE NL? ;

placeableExpression	: inlineExpression ;

inlineExpression	: stringLiteral
					| numberLiteral
//					| functionReference
//					| messageReference
//					| termReference
					| variableReference
//					| placeableInline
					;

stringLiteral		: STRING_OPEN ( ESCAPED_CHAR | UNICODE_ESCAPE | QUOTED_STRING )* STRING_CLOSE ;
numberLiteral		: NUMBER_LITERAL ;
variableReference	: VARIABLE_REFERENCE ;

/*
entry				: ( message | term | comment | emptyLine ) ;

message				: comment1? IDENTIFIER EQUALS ( expressionList attribute* | attribute+ );
term				: comment1? '-' IDENTIFIER EQUALS expressionList attribute*;

expressionList		: expression+ ;
expression			: textInline
					| textBlock
					| placeableInline
					| placeableBlock
					;

attribute			: LINE_END WS? '.' IDENTIFIER EQUALS expressionList ;

textInline			: INLINE_CHAR+ ;
textBlock			: WS INDENT INDENTED_CHAR textInline ;

placeableInline		: '{' INDENT? (selectExpression | inlineExpression) INDENT? '}' ;
placeableBlock		: WS placeableInline ;

selectExpression	: inlineExpression WS? '->' INDENT? variantList ;

inlineExpression	: stringLiteral
					| numberLiteral
					| functionReference
					| messageReference
					| termReference
					| variableReference
					| placeableInline
					;

functionReference	: IDENTIFIER callArguments ;
messageReference	: IDENTIFIER attributeAccessor? ;
termReference		: '-' IDENTIFIER attributeAccessor? callArguments? ;
variableReference	: '$' IDENTIFIER ;
attributeAccessor	: '.' IDENTIFIER ;

callArguments		: WS? '(' WS? argumentList WS? ')' ;
argumentList		: ( argument WS? ',' WS? )* argument? ;
argument			: namedArgument | inlineExpression ;
namedArgument		: IDENTIFIER WS? ':' WS? (STRING_LITERAL | NUMBER_LITERAL) ;

variantList			: variant* defaultVariant variant* LINE_END ;
defaultVariant		: '*' variant ;
variant				: LINE_END WS? variantKey INDENT? expressionList ;
variantKey			: '[' INDENT? ( NUMBER_LITERAL | IDENTIFIER ) ']' ;

stringLiteral		: STRING_LITERAL ;
numberLiteral		: NUMBER_LITERAL ;

comment				: comment3 | comment2 | comment1 ;

comment3			: commentLine3+ ;
comment2			: commentLine2+ ;
comment1			: commentLine1+ ;

commentLine3		: CMT3 ( ' ' COMMENT )? LINE_END ;
commentLine2		: CMT2 ( ' ' COMMENT )? LINE_END ;
commentLine1		: CMT1 ( ' ' COMMENT )? LINE_END ;

emptyLine			: (' ' | '\t')* LINE_END ;

// Lexer Rules

INLINE_CHAR			: ~( '{' | '}' | '\r' | '\n' ) ;
INDENTED_CHAR		: ~( '{' | '}' | '[' | '*' | '.' | '\r' | '\n' ) ;

SPECIAL_TEXT_CHAR	: '{' | '}' ;
QUOTED_CHAR			: '\\' SPECIAL_CHAR ;
SPECIAL_CHAR		: '"' | '\\' ;

STRING_LITERAL		: '"' QUOTED_CHAR* '"' ;
NUMBER_LITERAL		: '-'? [0-9]+ ('.' [0-9]+)? ;

IDENTIFIER			: [a-zA-Z] ([a-zA-Z0-9] | '_' | '-')* ;

EQUALS				: INDENT? '=' INDENT? ;

WS					: (INDENT? | LINE_END)+ ;
INDENT				: ' '+ ;

CMT3				: '###' ;
CMT2				: '##' ;
CMT1				: '#' ;

LINE_END			: NEWLINE | EOF ;
NEWLINE				: ('\r'? '\n' | '\r') ;
*/