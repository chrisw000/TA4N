using System.Collections.Generic;
using Microsoft.Extensions.Logging.Abstractions;
using NodaTime;
using NUnit.Framework;

namespace TA4N.Test
{
    [Ignore("NUnit is failing System.MissingMethodException on the .ConfigureForNodaTime extension method provided by NuGet package NodaTime.Serialization.JsonNet")]
    public sealed class JsonUtilTest
    {
        private JsonUtil _jsonUtil;
        private TimeSeries _seriesForRun;
        private IList<Tick> _ticks;
        private string _defaultName;

        [SetUp]
        public void SetUp()
        {
            /*
             * For unknown reasons NUnit fails with test with a System.MissingMethodException
             * on the extension method:
             * JsonSerializerSettings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
             * 
             * This is supplied by the NuGet package NodaTime.Serialization.JsonNet
             */
            _jsonUtil = new JsonUtil(NullLogger<JsonUtil>.Instance); //
            _ticks = new List<Tick>
            {
                new Tick(new LocalDateTime(2014, 6, 13, 0, 0, 0), 10d, 12d, 9d, 10.5d, 2d),
                new Tick(new LocalDateTime(2014, 6, 14, 0, 0, 0), 11d, 13d, 10d, 11.5d, 3d),
                new Tick(new LocalDateTime(2014, 6, 15, 0, 0, 0), 12d, 14d, 11d, 12.5d, 4d),
                new Tick(new LocalDateTime(2014, 6, 20, 0, 0, 0), 13d, 15d, 12d, 13.5d, 5d),
                new Tick(new LocalDateTime(2014, 6, 25, 0, 0, 0), 14d, 16d, 13d, 14.5d, 6d),
                new Tick(new LocalDateTime(2014, 6, 30, 0, 0, 0), 15d, 17d, 14d, 15.5d, 7d)
            };
            _defaultName = "Series Name";
            _seriesForRun = new TimeSeries(_defaultName, _ticks);
        }

        [Test]
        public void SerializeDeserializeSerialize()
        {
            // currently the Setup of this is commented out....
            // so test doesn't run
            if (_jsonUtil == null)
                return;
            var initialSerialize = _jsonUtil.SerializeObject(_seriesForRun);
            var initialDeserialize = _jsonUtil.DeserializeObject<TimeSeries>(initialSerialize);
            var secondSerialize = _jsonUtil.SerializeObject(initialDeserialize);
            Assert.That(secondSerialize, Is.EqualTo(initialSerialize));
        }
    }
}