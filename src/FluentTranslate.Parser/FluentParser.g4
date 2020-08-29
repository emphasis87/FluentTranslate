parser grammar FluentParser;

options { 
	tokenVocab = FluentLexer;
	contextSuperClass = FluentTranslate.Parser.FluentContext; 
}

/*
 * Parser Rules
 */

resource			: entry+ EOF ;

entry				: ( term | message | comment | emptyLine ) ;

comment				: COMMENT ;
emptyLine			: INDENT? NL ;

term				: TERM record attributeList ;
message				: record attributeList ;
attributeList		: attribute* ;
attribute			: ATTRIBUTE record ;

record				: IDENTIFIER INDENT? EQUALS ws expressionList ;

expressionList		: expression+ ;
expression			: text | placeable ;

ws					: (NL | NL_INDENT | INDENT)* ;
text				: ws TEXT ws ;

placeable			: PLACEABLE_OPEN ws placeableExpression ws PLACEABLE_CLOSE  ;
placeableExpression	: selectExpression | inlineExpression ;

selectExpression	: inlineExpression ws SELECTOR ws variantList ;

variantList			: variant* defaultVariant variant* ;
defaultVariant		: VARIANT_DEFAULT variant ;
variant				: VARIANT_OPEN (IDENTIFIER_REF | NUMBER_LITERAL) VARIANT_CLOSE expressionList ;


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

functionCall		: IDENTIFIER_REF ws argumentList ;

argumentList		: ws CALL_OPEN ( argument CALL_ARG_SEP )* argument? CALL_CLOSE ;
argument			: ws (namedArgument | argumentExpression) ws ;
namedArgument		: IDENTIFIER_REF ws CALL_ARG_NAME_SEP ws argumentExpression ;
argumentExpression	: inlineExpression ;
