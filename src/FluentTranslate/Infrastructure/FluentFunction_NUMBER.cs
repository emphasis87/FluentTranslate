using Microsoft.Extensions.Logging;

namespace FluentTranslate.Infrastructure
{
    /// <summary>
    /// https://projectfluent.org/fluent/guide/functions.html#partial-arguments
    /// https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Intl/NumberFormat
    /// </summary>
    public class FluentFunction_NUMBER : FluentFunction
    {
        private readonly ILogger<FluentFunction_NUMBER>? _logger;

        public FluentFunction_NUMBER(ILogger<FluentFunction_NUMBER>? logger = null)
        { 
            Name = "NUMBER";
            _logger = logger;
        }

        protected override object Evaluate(IFluentEngine engine, FluentEngineContext context, params FluentFunctionArgument[] arguments)
        {
            if (arguments.Length == 0)
                return 0;

            if (!string.IsNullOrWhiteSpace(arguments[0].Name))
            {
                _logger?.LogDebug("Unexpected argument ({Name}) at position 0.", arguments[0].Name);
                return base.Evaluate(engine, context, arguments);
            }

            var value = arguments[0].Value;
            if (value is string number && TryParseNumber(number, out var result))
            {
                // TODO use formatting parameters
                return $"{result}";
            }

            return $"{value}";

            /*
            currencyDisplay
            useGrouping
            minimumIntegerDigits
            minimumFractionDigits
            maximumFractionDigits
            minimumSignificantDigits
            maximumSignificantDigits
            
            style
            currency
            */
        }

        protected virtual bool TryParseNumber(string number, out object result)
        {
            result = 0;
            
            if (string.IsNullOrWhiteSpace(number))
                return false;

            if (ulong.TryParse(number, NumberStyles.Any, CultureInfo.InvariantCulture, out var uint64))
            {
                result = uint64;
                return true;
            }
            if (long.TryParse(number, NumberStyles.Any, CultureInfo.InvariantCulture, out var int64))
            {
                result = int64;
                return true;
            }
            if (decimal.TryParse(number, NumberStyles.Any, CultureInfo.InvariantCulture, out var f128))
            {
                result = f128;
                return true;
            }

            return false;
        }
    }
}
