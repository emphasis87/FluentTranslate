//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.8
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from d:\projects\FluentTranslate\src\FluentTranslate.Parser\FluentParser.g4 by ANTLR 4.8

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete listener for a parse tree produced by
/// <see cref="FluentParser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.8")]
[System.CLSCompliant(false)]
public interface IFluentParserListener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="FluentParser.resource"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterResource([NotNull] FluentParser.ResourceContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="FluentParser.resource"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitResource([NotNull] FluentParser.ResourceContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="FluentParser.entry"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterEntry([NotNull] FluentParser.EntryContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="FluentParser.entry"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitEntry([NotNull] FluentParser.EntryContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="FluentParser.comment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterComment([NotNull] FluentParser.CommentContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="FluentParser.comment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitComment([NotNull] FluentParser.CommentContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="FluentParser.emptyLine"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterEmptyLine([NotNull] FluentParser.EmptyLineContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="FluentParser.emptyLine"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitEmptyLine([NotNull] FluentParser.EmptyLineContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="FluentParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTerm([NotNull] FluentParser.TermContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="FluentParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTerm([NotNull] FluentParser.TermContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="FluentParser.message"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMessage([NotNull] FluentParser.MessageContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="FluentParser.message"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMessage([NotNull] FluentParser.MessageContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="FluentParser.expressionList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterExpressionList([NotNull] FluentParser.ExpressionListContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="FluentParser.expressionList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitExpressionList([NotNull] FluentParser.ExpressionListContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="FluentParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterExpression([NotNull] FluentParser.ExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="FluentParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitExpression([NotNull] FluentParser.ExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="FluentParser.text"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterText([NotNull] FluentParser.TextContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="FluentParser.text"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitText([NotNull] FluentParser.TextContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="FluentParser.placeable"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterPlaceable([NotNull] FluentParser.PlaceableContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="FluentParser.placeable"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitPlaceable([NotNull] FluentParser.PlaceableContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="FluentParser.placeableExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterPlaceableExpression([NotNull] FluentParser.PlaceableExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="FluentParser.placeableExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitPlaceableExpression([NotNull] FluentParser.PlaceableExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="FluentParser.selectExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterSelectExpression([NotNull] FluentParser.SelectExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="FluentParser.selectExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitSelectExpression([NotNull] FluentParser.SelectExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="FluentParser.variantList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterVariantList([NotNull] FluentParser.VariantListContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="FluentParser.variantList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitVariantList([NotNull] FluentParser.VariantListContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="FluentParser.variant"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterVariant([NotNull] FluentParser.VariantContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="FluentParser.variant"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitVariant([NotNull] FluentParser.VariantContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="FluentParser.defaultVariant"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDefaultVariant([NotNull] FluentParser.DefaultVariantContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="FluentParser.defaultVariant"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDefaultVariant([NotNull] FluentParser.DefaultVariantContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="FluentParser.inlineExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterInlineExpression([NotNull] FluentParser.InlineExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="FluentParser.inlineExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitInlineExpression([NotNull] FluentParser.InlineExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="FluentParser.stringLiteral"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStringLiteral([NotNull] FluentParser.StringLiteralContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="FluentParser.stringLiteral"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStringLiteral([NotNull] FluentParser.StringLiteralContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="FluentParser.numberLiteral"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterNumberLiteral([NotNull] FluentParser.NumberLiteralContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="FluentParser.numberLiteral"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitNumberLiteral([NotNull] FluentParser.NumberLiteralContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="FluentParser.variableReference"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterVariableReference([NotNull] FluentParser.VariableReferenceContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="FluentParser.variableReference"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitVariableReference([NotNull] FluentParser.VariableReferenceContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="FluentParser.termReference"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTermReference([NotNull] FluentParser.TermReferenceContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="FluentParser.termReference"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTermReference([NotNull] FluentParser.TermReferenceContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="FluentParser.messageReference"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMessageReference([NotNull] FluentParser.MessageReferenceContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="FluentParser.messageReference"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMessageReference([NotNull] FluentParser.MessageReferenceContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="FluentParser.functionCall"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFunctionCall([NotNull] FluentParser.FunctionCallContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="FluentParser.functionCall"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFunctionCall([NotNull] FluentParser.FunctionCallContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="FluentParser.argumentList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterArgumentList([NotNull] FluentParser.ArgumentListContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="FluentParser.argumentList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitArgumentList([NotNull] FluentParser.ArgumentListContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="FluentParser.argument"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterArgument([NotNull] FluentParser.ArgumentContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="FluentParser.argument"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitArgument([NotNull] FluentParser.ArgumentContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="FluentParser.namedArgument"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterNamedArgument([NotNull] FluentParser.NamedArgumentContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="FluentParser.namedArgument"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitNamedArgument([NotNull] FluentParser.NamedArgumentContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="FluentParser.argumentExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterArgumentExpression([NotNull] FluentParser.ArgumentExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="FluentParser.argumentExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitArgumentExpression([NotNull] FluentParser.ArgumentExpressionContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="FluentParser.attributeList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAttributeList([NotNull] FluentParser.AttributeListContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="FluentParser.attributeList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAttributeList([NotNull] FluentParser.AttributeListContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="FluentParser.attribute"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAttribute([NotNull] FluentParser.AttributeContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="FluentParser.attribute"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAttribute([NotNull] FluentParser.AttributeContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="FluentParser.multilineIndent"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMultilineIndent([NotNull] FluentParser.MultilineIndentContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="FluentParser.multilineIndent"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMultilineIndent([NotNull] FluentParser.MultilineIndentContext context);
}
