using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentTranslate.Common.Domain;

namespace FluentTranslate.Common
{
    public interface IFluentTranslator
    {
        string Evaluate(string reference, IDictionary<string, object> arguments);
    }

    public class FluentTranslator : IFluentTranslator
    {
        private readonly IFluentEngine _engine;

        public FluentTranslator(IFluentEngine engine)
        {
            _engine = engine;
        }

        public string Evaluate(string reference, IDictionary<string, object> arguments)
        {
            var container = _engine.FindByReference(reference);
            var context = new FluentEvaluationContext();
            var args = arguments
                .Select(x => (name: x.Key, value: x.Value))
                .ToList();
            context.BeginScope(args);
            var result = Evaluate(container, context);
            context.CloseScope();
            return result;
        }

        public string Evaluate(IFluentContainer container, FluentEvaluationContext context = null)
        {
            context ??= new FluentEvaluationContext();
            var contentItems = container.Content
                .Select(content => Evaluate(content, context))
                .ToList();
            var contents = string.Join("", contentItems);
            return contents;
        }

        public string Evaluate(IFluentContent element, FluentEvaluationContext context = null)
        {
            context ??= new FluentEvaluationContext();
            return element switch
            {
                FluentText text => Evaluate(text, context),
                FluentPlaceable placeable => Evaluate(placeable, context),
                _ => throw new ArgumentOutOfRangeException(nameof(element))
            };
        }

        public string Evaluate(FluentText text, FluentEvaluationContext context = null)
        {
            return text.Value;
        }

        public object Evaluate(IFluentExpression expression, FluentEvaluationContext context)
        {
            context ??= new FluentEvaluationContext();
            return expression switch
            {
                FluentPlaceable placeable => Evaluate(placeable, context),
                FluentSelection selection => Evaluate(selection, context),
                FluentVariableReference variableReference => Evaluate(variableReference, context),
                FluentMessageReference messageReference => Evaluate(messageReference, context),
                FluentTermReference termReference => Evaluate(termReference, context),
                FluentFunctionCall functionCall => Evaluate(functionCall, context),
                FluentStringLiteral stringLiteral => Evaluate(stringLiteral, context),
                FluentNumberLiteral numberLiteral => Evaluate(numberLiteral, context),
                _ => throw new ArgumentOutOfRangeException(nameof(expression))
            };
        }

        public string Evaluate(FluentPlaceable placeable, FluentEvaluationContext context = null)
        {
            context ??= new FluentEvaluationContext();
            var content = Evaluate(placeable.Content, context);
            return $"{content}";
        }

        public string Evaluate(FluentSelection selection, FluentEvaluationContext context = null)
        {
            context ??= new FluentEvaluationContext();
            var match = Evaluate(selection.Match, context);
            var isNumeric = match.GetType().IsNumericType();
            var variant = selection.Variants.FirstOrDefault(v =>
            {
                return v.Key switch
                {
                    FluentNumberLiteral numberLiteral when isNumeric => Equals(Evaluate(numberLiteral, context), match),
                    FluentIdentifier identifier when isNumeric => identifier.Id switch
                    {
                        "zero" when Equals(match, 0) => true,
                        "one" when Equals(match, 1) => true,
                        "two" when Equals(match, 2) => true,
                        "few" => true,
                        "many" => true,
                        "other" => true,
                        _ => false
                    },
                    FluentIdentifier identifier => Equals(identifier.Id, $"{match}"),
                    _ => false
                };
            });
            var result = Evaluate(variant, context);
            return result;
        }

        public object Evaluate(FluentVariableReference variableReference, FluentEvaluationContext context)
        {
            context ??= new FluentEvaluationContext();
            var variable = context.ResolveVariable(variableReference.Id);
            return variable;
        }

        public string Evaluate(FluentMessageReference messageReference, FluentEvaluationContext context)
        {
            context ??= new FluentEvaluationContext();
            var container = _engine.FindByReference(messageReference.Reference);
            return Evaluate(container, context);
        }

        public string Evaluate(FluentTermReference termReference, FluentEvaluationContext context)
        {
            var container = _engine.FindByReference(termReference.Reference);
            var args = termReference.Arguments
                .Select(argument => (name: argument.Id, value: Evaluate(argument.Value, context)))
                .ToList();
            context.BeginScope(args);
            var result = Evaluate(container, context);
            context.CloseScope();
            return result;
        }

        public object Evaluate(FluentFunctionCall functionCall, FluentEvaluationContext context)
        {
            var args = functionCall.Arguments
                .Select(argument => (name: argument.Id, value: Evaluate(argument.Value, context)))
                .ToList();
            var result = _engine.Call(functionCall.Id, args);
            return result;
        }

        public string Evaluate(FluentStringLiteral stringLiteral, FluentEvaluationContext context)
        {
            return stringLiteral.Value;
        }

        public object Evaluate(FluentNumberLiteral numberLiteral, FluentEvaluationContext context)
        {
            if (int.TryParse(numberLiteral.Value, out var i32))
                return i32;
            if (long.TryParse(numberLiteral.Value, out var i64))
                return i64;
            if (float.TryParse(numberLiteral.Value, out var f32))
                return f32;
            if (double.TryParse(numberLiteral.Value, out var f64))
                return f64;
            if (decimal.TryParse(numberLiteral.Value, out var d128))
                return d128;

            throw new ArgumentOutOfRangeException(nameof(numberLiteral));
        }
    }

    public static class FluentTranslatorExtensions
    {
        public static string Translate(this IFluentTranslator translator, string reference, params (string name, object value)[] arguments)
        {
            var args = arguments.ToDictionary(x => x.name, x => x.value);
            return translator.Evaluate(reference, args);
        }

        internal static HashSet<Type> NumericTypes = new HashSet<Type>()
        {
            typeof(sbyte),
            typeof(byte),
            typeof(ushort),
            typeof(short),
            typeof(uint),
            typeof(int),
            typeof(ulong),
            typeof(long),
            typeof(float),
            typeof(double),
            typeof(decimal)
        };

        internal static bool IsNumericType(this Type type)
        {
            return NumericTypes.Contains(type)
                || NumericTypes.Contains(Nullable.GetUnderlyingType(type));
        }
    }
}
