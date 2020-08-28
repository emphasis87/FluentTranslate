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

term				: TERM expressionList attributeList ;
message				: MESSAGE expressionList attributeList ;

expressionList		: expression+ ;
expression			: text | placeable ;

text				: TEXT NL? ;

placeable			: PLACEABLE_OPEN placeableExpression PLACEABLE_CLOSE NL? ;
placeableExpression	: selectExpression | inlineExpression ;

selectExpression	: inlineExpression SELECTOR variantList ;

variantList			: variant* defaultVariant variant* ;
variant				: VARIANT_KEY expressionList ;
defaultVariant		: VARIANT_DEFAULT_KEY expressionList ;

inlineExpression	: stringLiteral
					| numberLiteral
					| variableReference
					| termReference					
					| messageReference
					| functionCall
					| placeable
					;

stringLiteral		: STRING_OPEN ( ESCAPED_CHAR | UNICODE_ESCAPE | QUOTED_STRING )* STRING_CLOSE ;
numberLiteral		: NUMBER_LITERAL ;

variableReference	: VARIABLE_REF ;
termReference		: TERM_REF ATTRIBUTE_REF?;
messageReference	: MESSAGE_REF ATTRIBUTE_REF? ;

functionCall		: FUNCTION_CALL argumentList CALL_CLOSE ;

argumentList		: (argument CALL_ARG_SEP )* argument? ;
argument			: namedArgument | argumentExpression ;
namedArgument		: CALL_NAMED_ARG argumentExpression ;
argumentExpression	: inlineExpression ;

attributeList		: attribute* ;
attribute			: ATTRIBUTE expressionList ;

multilineIndent		: (INDENT? NL)* INDENT? ;
