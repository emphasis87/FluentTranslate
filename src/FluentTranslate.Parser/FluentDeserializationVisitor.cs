using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Antlr4.Runtime.Tree;
using FluentTranslate.Common.Domain;

namespace FluentTranslate.Parser
{
	public class FluentDeserializationVisitor : FluentParserBaseVisitor<List<IFluentElement>>
	{
		public override List<IFluentElement> Visit(IParseTree tree)
		{
			return base.Visit(tree);
		}

		protected override List<IFluentElement> DefaultResult => new List<IFluentElement>();

		protected override List<IFluentElement> AggregateResult(List<IFluentElement> aggregate,
			List<IFluentElement> nextResult)
		{
			aggregate.AddRange(nextResult);
			return aggregate;
		}

		public override List<IFluentElement> VisitResource(FluentParser.ResourceContext context)
		{
			var resource = new FluentResource();
			var result = base.VisitResource(context);
			foreach (var child in result)
			{
				switch (child)
				{
					case IFluentEntry entry:
						resource.Entries.Add(entry);
						break;
					default:
						throw UnsupportedChildTypeException(child);
				}
			}

			result.Clear();
			result.Add(resource);
			return result;
		}

		public override List<IFluentElement> VisitComment(FluentParser.CommentContext context)
		{
			var result = DefaultResult;
			var comment = new FluentComment()
			{
				Level = context.COMMENT_OPEN().GetText().Length,
				Value = context.COMMENT().GetText()
			};
			result.Add(comment);
			return result;
		}

		public override List<IFluentElement> VisitEntry(FluentParser.EntryContext context)
		{
			return base.VisitEntry(context);
		}

		public override List<IFluentElement> VisitTerm(FluentParser.TermContext context)
		{
			var result = base.VisitTerm(context);
			var record = (FluentRecord) result[0];
			result.RemoveAt(0);
			return AggregateRecord(record, result);
		}

		public override List<IFluentElement> VisitMessage(FluentParser.MessageContext context)
		{
			var result = base.VisitMessage(context);
			var record = (FluentRecord) result[0];
			result.RemoveAt(0);
			return AggregateRecord(record, result);
		}

		private static List<IFluentElement> AggregateRecord(FluentRecord record, List<IFluentElement> result)
		{
			foreach (var child in result)
			{
				switch (child)
				{
					case IFluentContent content:
						record.Content.Add(content);
						break;
					case FluentAttribute attribute:
						record.Attributes.Add(attribute);
						break;
				}
			}

			result.Clear();
			result.Add(record);
			return result;
		}

		public override List<IFluentElement> VisitRecord(FluentParser.RecordContext context)
		{
			var result = DefaultResult;
			var record = context.Parent is FluentParser.TermContext
				? (FluentRecord) new FluentTerm()
				: new FluentMessage();
			record.Id = context.IDENTIFIER().GetText();
			result.Add(record);
			return result;
		}

		public override List<IFluentElement> VisitText(FluentParser.TextContext context)
		{
			var result = DefaultResult;
			var text = new FluentText()
			{
				Value = context.GetText()
			};
			result.Add(text);
			return result;
		}

		public override List<IFluentElement> VisitPlaceable(FluentParser.PlaceableContext context)
		{
			var result = base.VisitPlaceable(context);
			if (result.Count == 0)
				return result;

			// Remove indentation text from inner placeable, e.g
			// ... { indentation { $variable } } ...
			result.RemoveAll(x => x is FluentText);

			var child = result[0];
			switch (child)
			{
				case FluentPlaceable _:
				{
					// Inner placeable, e.g.
					// ... { { $variable } } ...
					break;
				}
				case IFluentExpression content:
				{
					var placeable = new FluentPlaceable()
					{
						Content = content,
					};
					result[0] = placeable;
					break;
				}
				default:
					throw UnsupportedChildTypeException(child);
			}

			// Add indentation prefix as a part of the expression, e.g.
			// ... { $variable } indentation { "literal" } ...
			var prefix = context.Prefix.GetText();
			if (!string.IsNullOrEmpty(prefix))
			{
				var text = new FluentText()
				{
					Value = prefix
				};
				result.Insert(0, text);
			}

			return result;
		}

		public override List<IFluentElement> VisitSelectExpression(FluentParser.SelectExpressionContext context)
		{
			var result = base.VisitSelectExpression(context);
			var selection = new FluentSelection();
			foreach (var child in result)
			{
				switch (child)
				{
					case IFluentExpression content:
						selection.Match = content;
						break;
					case FluentVariant variant:
						selection.Variants.Add(variant);
						break;
					default:
						throw UnsupportedChildTypeException(child);
				}
			}

			result.Clear();
			result.Add(selection);
			return result;
		}

		public override List<IFluentElement> VisitDefaultVariant(FluentParser.DefaultVariantContext context)
		{
			var result = base.VisitDefaultVariant(context);
			var child = result.SingleOrDefault();
			switch (child)
			{
				case FluentVariant variant:
					variant.IsDefault = true;
					break;
			}

			return result;
		}

		public override List<IFluentElement> VisitVariant(FluentParser.VariantContext context)
		{
			var result = base.VisitVariant(context);

			var variant = new FluentVariant();
			if (context.IDENTIFIER_REF() != null)
			{
				variant.Key = new FluentIdentifier()
				{
					Id = context.IDENTIFIER_REF().GetText()
				};
			}

			if (context.NUMBER_LITERAL() != null)
			{
				variant.Key = new FluentNumberLiteral()
				{
					Value = context.NUMBER_LITERAL().GetText()
				};
			}

			foreach (var child in result)
			{
				switch (child)
				{
					case IFluentContent content:
						variant.Content.Add(content);
						break;
				}
			}

			result.Clear();
			result.Add(variant);
			return result;
		}

		public override List<IFluentElement> VisitStringLiteral(FluentParser.StringLiteralContext context)
		{
			var result = DefaultResult;
			var stringLiteral = new FluentStringLiteral()
			{
				Value = context.GetText()
			};
			result.Add(stringLiteral);
			return result;
		}

		public override List<IFluentElement> VisitNumberLiteral(FluentParser.NumberLiteralContext context)
		{
			var result = DefaultResult;
			var numberLiteral = new FluentNumberLiteral()
			{
				Value = context.GetText()
			};
			result.Add(numberLiteral);
			return result;
		}

		public override List<IFluentElement> VisitVariableReference(FluentParser.VariableReferenceContext context)
		{
			var result = DefaultResult;
			var variableReference = new FluentVariableReference()
			{
				Id = context.IDENTIFIER_REF().GetText()
			};
			result.Add(variableReference);
			return result;
		}

		public override List<IFluentElement> VisitTermReference(FluentParser.TermReferenceContext context)
		{
			var result = base.VisitTermReference(context);
			var reference = (FluentTermReference)result[0];
			
			var call = result.Skip(1).SingleOrDefault();
			if (call is IFluentCallable callable)
			{
				foreach (var argument in callable.Arguments)
				{
					reference.Arguments.Add(argument);
				}
			}

			return result;
		}

		public override List<IFluentElement> VisitRecordReference(FluentParser.RecordReferenceContext context)
		{
			var result = DefaultResult;
			var attributeAccessor = context.attributeAccessor();
			var reference = context.Parent is FluentParser.TermReferenceContext
				? (FluentRecordReference) new FluentTermReference()
				: new FluentMessageReference();
			reference.Id = context.IDENTIFIER_REF().GetText();
			reference.AttributeId = attributeAccessor?.IDENTIFIER_REF().GetText();
			result.Add(reference);
			return result;
		}

		public override List<IFluentElement> VisitFunctionCall(FluentParser.FunctionCallContext context)
		{
			var result = base.VisitFunctionCall(context);
			var child = result[0];
			switch (child)
			{
				case FluentFunctionCall functionCall:
					functionCall.Id = context.IDENTIFIER_REF().GetText();
					break;
				default:
					throw UnsupportedChildTypeException(child);
			}
			return result;
		}

		public override List<IFluentElement> VisitArgumentList(FluentParser.ArgumentListContext context)
		{
			var result = base.VisitArgumentList(context);
			var functionCall = new FluentFunctionCall();
			foreach (var child in result)
			{
				switch (child)
				{
					case FluentCallArgument argument:
						functionCall.Arguments.Add(argument);
						break;
					default:
						throw UnsupportedChildTypeException(child);
				}
			}

			result.Clear();
			result.Add(functionCall);
			return result;
		}

		public override List<IFluentElement> VisitArgumentExpression(FluentParser.ArgumentExpressionContext context)
		{
			var result = base.VisitArgumentExpression(context);
			var child = result[0];
			switch (child)
			{
				case IFluentExpression content:
				{
					var argument = new FluentCallArgument()
					{
						Value = content
					};
					result[0] = argument;
					break;
				}
				default:
					throw UnsupportedChildTypeException(child);
			}

			return result;
		}

		public override List<IFluentElement> VisitNamedArgument(FluentParser.NamedArgumentContext context)
		{
			var result = base.VisitNamedArgument(context);
			var child = result[0];
			switch (child)
			{
				case FluentCallArgument argument:
					argument.Id = context.IDENTIFIER_REF().GetText();
					break;
				default:
					throw UnsupportedChildTypeException(child);
			}

			return result;
		}

		private static Exception UnsupportedChildTypeException(IFluentElement child, [CallerMemberName] string callerName = null)
		{
			return new ArgumentOutOfRangeException(nameof(child), $"{callerName}: Child of type {child.GetType()} is not supported.");
		}
	}
}
