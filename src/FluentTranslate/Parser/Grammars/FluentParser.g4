parser grammar FluentParser;

options { 
	tokenVocab = FluentLexer;
	contextSuperClass = FluentTranslate.Parser.FluentParserContext; 
}

/*
 * Parser Rules
 */

document			: entry* EOF ;

entry				: ( record | comment | emptyLine ) ;

comment				: COMMENT_OPEN COMMENT NL? ;

record				: term | message ;
recordHeader		: IDENTIFIER INDENT? EQUALS INDENT? ;
term				: TERM recordHeader expressionList attributeList? NL? ;
message				: recordHeader expressionList? attributeList? NL? ;

attributeList		: attribute+ ;
attribute			: ws indent ATTRIBUTE record expressionList ;

expressionList		: expression+ ;
expression			: text | placeable ;

text				: ws TEXT ;

indent				: ( INDENT | NL_INDENT ) ;
ws					: ( INDENT | NL | NL_INDENT )*? ;

placeable			: Prefix=ws PLACEABLE_OPEN ws placeableExpression ws PLACEABLE_CLOSE ;
placeableExpression	: selectExpression | inlineExpression ;

selectExpression	: inlineExpression ws SELECTOR ws variantList ;

variantList			: (ws variant)* ws defaultVariant (ws variant)* ;
defaultVariant		: VARIANT_DEFAULT variant ;
variant				: VARIANT_OPEN INDENT? ( identifier | NUMBER_LITERAL ) INDENT? VARIANT_CLOSE expressionList ;
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
argument			: ws ( namedArgument | inlineArgument ) ;
namedArgument		: IDENTIFIER_REF ws CALL_ARG_NAME_SEP ws argumentExpression ;
inlineArgument		: argumentExpression ;
argumentExpression	: inlineExpression ;

emptyLine			: INDENT? ( NL | NL_INDENT ) ;
