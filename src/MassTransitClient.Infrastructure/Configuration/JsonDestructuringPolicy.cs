using Serilog.Core;
using Serilog.Events;
using System.Text.Json;

namespace MassTransitClient.Infrastructure.Configuration
{
    public class JsonDestructuringPolicy : IDestructuringPolicy
    {
        public bool TryDestructure(object value, ILogEventPropertyValueFactory propertyValueFactory, out LogEventPropertyValue result)
        {
            var type = value.GetType();

            if (!type.IsValueType && type != typeof(string))
            {
                result = new ScalarValue(JsonSerializer.Serialize(value));
                return true;
            }

            result = null;
            return false;
        }
    }
}
