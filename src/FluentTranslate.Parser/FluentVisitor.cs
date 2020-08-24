//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.8
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from D:\projects\FluentTranslate\src\FluentTranslate.Parser\Fluent.g4 by ANTLR 4.8

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="FluentParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.8")]
[System.CLSCompliant(false)]
public interface IFluentVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.entry"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitEntry([NotNull] FluentParser.EntryContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.message"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMessage([NotNull] FluentParser.MessageContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.term"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTerm([NotNull] FluentParser.TermContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.expressionList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpressionList([NotNull] FluentParser.ExpressionListContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitExpression([NotNull] FluentParser.ExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.attribute"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAttribute([NotNull] FluentParser.AttributeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.textInline"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTextInline([NotNull] FluentParser.TextInlineContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.textBlock"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTextBlock([NotNull] FluentParser.TextBlockContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.placeableInline"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPlaceableInline([NotNull] FluentParser.PlaceableInlineContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.placeableBlock"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPlaceableBlock([NotNull] FluentParser.PlaceableBlockContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.selectExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSelectExpression([NotNull] FluentParser.SelectExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.inlineExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitInlineExpression([NotNull] FluentParser.InlineExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.functionReference"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFunctionReference([NotNull] FluentParser.FunctionReferenceContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.messageReference"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMessageReference([NotNull] FluentParser.MessageReferenceContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.termReference"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTermReference([NotNull] FluentParser.TermReferenceContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.variableReference"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVariableReference([NotNull] FluentParser.VariableReferenceContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.attributeAccessor"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAttributeAccessor([NotNull] FluentParser.AttributeAccessorContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.callArguments"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCallArguments([NotNull] FluentParser.CallArgumentsContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.argumentList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitArgumentList([NotNull] FluentParser.ArgumentListContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.argument"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitArgument([NotNull] FluentParser.ArgumentContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.namedArgument"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNamedArgument([NotNull] FluentParser.NamedArgumentContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.variantList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVariantList([NotNull] FluentParser.VariantListContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.defaultVariant"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDefaultVariant([NotNull] FluentParser.DefaultVariantContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.variant"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVariant([NotNull] FluentParser.VariantContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.variantKey"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVariantKey([NotNull] FluentParser.VariantKeyContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.stringLiteral"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStringLiteral([NotNull] FluentParser.StringLiteralContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.numberLiteral"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNumberLiteral([NotNull] FluentParser.NumberLiteralContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.comment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitComment([NotNull] FluentParser.CommentContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.comment3"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitComment3([NotNull] FluentParser.Comment3Context context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.comment2"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitComment2([NotNull] FluentParser.Comment2Context context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.comment1"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitComment1([NotNull] FluentParser.Comment1Context context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.commentLine3"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCommentLine3([NotNull] FluentParser.CommentLine3Context context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.commentLine2"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCommentLine2([NotNull] FluentParser.CommentLine2Context context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.commentLine1"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCommentLine1([NotNull] FluentParser.CommentLine1Context context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.emptyLine"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitEmptyLine([NotNull] FluentParser.EmptyLineContext context);
}
