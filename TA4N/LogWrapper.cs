using Microsoft.Extensions.Logging;

namespace TA4N
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class LogWrapper
    {
        private static ILoggerFactory _factory;

        public LogWrapper(ILoggerFactory factory)
        {
            _factory = factory;
        }

        internal static ILoggerFactory Factory => _factory;
    }  
}
