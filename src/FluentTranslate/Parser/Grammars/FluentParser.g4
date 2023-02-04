parser grammar FluentParser;

options { 
	tokenVocab = FluentLexer;
	contextSuperClass = FluentTranslate.Parser.FluentParserContext; 
}

/*
 * Parser Rules
 */

document			: Content=entryList EOF ;

entryList			: entry* ;
entry				: ( term | message | comment | emptyLine ) ;

comment				: COMMENT_OPEN COMMENT NL? ;

record				: IDENTIFIER INDENT? EQUALS INDENT? ;

term				: TERM record Content=expressionList Attributes=attributeList? NL? ;
message				: record Content=expressionList? Attributes=attributeList? NL? ;

attributeList		: attribute+ ;
attribute			: ws indent ATTRIBUTE record Content=expressionList ;

expressionList		: ws? ( blank? expression )+ ws? ;
expression			: text | placeable ;

text				: TEXT ;

indent				: ( INDENT | NL_INDENT ) ;
blank				: ( INDENT | NL | NL_INDENT )+ ;
ws					: ( INDENT | NL | NL_INDENT )*? ;

placeable			: PLACEABLE_OPEN ws Content=placeableExpression ws PLACEABLE_CLOSE ;
placeableExpression	: selectExpression | inlineExpression ;

selectExpression	: Match=inlineExpression ws SELECTOR ws Variants=variantList ;

variantList			: (ws variant)* ws defaultVariant (ws variant)* ;
defaultVariant		: VARIANT_DEFAULT variant ;
variant				: VARIANT_OPEN INDENT? Key=variantKey INDENT? VARIANT_CLOSE Content=expressionList ;
variantKey			: ( identifier | numberLiteral ) ;
identifier			: IDENTIFIER_REF ;

inlineExpression	: stringLiteral
					| numberLiteral
					| functionCall
					| variableReference
					| termReference		
					| messageReference
					| placeable
					;

stringLiteral		: STRING_OPEN ( ESCAPED_CHAR | UNICODE_ESCAPE | QUOTED_STRING )* STRING_CLOSE ;
numberLiteral		: NUMBER_LITERAL ;

variableReference	: VARIABLE_REF IDENTIFIER_REF ;
termReference		: TERM_REF recordReference argumentList? ;
messageReference	: recordReference ;
recordReference		: IDENTIFIER_REF attributeAccessor? ;
attributeAccessor	: ATTRIBUTE_REF IDENTIFIER_REF ;

functionCall		: IDENTIFIER_REF argumentList ;

argumentList		: ws CALL_OPEN ( argument ws CALL_ARG_SEP )* argument? ws CALL_CLOSE ;
argument			: ws argumentName? Argument=inlineExpression ;
argumentName		: IDENTIFIER_REF ws CALL_ARG_NAME_SEP ws ;

emptyLine			: INDENT? ( NL | NL_INDENT ) ;
