using FluentTranslate.Common;
using FluentTranslate.Domain;
using FluentTranslate.Domain.Common;
using FluentTranslate.Parser;
using FluentTranslate.Services;

namespace FluentTranslate.Infrastructure
{
	public interface IFluentEngine
	{
		string Evaluate(string query, IDictionary<string, object> parameters = null);
		string Evaluate(IFluentElement element, IDictionary<string, object> parameters = null);
	}

	public class FluentEngine : IFluentEngine, IEquatable<FluentEngine>
	{
		protected IFluentConfiguration Configuration { get; }

		protected IFluentCloneFactory Factory =>
			Configuration?.Services.GetService<IFluentCloneFactory>() ?? FluentCloneFactory.Default;

		protected FluentResource Resource { get; }
		protected Dictionary<string, FluentRecord> RecordByReference { get; }

		public FluentEngine(FluentResource resource, IFluentConfiguration configuration = null)
		{
			Configuration = configuration;

			Resource = resource is null ? new FluentResource() : Factory.Clone(resource);
			RecordByReference = new Dictionary<string, FluentRecord>();

			Initialize();
		}

		protected void Initialize()
		{
			foreach (var record in Resource.Entries.OfType<FluentRecord>())
			{
				RecordByReference[record.Reference] = record;
			}
		}

		public string Evaluate(string query, IDictionary<string, object> parameters = null)
		{
			if (query is null)
				return null;

			List<IFluentElement> elements;
			try
			{
				elements = FluentConverter.Deserialize(query, (lexer, parser) =>
				{
					lexer.PushMode(FluentLexer.SINGLELINE);
					return parser.expressionList();
				});
			}
			catch (Exception ex)
			{
				throw new ArgumentException($"Unable to deserialize query: '{query}'.", ex);
			}

			var result = Evaluate(elements, parameters);
			return result;
		}

		public string Evaluate(IFluentElement element, IDictionary<string, object> parameters = null)
		{
			var context = new FluentEvaluationContext(parameters);
			var result = Evaluate(element, context);
			return result.ToString();
		}

		private string Evaluate(IEnumerable<IFluentElement> elements, IDictionary<string, object> parameters = null)
		{
			var context = new FluentEvaluationContext(parameters);
			var results = elements
				.Select(x => Evaluate(x, context))
				.Select(x => x.ToString())
				.ToArray();
			return string.Concat(results);
		}

		public object Evaluate(IFluentElement element, FluentEvaluationContext context)
		{
			context ??= new FluentEvaluationContext();
			return element switch
			{
				FluentText text => Evaluate(text, context),
				FluentPlaceable placeable => Evaluate(placeable, context),
				//FluentSelection selection => Evaluate(selection, context),
				//FluentVariableReference variableReference => Evaluate(variableReference, context),
				FluentMessageReference messageReference => Evaluate(messageReference, context),
				FluentTermReference termReference => Evaluate(termReference, context),
				//FluentFunctionCall functionCall => Evaluate(functionCall, context),
				FluentStringLiteral stringLiteral => Evaluate(stringLiteral, context),
				FluentNumberLiteral numberLiteral => Evaluate(numberLiteral, context),
				_ => throw new ArgumentOutOfRangeException(nameof(element), element, "Element is not supported for string evaluation.")
			};
		}

		public virtual object Evaluate(FluentPlaceable placeable, FluentEvaluationContext context)
		{
			return Evaluate(placeable.Content, context);
		}

		public virtual string Evaluate(FluentMessageReference messageReference, FluentEvaluationContext context)
		{
			return Evaluate((FluentRecordReference) messageReference, context);
		}

		public virtual string Evaluate(FluentTermReference termReference, FluentEvaluationContext context)
		{
			return Evaluate((FluentRecordReference)termReference, context);
		}

		protected virtual string Evaluate(FluentRecordReference reference, FluentEvaluationContext context)
		{
			if (RecordByReference.TryGetValue(reference.TargetReference, out var record))
			{
				var values = record.Content
					.Select(x => Evaluate(x, context))
					.ToArray();

				var result = Serialize(values);
				return result;
			}

			return $"{{{reference.TargetReference}}}";
		}

		public virtual string Evaluate(FluentText text, FluentEvaluationContext context)
		{
			return text.Value;
		}

		public virtual string Evaluate(FluentStringLiteral literal, FluentEvaluationContext context)
		{
			return literal.Value;
		}

		public virtual object Evaluate(FluentNumberLiteral literal, FluentEvaluationContext context)
		{
			if (int.TryParse(literal.Value, out var i32))
				return i32;
			if (long.TryParse(literal.Value, out var i64))
				return i64;
			if (float.TryParse(literal.Value, out var f32))
				return f32;
			if (double.TryParse(literal.Value, out var f64))
				return f64;
			if (decimal.TryParse(literal.Value, out var d128))
				return d128;

			throw new ArgumentOutOfRangeException(nameof(literal));
		}

		protected virtual string Serialize(object[] values)
		{
			var result = new StringBuilder();
			foreach (var value in values)
			{
				var serialized = Serialize(value);
				result.Append(serialized);
			}

			return result.ToString();
		}

		protected virtual string Serialize(object value)
		{
			return $"{value}";
		}

		public bool Equals(FluentEngine other)
		{
			if (other is null) return false;
			if (ReferenceEquals(this, other)) return true;
			return FluentEqualityComparer.Default.Equals(Resource, other.Resource);
		}

		public override bool Equals(object obj)
		{
			if (obj is null) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((FluentEngine) obj);
		}

		public override int GetHashCode()
		{
			return Helpers.Hash(Resource);
		}
	}
}