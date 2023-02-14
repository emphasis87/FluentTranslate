using FluentTranslate.Domain;
using FluentTranslate.Domain.Common;
using FluentTranslate.Parser;
using Microsoft.Extensions.Logging;

namespace FluentTranslate.Infrastructure
{
	public interface IFluentEngine
	{
		void Add(FluentDocument document);
		void Add(FluentRecord record);

		void Add(IFluentFunction function, string? name = default);

		string GetString(string query, FluentEngineContext? context = null);
		string GetString(IFluentElement element, FluentEngineContext? context = null);

		object GetValue(IFluentElement element, FluentEngineContext? context = null);
	}

	public class FluentEngine : IFluentEngine
	{
        private readonly ILogger<FluentEngine>? _logger;

        protected Dictionary<string, IFluentContainer> Content { get; } = new();
		protected Dictionary<string, IFluentFunction> Functions { get; } = new();

		public FluentEngine(ILogger<FluentEngine>? logger = default)
		{
            _logger = logger;
        }

		public void Add(FluentDocument document)
		{
			var records = document.Content
                .OfType<FluentRecord>()
                .SelectMany(x =>
                    GetContent(x).Prepend((x.Reference, x)));

			lock (Content)
			{
                foreach (var (name, record) in records)
                    Content[name] = record;
            }
		}

		public void Add(FluentRecord record)
		{
            var attributes = GetContent(record);

			lock (Content)
			{
				foreach (var (name, attribute) in attributes)
					Content[name] = attribute;
			}
        }

        private IEnumerable<(string, IFluentContainer)> GetContent(FluentRecord record)
		{
			return record.Attributes
				.Select(x => ($"{record.Reference}.{x.Id}", (IFluentContainer)x));
		}

		public void Add(IFluentFunction function, string? name = default)
		{
            name ??= function.Name;
            if (string.IsNullOrWhiteSpace(name))
            {
                _logger?.LogDebug("Missing name of the added function.");
                return;
            }

            lock (Functions)
			{
				Functions[name] = function;
			}
		}

		private readonly static Regex _record = new(@"^(?<term>-)?(?<id>[a-zA-Z]+)(\.(?<attributeId>[a-zA-Z]+))?$", RegexOptions.Compiled | RegexOptions.Singleline);

        public string GetString(string query, FluentEngineContext? context = null)
		{
			if (string.IsNullOrWhiteSpace(query))
				throw new ArgumentException("The query must not be empty.", nameof(query));

			context ??= new();

			var m = _record.Match(query);
            if (m.Success)
			{
				return Evaluate(query, context);
			}

			IFluentElement element;
			try
			{
				element = FluentConverter.Deserialize(query, (lexer, parser) =>
				{
					lexer.PushMode(FluentLexer.SINGLELINE);
					return parser.expressionList();
				});
			}
			catch (Exception ex)
			{
				throw new ArgumentException($"Unable to deserialize a query: {query}", ex);
			}

			var result = GetString(element, context);

			return result;
		}

        public virtual string GetString(IFluentElement element, FluentEngineContext? context = null)
        {
            if (element is null)
                return string.Empty;

            context ??= new();
			return Evaluate(element, context);
        }
		
		protected virtual string Evaluate(IFluentElement element, FluentEngineContext context)
		{
            if (element is null)
                return string.Empty;

            return element switch
            {
                FluentText text => Evaluate(text, context),
                FluentPlaceable placeable => Evaluate(placeable, context),
                //FluentSelection selection => Evaluate(selection, context),
                FluentVariableReference variableReference => Evaluate(variableReference, context),
                FluentMessageReference messageReference => Evaluate(messageReference, context),
                FluentTermReference termReference => Evaluate(termReference, context),
                FluentFunctionCall functionCall => Evaluate(functionCall, context),
                FluentStringLiteral stringLiteral => Evaluate(stringLiteral, context),
                FluentNumberLiteral numberLiteral => Evaluate(numberLiteral, context),
                IFluentContainer container => EvaluateList(container.Content, context),
                _ => EvaluateOther(element, context),
            };
        }
		
		protected virtual string EvaluateOther(IFluentElement element, FluentEngineContext context)
        {
            return string.Empty;
        }

        protected virtual string EvaluateList(IEnumerable<IFluentElement> elements, FluentEngineContext? context = null)
        {
            if (elements is null)
                return string.Empty;

            context ??= new();
            var results = elements.Select(x => Evaluate(x, context));
            return string.Concat(results);
        }

        protected virtual string Evaluate(FluentPlaceable placeable, FluentEngineContext context)
        {
            return Evaluate(placeable.Content, context);
        }

        protected virtual string Evaluate(FluentVariableReference variableReference, FluentEngineContext context)
        {
            var reference = variableReference.Target;
            if (!context.Variables.TryGetValue(reference, out var variable))
                throw new ArgumentException($"Could not find the variable: {reference}");

            return $"{variable}";
        }

        protected virtual string Evaluate(FluentMessageReference messageReference, FluentEngineContext context)
		{
			return Evaluate(messageReference.Target, context);
		}

		protected virtual string Evaluate(FluentTermReference termReference, FluentEngineContext context)
		{
            var arguments = termReference.Arguments
                .Where(x => x.Identifier is not null)
                .ToDictionary(x => x.Identifier, x => GetValue(x.Content, context));

            var next = new FluentEngineContext()
            {
                Parent = context,
                Variables = arguments,
            };

            return Evaluate(termReference.Target, context);
		}

		protected virtual string Evaluate(string reference, FluentEngineContext context)
		{
            if (!Content.TryGetValue(reference, out var container))
                throw new ArgumentException($"Could not find the reference: {reference}");

            return container switch
            {
                FluentMessage message => Evaluate(message, context),
                FluentTerm term => Evaluate(term, context),
                FluentAttribute attribute => Evaluate(attribute, context),
                _ => reference,
            };
        }

        protected virtual string Evaluate(FluentText text, FluentEngineContext context)
        {
            return text.Value;
        }

        protected virtual string Evaluate(FluentStringLiteral literal, FluentEngineContext context)
        {
            return literal.Value;
        }

        protected virtual string Evaluate(FluentNumberLiteral literal, FluentEngineContext context)
        {
            return literal.Value;
        }

        protected virtual string Evaluate(FluentMessage message, FluentEngineContext context)
        {
            return EvaluateList(message.Content, context);
        }

        protected virtual string Evaluate(FluentTerm term, FluentEngineContext context)
        {
            // TODO arguments -> variables
            return EvaluateList(term.Content, context);
        }

        protected virtual string Evaluate(FluentFunctionCall functionCall, FluentEngineContext context)
        {
            var value = GetValue(functionCall, context);
            return $"{value}";
        }

        protected virtual string Evaluate(FluentAttribute attribute, FluentEngineContext context)
        {
            return EvaluateList(attribute.Content, context);
        }

        public virtual object GetValue(IFluentElement element, FluentEngineContext? context = null)
        {
			/*
			 inlineExpression	: 
				      stringLiteral
					| numberLiteral
					| functionCall
					| variableReference
					| termReference		
					| messageReference
					| placeable
			*/

			context ??= new();
            return element switch
            {
                FluentPlaceable placeable => GetValue(placeable, context),
                FluentMessageReference messageReference => GetValue(messageReference, context),
                FluentTermReference termReference => GetValue(termReference, context),
                FluentVariableReference variableReference => GetValue(variableReference, context),
                FluentFunctionCall functionCall => GetValue(functionCall, context),
            };
        }

        protected virtual object GetValue(FluentPlaceable placeable, FluentEngineContext context)
		{
			return GetValue(placeable.Content, context);
		}

		protected virtual object GetValue(FluentMessageReference messageReference, FluentEngineContext context)
		{
            return GetValue(messageReference.Target, context);
        }

        protected virtual object GetValue(FluentTermReference termReference, FluentEngineContext context)
        {
            return GetValue(termReference.Target, context);
        }

        protected virtual object GetValue(string reference, FluentEngineContext context)
		{
            if (!Content.TryGetValue(reference, out var container))
                throw new ArgumentException($"Could not find the reference: {reference}");

			return container switch
			{
				FluentMessage message => Evaluate(message, context),
                FluentTerm term => Evaluate(term, context),
                FluentAttribute attribute => Evaluate(attribute, context),
				_ => reference,
            };
        }

        protected virtual object GetValue(FluentVariableReference variableReference, FluentEngineContext context)
        {
            var variable = context[variableReference.Id];
            return variable;
        }

        protected virtual object GetValue(FluentMessage message, FluentEngineContext context)
        {
            return EvaluateList(message.Content, context);
        }

        protected virtual object GetValue(FluentTerm term, FluentEngineContext context)
        {
			// TODO arguments -> variables
            return EvaluateList(term.Content, context);
        }

        protected virtual object GetValue(FluentAttribute attribute, FluentEngineContext context)
        {
            return EvaluateList(attribute.Content, context);
        }

        protected virtual object GetValue(FluentFunctionCall fuctionCall, FluentEngineContext context)
		{
			var arguments = fuctionCall.Arguments
				.Select(x => new FluentFunctionArgument(x.Identifier, GetValue(x, context)))
                .ToArray();

			if (!Functions.TryGetValue(fuctionCall.Id, out var function))
				throw new ArgumentException($"Could not find function: {fuctionCall.Id}");
			
			return function.Invoke(this, context, arguments);
		}

        protected virtual object GetValue(FluentCallArgument argument, FluentEngineContext context)
        {
			return GetValue(argument.Content, context);
        }

        protected virtual object GetValue(FluentStringLiteral literal, FluentEngineContext context)
        {
            return literal.Value;
        }

        protected virtual object GetValue(FluentNumberLiteral literal)
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
    }
}