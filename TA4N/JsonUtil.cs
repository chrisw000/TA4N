using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NodaTime;
using NodaTime.Serialization.JsonNet;

namespace TA4N
{
    public class JsonUtil : IJsonUtil
    {
        private static ILogger<JsonUtil> _logger;
        
        public JsonUtil(ILogger<JsonUtil> logger)
        {
            if (_logger == null)
            {
                _logger = logger;
            }

            SerializerSettings = new JsonSerializerSettings
            {
                FloatParseHandling = FloatParseHandling.Decimal,
                NullValueHandling = NullValueHandling.Ignore,
                Error = delegate(object sender, ErrorEventArgs args)
                {
                    if (args.CurrentObject == args.ErrorContext.OriginalObject)
                    {
                        _logger?.LogError("Json serialization error {@OriginalObject} {@Member} {@ErrorMessage}"
                            , args.ErrorContext.OriginalObject
                            , args.ErrorContext.Member
                            , args.ErrorContext.Error.Message);
                    }
                }
            // For unknown reasons this extension method causes System.MissingMethodException
            // to be thrown in NUnit - so this class doesn't have any test coverage at the moment
            }.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
        }

        private JsonSerializerSettings SerializerSettings { get; } 

        public string SerializeObject(object value)
        {
            return JsonConvert.SerializeObject(value, SerializerSettings);
        }

        public T DeserializeObject<T>(string contentBody)
        {
            return JsonConvert.DeserializeObject<T>(contentBody, SerializerSettings);
        }
    }
}
