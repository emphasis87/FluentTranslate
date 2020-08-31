using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Antlr4.Runtime;
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
			if (result.Count == 0)
			{
				result.Add(resource);
				return result;
			}

			for (var i = 0; i < result.Count - 1; i++)
			{
				switch (result[i], result[i+1])
				{
					case (FluentComment left, FluentComment right) when FluentComment.CanAggregate(left, right):
					{
						result[i] = FluentComment.Aggregate(left, right);
						result.RemoveAt(i + 1);
						i--;
						break;
					}
					case (FluentEmptyLines left, FluentEmptyLines right):
					{
						result[i] = FluentEmptyLines.Aggregate(left, right);
						result.RemoveAt(i + 1);
						i--;
						break;
					}
				}
			}

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

		public override List<IFluentElement> VisitEntry(FluentParser.EntryContext context)
		{
			return base.VisitEntry(context);
		}

		public override List<IFluentElement> VisitEmptyLine(FluentParser.EmptyLineContext context)
		{
			var result = DefaultResult;
			var empty = new FluentEmptyLines();
			result.Add(empty);
			return result;
		}

		public override List<IFluentElement> VisitComment(FluentParser.CommentContext context)
		{
			var result = DefaultResult;
			var comment = new FluentComment
			{
				Level = context.COMMENT_OPEN().GetText().TrimEnd().Length,
				Value = context.COMMENT().GetText()
			};
			result.Add(comment);
			return result;
		}

		public override List<IFluentElement> VisitTerm(FluentParser.TermContext context)
		{
			var result = base.VisitTerm(context);
			var record = (FluentRecord) result[0];
			result.RemoveAt(0);
			
			AggregateContainer(record, result);
			AggregateRecord(record, result);

			if (result.Count != 0)
				throw UnsupportedChildTypeException(result[0]);

			result.Add(record);
			return result;
		}

		public override List<IFluentElement> VisitMessage(FluentParser.MessageContext context)
		{
			var result = base.VisitMessage(context);
			var record = (FluentRecord) result[0];
			result.RemoveAt(0);
			
			AggregateContainer(record, result);
			AggregateRecord(record, result);

			if (result.Count != 0)
				throw UnsupportedChildTypeException(result[0]);

			result.Add(record);
			return result;
		}

		public override List<IFluentElement> VisitAttribute(FluentParser.AttributeContext context)
		{
			var result = base.VisitAttribute(context);
			var attribute = (FluentAttribute) result[0];
			result.RemoveAt(0);

			AggregateContainer(attribute, result);

			if (result.Count != 0)
				throw UnsupportedChildTypeException(result[0]);

			result.Add(attribute);
			return result;
		}

		private static void AggregateContainer(IFluentContainer container, IList<IFluentElement> result)
		{
			for (var i = 0; i < result.Count; i++)
			{
				switch (result[i])
				{
					case IFluentContent content:
						container.Content.Add(content);
						result.RemoveAt(i);
						i--;
						break;
				}
			}
		}

		private static void AggregateRecord(FluentRecord record, IList<IFluentElement> result)
		{
			for (var i = 0; i < result.Count; i++)
			{
				switch (result[i])
				{
					case FluentAttribute attribute:
						record.Attributes.Add(attribute);
						result.RemoveAt(i);
						i--;
						break;
				}
			}
		}

		public override List<IFluentElement> VisitRecord(FluentParser.RecordContext context)
		{
			var result = DefaultResult;
			
			var id = context.IDENTIFIER().GetText();
			IFluentContainer container = context.Parent switch
			{
				FluentParser.TermContext _ => new FluentTerm {Id = id},
				FluentParser.MessageContext _ => new FluentMessage {Id = id},
				FluentParser.AttributeContext _ => new FluentAttribute {Id = id},
				_ => throw UnsupportedContextTypeException(context.Parent)
			};

			result.Add(container);
			return result;
		}

		public override List<IFluentElement> VisitText(FluentParser.TextContext context)
		{
			var result = DefaultResult;
			var text = new FluentText {Value = context.GetText()};
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
					var placeable = new FluentPlaceable {Content = content};
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
				var text = new FluentText
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
			var id = context.IDENTIFIER_REF()?.GetText();
			if (id != null) 
				variant.Key = new FluentIdentifier {Id = id};

			var numberLiteral = context.NUMBER_LITERAL()?.GetText();
			if (numberLiteral != null)
				variant.Key = new FluentNumberLiteral {Value = numberLiteral};

			AggregateContainer(variant, result);

			if (result.Count != 0)
				throw UnsupportedChildTypeException(result[0]);

			result.Add(variant);
			return result;
		}

		public override List<IFluentElement> VisitStringLiteral(FluentParser.StringLiteralContext context)
		{
			var result = DefaultResult;
			var stringLiteral = new FluentStringLiteral
			{
				Value = context.GetText()
			};
			result.Add(stringLiteral);
			return result;
		}

		public override List<IFluentElement> VisitNumberLiteral(FluentParser.NumberLiteralContext context)
		{
			var result = DefaultResult;
			var numberLiteral = new FluentNumberLiteral
			{
				Value = context.GetText()
			};
			result.Add(numberLiteral);
			return result;
		}

		public override List<IFluentElement> VisitVariableReference(FluentParser.VariableReferenceContext context)
		{
			var result = DefaultResult;
			var id = context.IDENTIFIER_REF().GetText();
			var variableReference = new FluentVariableReference {Id = id};
			result.Add(variableReference);
			return result;
		}

		public override List<IFluentElement> VisitTermReference(FluentParser.TermReferenceContext context)
		{
			var result = base.VisitTermReference(context);
			var reference = result.OfType<FluentTermReference>().Aggregate(FluentTermReference.Aggregate);
			result.Clear();
			result.Add(reference);
			return result;
		}

		public override List<IFluentElement> VisitRecordReference(FluentParser.RecordReferenceContext context)
		{
			var result = DefaultResult;

			FluentRecordReference reference = context.Parent switch
			{
				FluentParser.TermReferenceContext _ => new FluentTermReference(),
				FluentParser.MessageReferenceContext _ => new FluentMessageReference(),
				_ => throw UnsupportedContextTypeException(context.Parent),
			};

			var id = context.IDENTIFIER_REF().GetText();
			var attributeId = context.attributeAccessor()?.IDENTIFIER_REF().GetText();

			reference.Id = id;
			reference.AttributeId = attributeId;

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
			IFluentCallable call = context.Parent switch
			{
				FluentParser.TermReferenceContext _ => new FluentTermReference(),
				FluentParser.FunctionCallContext _ => new FluentFunctionCall(),
				_ => throw UnsupportedContextTypeException(context.Parent),
			};

			foreach (var child in result)
			{
				switch (child)
				{
					case FluentCallArgument argument:
						call.Arguments.Add(argument);
						break;
					default:
						throw UnsupportedChildTypeException(child);
				}
			}

			result.Clear();
			result.Add(call);
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
					var argument = new FluentCallArgument {Value = content};
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
			var argumentId = context.IDENTIFIER_REF().GetText();
			var child = result[0];
			switch (child)
			{
				case FluentCallArgument argument:
					argument.Id = argumentId;
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

		private static Exception UnsupportedContextTypeException(RuleContext context, [CallerMemberName] string callerName = null)
		{
			return new ArgumentOutOfRangeException(nameof(context), $"{callerName}: Context of type {context.GetType()} is not supported.");
		}
	}
}
