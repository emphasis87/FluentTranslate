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
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="FluentParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.8")]
[System.CLSCompliant(false)]
public interface IFluentParserVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.resource"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitResource([NotNull] FluentParser.ResourceContext context);
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
	/// Visit a parse tree produced by <see cref="FluentParser.emptyLine"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitEmptyLine([NotNull] FluentParser.EmptyLineContext context);
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
	/// Visit a parse tree produced by <see cref="FluentParser.textBlock"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTextBlock([NotNull] FluentParser.TextBlockContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.textInline"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTextInline([NotNull] FluentParser.TextInlineContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.comment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitComment([NotNull] FluentParser.CommentContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.placeable"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPlaceable([NotNull] FluentParser.PlaceableContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.placeableExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPlaceableExpression([NotNull] FluentParser.PlaceableExpressionContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="FluentParser.inlineExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitInlineExpression([NotNull] FluentParser.InlineExpressionContext context);
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
	/// Visit a parse tree produced by <see cref="FluentParser.variableReference"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVariableReference([NotNull] FluentParser.VariableReferenceContext context);
}
