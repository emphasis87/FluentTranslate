using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentTranslate.Domain;

namespace FluentTranslate.Infrastructure
{
	public interface IFluentEngine
	{
		string Evaluate(string message, IDictionary<string, object> parameters);
	}

	public class FluentEngine : IFluentEngine, IEquatable<FluentEngine>
	{
		protected IFluentConfiguration Configuration { get; }

		protected IFluentCloneFactory Factory =>
			Configuration.Services.GetService<IFluentCloneFactory>() ?? FluentCloneFactory.Default;

		protected FluentResource Resource { get; }
		protected Dictionary<string, FluentRecord> RecordByReference { get; }

		public FluentEngine(FluentResource resource, IFluentConfiguration configuration)
		{
			Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

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

		public string Evaluate(string message, IDictionary<string, object> parameters)
		{
			if (!RecordByReference.TryGetValue(message, out var record)) 
				throw new ArgumentException("Unable to resolve message.", nameof(message));

			var context = new FluentEvaluationContext(parameters);
			var result = Evaluate(record, context);
			return result.ToString();
		}

		public object Evaluate(IFluentElement element, FluentEvaluationContext context)
		{
			context ??= new FluentEvaluationContext();
			return element switch
			{
				FluentMessage message => Evaluate(message, context),
				FluentTerm term => Evaluate(term, context),
				FluentText text => Evaluate(text, context),
				//FluentPlaceable placeable => Evaluate(placeable, context),
				//FluentSelection selection => Evaluate(selection, context),
				//FluentVariableReference variableReference => Evaluate(variableReference, context),
				//FluentMessageReference messageReference => Evaluate(messageReference, context),
				//FluentTermReference termReference => Evaluate(termReference, context),
				//FluentFunctionCall functionCall => Evaluate(functionCall, context),
				FluentStringLiteral stringLiteral => Evaluate(stringLiteral, context),
				FluentNumberLiteral numberLiteral => Evaluate(numberLiteral, context),
				_ => throw new ArgumentOutOfRangeException(nameof(element))
			};
		}

		public virtual string Evaluate(FluentMessage message, FluentEvaluationContext context)
		{
			var values = message.Content
				.Select(x => Evaluate(x, context))
				.ToArray();

			var result = Serialize(values);
			return result;
		}

		public virtual string Evaluate(FluentTerm term, FluentEvaluationContext context)
		{
			var values = term.Content
				.Select(x => Evaluate(x, context))
				.ToArray();

			var result = Serialize(values);
			return result;
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
			return (Resource != null ? Resource.GetHashCode() : 0);
		}
	}
}