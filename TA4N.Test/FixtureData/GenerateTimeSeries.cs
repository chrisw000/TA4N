using System;
using System.Collections.Generic;
using NodaTime;
using TA4N.Extend;

namespace TA4N.Test.FixtureData
{
    public class GenerateTimeSeries
    {
        public static TimeSeries From(params double[] data)
        {
            return new TimeSeries(DoublesToTicks(data));
        }

        public static TimeSeries From(IList<Tick> ticks)
        {
            return new TimeSeries(ticks);
        }

        public static TimeSeries From(double[] data, LocalDateTime[] times)
        {
            return new TimeSeries(DoublesAndTimesToTicks(data, times));
        }

        public static TimeSeries From(params LocalDateTime[] dates)
        {
            return new TimeSeries(TimesToTicks(dates));
        }

        public static TimeSeries WithArbitraryTicks()
        {
            return new TimeSeries(ArbitraryTicks());
        }

        #region helpers
        private static IList<Tick> DoublesToTicks(params double[] data)
        {
            var ticks = new List<Tick>();
            for (var i = 0; i < data.Length; i++)
            {
                //TODO: double check the intention here...
                ticks.Add(GenerateTick.From((new LocalDateTime()).WithMillisOfSecond(i), data[i]));
            }
            return ticks;
        }

        private static IList<Tick> DoublesAndTimesToTicks(double[] data, LocalDateTime[] times)
        {
            if (data.Length != times.Length)
            {
                throw new ArgumentException();
            }
            var ticks = new List<Tick>();
            for (var i = 0; i < data.Length; i++)
            {
                ticks.Add(GenerateTick.From(times[i], data[i]));
            }
            return ticks;
        }

        private static IList<Tick> TimesToTicks(params LocalDateTime[] dates)
        {
            var ticks = new List<Tick>();
            var i = 1;
            foreach (var date in dates)
            {
                ticks.Add(GenerateTick.From(date, i++));
            }
            return ticks;
        }

        private static IList<Tick> ArbitraryTicks()
        {
            var ticks = new List<Tick>();
            for (var i = 0d; i < 10; i++)
            {
                ticks.Add(GenerateTick.From(new LocalDateTime(0,1,1,0,0), i, i + 1, i + 2, i + 3, i + 4, i + 5, (int)(i + 6)));
            }
            return ticks;
        }
        #endregion
    }
}