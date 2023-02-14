using FluentTranslate.Domain;

namespace FluentTranslate.Infrastructure
{
    public interface IFluentFunction
    {
        string Name { get; }

        object Invoke(IFluentEngine engine, FluentEngineContext context, FluentFunctionArgument[] arguments);
    }

    public record struct FluentFunctionArgument(string? Name, object Value);

    public abstract class FluentFunction : IFluentFunction
    {
        public string Name { get; init; }

        public FluentFunction(string? name = null)
        {
            Name = name ?? default!;
        }

        public object Invoke(IFluentEngine engine, FluentEngineContext context, params FluentFunctionArgument[] arguments)
        {
            EvaluateArguments(engine, context, arguments);
            var result = Evaluate(engine, context, arguments);
            return result;
        }

        protected virtual object Evaluate(IFluentEngine engine, FluentEngineContext context, params FluentFunctionArgument[] arguments)
        {
            return string.Empty;
        }

        protected void EvaluateArguments(IFluentEngine engine, FluentEngineContext context, params FluentFunctionArgument[] arguments)
        {
            for (var i = 0; i < arguments.Length; i++)
            {
                ref var arg = ref arguments[i];
                if (arg.Value is IFluentElement element)
                    arg = new(arg.Name, engine.GetValue(element, context));
            }
        }
    }
}
