/*
The MIT License (MIT)

Copyright (c) 2014-2016 Marc de Verdelhan & respective authors (see AUTHORS)

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
using System;
using System.Collections.Generic;
using NodaTime;
using NUnit.Framework;
using TA4N.Extend;
using TA4N.Test.FixtureData;
using TA4N.Trading.Rules;

namespace TA4N.Test
{
    public sealed class TimeSeriesTest
    {
        private TimeSeries _defaultSeries;
        private TimeSeries _subSeries;
        private TimeSeries _emptySeries;
        private TimeSeries _seriesForRun;
        private Strategy _strategy;
        private IList<Tick> _ticks;
        private string _defaultName;
        private LocalDateTime _date;

        [SetUp]
        public void SetUp()
        {
            _ticks = new List<Tick>
            {
                GenerateTick.From(new LocalDateTime(2014, 6, 13, 0, 0), 1d),
                GenerateTick.From(new LocalDateTime(2014, 6, 14, 0, 0, 0), 2d),
                GenerateTick.From(new LocalDateTime(2014, 6, 15, 0, 0, 0), 3d),
                GenerateTick.From(new LocalDateTime(2014, 6, 20, 0, 0, 0), 4d),
                GenerateTick.From(new LocalDateTime(2014, 6, 25, 0, 0, 0), 5d),
                GenerateTick.From(new LocalDateTime(2014, 6, 30, 0, 0, 0), 6d)
            };
            _date = new LocalDateTime();
            _defaultName = "Series Name";
            _defaultSeries = new TimeSeries(_defaultName, _ticks);
            _subSeries = _defaultSeries.Subseries(2, 4);
            _emptySeries = new TimeSeries();
            _seriesForRun = GenerateTimeSeries.From(new[] { 1d, 2d, 3d, 4d, 5d, 6d, 7d, 8d, 9d }, new[] { _date.WithDate(2013, 01, 01), _date.WithDate(2013, 08, 01), _date.WithDate(2013, 10, 01), _date.WithDate(2013, 12, 01), _date.WithDate(2014, 02, 01), _date.WithDate(2015, 01, 01), _date.WithDate(2015, 08, 01), _date.WithDate(2015, 10, 01), _date.WithDate(2015, 12, 01) });
            // Strategy would need a real test class
            _strategy = new Strategy(new FixedRule(0, 2, 3, 6), new FixedRule(1, 4, 7, 8))
            {
                UnstablePeriod = 2
            };
        }

        [Test]
        public void GetEndGetBeginGetTickCount()
        {
            Assert.Multiple(() =>
            {
                // Original series
                Assert.That(_defaultSeries.Begin, Is.EqualTo(0));
                Assert.That(_defaultSeries.End, Is.EqualTo(_ticks.Count - 1));
                Assert.That(_defaultSeries.TickCount, Is.EqualTo(_ticks.Count));
                // Sub-series
                Assert.That(_subSeries.Begin, Is.EqualTo(2));
                Assert.That(_subSeries.End, Is.EqualTo(4));
                Assert.That(_subSeries.TickCount, Is.EqualTo(3));
                // Empty series
                Assert.That(_emptySeries.Begin, Is.EqualTo(-1));
                Assert.That(_emptySeries.End, Is.EqualTo(-1));
                Assert.That(_emptySeries.TickCount, Is.EqualTo(0));
            });
        }

        [Test]
        public void GetSeriesPeriodDescription()
        {
            Assert.Multiple(() =>
            {
                // Original series
                Assert.That(_defaultSeries.SeriesPeriodDescription.EndsWith(_ticks[_defaultSeries.End].EndTime.ToString("HH:mm dd/MM/yyyy", null)), Is.True);
                Assert.That(_defaultSeries.SeriesPeriodDescription.StartsWith(_ticks[_defaultSeries.Begin].EndTime.ToString("HH:mm dd/MM/yyyy", null)), Is.True);
                // Sub-series
                Assert.That(_subSeries.SeriesPeriodDescription.EndsWith(_ticks[_subSeries.End].EndTime.ToString("HH:mm dd/MM/yyyy", null)), Is.True);
                Assert.That(_subSeries.SeriesPeriodDescription.StartsWith(_ticks[_subSeries.Begin].EndTime.ToString("HH:mm dd/MM/yyyy", null)), Is.True);
                // Empty series
                Assert.That(_emptySeries.SeriesPeriodDescription, Is.EqualTo(string.Empty));
            });
        }

        [Test]
        public void GetName()
        {
            Assert.Multiple(() =>
            {
                Assert.That(_defaultSeries.Name, Is.EqualTo(_defaultName));
                Assert.That(_subSeries.Name, Is.EqualTo(_defaultName));
            });
        }

        [Test]
        public void GetTickWithRemovedIndexOnMovingSeriesShouldReturnFirstRemainingTick()
        {
            var tick = _defaultSeries.GetTick(4);
            _defaultSeries.MaximumTickCount = 2;
            Assert.Multiple(() =>
            {
                Assert.That(_defaultSeries.GetTick(0), Is.SameAs(tick));
                Assert.That(_defaultSeries.GetTick(1), Is.SameAs(tick));
                Assert.That(_defaultSeries.GetTick(2), Is.SameAs(tick));
                Assert.That(_defaultSeries.GetTick(3), Is.SameAs(tick));
                Assert.That(_defaultSeries.GetTick(4), Is.SameAs(tick));
                Assert.That(_defaultSeries.GetTick(5), Is.Not.SameAs(tick));
            });
        }

        [Test]
        public void GetTickOnMovingAndEmptySeriesShouldThrowException()
        {
            _defaultSeries.MaximumTickCount = 2;
            _ticks.Clear(); // Should not be used like this
            Assert.Throws<IndexOutOfRangeException>(() => _defaultSeries.GetTick(1));
        }

        [Test]
        public void GetTickWithNegativeIndexShouldThrowException()
        {
            Assert.Throws<IndexOutOfRangeException>(() => _defaultSeries.GetTick(-1));
        }

        [Test]
        public void GetTickWithIndexGreaterThanTickCountShouldThrowException()
        {
            Assert.Throws<IndexOutOfRangeException>(() => _defaultSeries.GetTick(10));
        }

        [Test]
        public void GetTickOnMovingSeries()
        {
            var tick = _defaultSeries.GetTick(4);
            _defaultSeries.MaximumTickCount = 2;
            Assert.That(_defaultSeries.GetTick(4), Is.EqualTo(tick));
        }

        [Test]
        public void MaximumTickCountOnSubserieShouldThrowException()
        {
            Assert.Throws<InvalidOperationException>(() => _subSeries.MaximumTickCount = 10);
        }

        [Test]
        public void NegativeMaximumTickCountShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => _defaultSeries.MaximumTickCount = -1);
        }

        [Test]
        public void SetMaximumTickCount()
        {
            // Before
            Assert.Multiple(() =>
            {
                Assert.That(_defaultSeries.Begin, Is.EqualTo(0));
                Assert.That(_defaultSeries.End, Is.EqualTo(_ticks.Count - 1));
                Assert.That(_defaultSeries.TickCount, Is.EqualTo(_ticks.Count));
            });
            _defaultSeries.MaximumTickCount = 3;
            // After
            Assert.Multiple(() =>
            {
                Assert.That(_defaultSeries.Begin, Is.EqualTo(0));
                Assert.That(_defaultSeries.End, Is.EqualTo(5));
                Assert.That(_defaultSeries.TickCount, Is.EqualTo(3));
            });
        }

        [Test]
        public void AddNullTickShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => _defaultSeries.AddTick(null));
        }

        [Test]
        public void AddTickWithEndTimePriorToSeriesEndTimeShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => _defaultSeries.AddTick(GenerateTick.From(_date.WithDate(2000, 1, 1), 99d)));
        }

        [Test]
        public void AddTick()
        {
            _defaultSeries = new TimeSeries();
            Tick firstTick = GenerateTick.From(_date.WithDate(2014, 6, 13), 1d);
            Tick secondTick = GenerateTick.From(_date.WithDate(2014, 6, 14), 2d);
            Assert.Multiple(() =>
            {
                Assert.That(_defaultSeries.TickCount, Is.EqualTo(0));
                Assert.That(_defaultSeries.Begin, Is.EqualTo(-1));
                Assert.That(_defaultSeries.End, Is.EqualTo(-1));
            });
            _defaultSeries.AddTick(firstTick);
            Assert.Multiple(() =>
            {
                Assert.That(_defaultSeries.TickCount, Is.EqualTo(1));
                Assert.That(_defaultSeries.Begin, Is.EqualTo(0));
                Assert.That(_defaultSeries.End, Is.EqualTo(0));
            });
            _defaultSeries.AddTick(secondTick);
            Assert.Multiple(() =>
            {
                Assert.That(_defaultSeries.TickCount, Is.EqualTo(2));
                Assert.That(_defaultSeries.Begin, Is.EqualTo(0));
                Assert.That(_defaultSeries.End, Is.EqualTo(1));
            });
        }

        [Test]
        public void SubseriesWithIndexes()
        {
            var subSeries2 = _defaultSeries.Subseries(2, 5);
            Assert.Multiple(() =>
            {
                Assert.That(subSeries2.Name, Is.EqualTo(_defaultSeries.Name));
                Assert.That(subSeries2.Begin, Is.EqualTo(2));
                Assert.That(subSeries2.Begin, Is.Not.EqualTo(_defaultSeries.Begin));
                Assert.That(subSeries2.End, Is.EqualTo(5));
                Assert.That(subSeries2.End, Is.EqualTo(_defaultSeries.End));
                Assert.That(subSeries2.TickCount, Is.EqualTo(4));
            });
        }

        [Test]
        public void SubseriesOnSeriesWithMaximumTickCountShouldThrowException()
        {
            _defaultSeries.MaximumTickCount = 3;
            Assert.Throws<InvalidOperationException>(() => _defaultSeries.Subseries(0, 1));
        }

        [Test]
        public void SubseriesWithInvalidIndexesShouldThrowException()
        {
            Assert.Throws<IndexOutOfRangeException>(() => _defaultSeries.Subseries(4, 2));
        }

        [Test]
        public void SubseriesWithDuration()
        {
            var subSeries2 = _defaultSeries.Subseries(1, Period.FromWeeks(2));
            Assert.Multiple(() =>
            {
                Assert.That(subSeries2.Name, Is.EqualTo(_defaultSeries.Name));
                Assert.That(subSeries2.Begin, Is.EqualTo(1));
                Assert.That(subSeries2.Begin, Is.Not.EqualTo(_defaultSeries.Begin));
                Assert.That(subSeries2.End, Is.EqualTo(4));
                Assert.That(subSeries2.End, Is.Not.EqualTo(_defaultSeries.End));
                Assert.That(subSeries2.TickCount, Is.EqualTo(4));
            });
        }

        [Test]
        public void SplitEvery3Ticks()
        {
            TimeSeries series = GenerateTimeSeries.From(_date.WithYear(2010), _date.WithYear(2011), _date.WithYear(2012), _date.WithYear(2015), _date.WithYear(2016), _date.WithYear(2017), _date.WithYear(2018), _date.WithYear(2019));
            var subseries = series.Split(3);
            Assert.Multiple(() =>
            {
                Assert.That(subseries.Count, Is.EqualTo(3));
                Assert.That(subseries[0].Begin, Is.EqualTo(0));
                Assert.That(subseries[0].End, Is.EqualTo(2));
                Assert.That(subseries[1].Begin, Is.EqualTo(3));
                Assert.That(subseries[1].End, Is.EqualTo(5));
                Assert.That(subseries[2].Begin, Is.EqualTo(6));
                Assert.That(subseries[2].End, Is.EqualTo(7));
            });
        }

        [Test]
        public void SplitByYearForTwoYearsSubseries()
        {
            TimeSeries series = GenerateTimeSeries.From(_date.WithYear(2010), _date.WithYear(2011), _date.WithYear(2012), _date.WithYear(2015), _date.WithYear(2016));
            var subseries = series.Split(Period.FromYears(1), Period.FromYears(2));
            Assert.Multiple(() =>
            {
                Assert.That(subseries.Count, Is.EqualTo(5));
                Assert.That(subseries[0].Begin, Is.EqualTo(0));
                Assert.That(subseries[0].End, Is.EqualTo(1));
                Assert.That(subseries[1].Begin, Is.EqualTo(1));
                Assert.That(subseries[1].End, Is.EqualTo(2));
                Assert.That(subseries[2].Begin, Is.EqualTo(2));
                Assert.That(subseries[2].End, Is.EqualTo(2));
                Assert.That(subseries[4].Begin, Is.EqualTo(4));
                Assert.That(subseries[4].End, Is.EqualTo(4));
            });
        }

        [Test]
        public void SplitByMonthForOneWeekSubseries()
        {
            TimeSeries series = GenerateTimeSeries.From(_date.WithMonthOfYear(0x4), _date.WithMonthOfYear(0x5), _date.WithMonthOfYear(0x7));
            var subseries = series.Split(Period.FromMonths(1), Period.FromWeeks(1));
            Assert.Multiple(() =>
            {
                Assert.That(subseries.Count, Is.EqualTo(3));
                Assert.That(subseries[0].Begin, Is.EqualTo(0));
                Assert.That(subseries[0].End, Is.EqualTo(0));
                Assert.That(subseries[1].Begin, Is.EqualTo(1));
                Assert.That(subseries[1].End, Is.EqualTo(1));
                Assert.That(subseries[2].Begin, Is.EqualTo(2));
                Assert.That(subseries[2].End, Is.EqualTo(2));
            });
        }

        [Test]
        public void SplitByHour()
        {
            var time = new LocalDateTime(1970, 1, 1, 10, 0, 0);
            TimeSeries series = GenerateTimeSeries.From(time, time.PlusMinutes(1), time.PlusMinutes(2), time.PlusMinutes(10), time.PlusMinutes(15), time.PlusMinutes(25), time.PlusHours(1), time.PlusHours(5), time.PlusHours(10).PlusMinutes(10), time.PlusHours(10).PlusMinutes(20), time.PlusHours(10).PlusMinutes(30));
            var subseries = series.Split(Period.FromHours(1));
            Assert.Multiple(() =>
            {
                Assert.That(subseries.Count, Is.EqualTo(4));
                Assert.That(subseries[0].Begin, Is.EqualTo(0));
                Assert.That(subseries[0].End, Is.EqualTo(5));
                Assert.That(subseries[1].Begin, Is.EqualTo(6));
                Assert.That(subseries[1].End, Is.EqualTo(6));
                Assert.That(subseries[2].Begin, Is.EqualTo(7));
                Assert.That(subseries[2].End, Is.EqualTo(7));
                Assert.That(subseries[3].Begin, Is.EqualTo(8));
                Assert.That(subseries[3].End, Is.EqualTo(10));
            });
        }

        [Test]
        public void RunOnWholeSeries()
        {
            TimeSeries series = GenerateTimeSeries.From(20d, 40d, 60d, 10d, 30d, 50d, 0d, 20d, 40d);
            var allTrades = series.Run(_strategy).Trades;
            Assert.That(allTrades.Count, Is.EqualTo(2));
        }

        [Test]
        public void RunOnWholeSeriesWithAmount()
        {
            TimeSeries series = GenerateTimeSeries.From(20d, 40d, 60d, 10d, 30d, 50d, 0d, 20d, 40d);
            var allTrades = series.Run(_strategy, OrderType.Buy, Decimal.Hundred).Trades;
            Assert.Multiple(() =>
            {
                Assert.That(allTrades.Count, Is.EqualTo(2));
                Assert.That(allTrades[0].Entry.Amount, Is.EqualTo(Decimal.Hundred));
                Assert.That(allTrades[1].Entry.Amount, Is.EqualTo(Decimal.Hundred));
            });
        }

        [Test]
        public void RunOnSlice()
        {
            var subseries = _seriesForRun.Split(Period.FromYears(2000));
            var slice = subseries[0];
            var trades = slice.Run(_strategy).Trades;
            Assert.Multiple(() =>
            {
                Assert.That(trades.Count, Is.EqualTo(2));
                Assert.That(trades[0].Entry, Is.EqualTo(Order.BuyAt(2, slice.GetTick(2).ClosePrice, Decimal.NaNRenamed)));
                Assert.That(trades[0].Exit, Is.EqualTo(Order.SellAt(4, slice.GetTick(4).ClosePrice, Decimal.NaNRenamed)));
                Assert.That(trades[1].Entry, Is.EqualTo(Order.BuyAt(6, slice.GetTick(6).ClosePrice, Decimal.NaNRenamed)));
                Assert.That(trades[1].Exit, Is.EqualTo(Order.SellAt(7, slice.GetTick(7).ClosePrice, Decimal.NaNRenamed)));
            });
        }

        [Test]
        public void RunWithOpenEntryBuyLeft()
        {
            var subseries = _seriesForRun.Split(Period.FromYears(1));
            var slice = subseries[0];
            var aStrategy = new Strategy(new FixedRule(1), new FixedRule(3));
            var trades = slice.Run(aStrategy).Trades;
            Assert.Multiple(() =>
            {
                Assert.That(trades.Count, Is.EqualTo(1));
                Assert.That(trades[0].Entry, Is.EqualTo(Order.BuyAt(1, slice.GetTick(1).ClosePrice, Decimal.NaNRenamed)));
                Assert.That(trades[0].Exit, Is.EqualTo(Order.SellAt(3, slice.GetTick(3).ClosePrice, Decimal.NaNRenamed)));
            });
        }

        [Test]
        public void RunWithOpenEntrySellLeft()
        {
            var subseries = _seriesForRun.Split(Period.FromYears(1));
            var slice = subseries[0];
            var aStrategy = new Strategy(new FixedRule(1), new FixedRule(3));
            var trades = slice.Run(aStrategy, OrderType.Sell).Trades;
            Assert.Multiple(() =>
            {
                Assert.That(trades.Count, Is.EqualTo(1));
                Assert.That(trades[0].Entry, Is.EqualTo(Order.SellAt(1, slice.GetTick(1).ClosePrice, Decimal.NaNRenamed)));
                Assert.That(trades[0].Exit, Is.EqualTo(Order.BuyAt(3, slice.GetTick(3).ClosePrice, Decimal.NaNRenamed)));
            });
        }

        [Test]
        public void RunSplitted()
        {
            var subseries = _seriesForRun.Split(Period.FromYears(1));
            var slice0 = subseries[0];
            var slice1 = subseries[1];
            var slice2 = subseries[2];
            var trades = slice0.Run(_strategy).Trades;
            Assert.That(trades.Count, Is.EqualTo(1));
            Assert.That(trades[0].Entry, Is.EqualTo(Order.BuyAt(2, slice0.GetTick(2).ClosePrice, Decimal.NaNRenamed)));
            Assert.That(trades[0].Exit, Is.EqualTo(Order.SellAt(4, slice0.GetTick(4).ClosePrice, Decimal.NaNRenamed)));
            trades = slice1.Run(_strategy).Trades;
            Assert.That(trades.Count == 0, Is.True);
            trades = slice2.Run(_strategy).Trades;
            Assert.That(trades.Count, Is.EqualTo(1));
            Assert.That(trades[0].Entry, Is.EqualTo(Order.BuyAt(6, slice2.GetTick(6).ClosePrice, Decimal.NaNRenamed)));
            Assert.That(trades[0].Exit, Is.EqualTo(Order.SellAt(7, slice2.GetTick(7).ClosePrice, Decimal.NaNRenamed)));
        }

        [Test]
        public void Splitted()
        {
            var date = new LocalDateTime();
            TimeSeries series = GenerateTimeSeries.From(new double[] { 1d, 2d, 3d, 4d, 5d, 6d, 7d, 8d, 9d, 10d }, new LocalDateTime[] { date.WithYear(2000), date.WithYear(2000), date.WithYear(2001), date.WithYear(2001), date.WithYear(2002), date.WithYear(2002), date.WithYear(2002), date.WithYear(2003), date.WithYear(2004), date.WithYear(2005) });
            var aStrategy = new Strategy(new FixedRule(0, 3, 5, 7), new FixedRule(2, 4, 6, 9));
            var subseries = series.Split(Period.FromYears(1));
            var slice0 = subseries[0];
            var slice1 = subseries[1];
            var slice2 = subseries[2];
            var slice3 = subseries[3];
            var slice4 = subseries[4];
            var slice5 = subseries[5];
            var trades = slice0.Run(aStrategy).Trades;
            Assert.That(trades.Count, Is.EqualTo(1));
            Assert.That(trades[0].Entry, Is.EqualTo(Order.BuyAt(0, slice0.GetTick(0).ClosePrice, Decimal.NaNRenamed)));
            Assert.That(trades[0].Exit, Is.EqualTo(Order.SellAt(2, slice0.GetTick(2).ClosePrice, Decimal.NaNRenamed)));
            trades = slice1.Run(aStrategy).Trades;
            Assert.That(trades.Count, Is.EqualTo(1));
            Assert.That(trades[0].Entry, Is.EqualTo(Order.BuyAt(3, slice1.GetTick(3).ClosePrice, Decimal.NaNRenamed)));
            Assert.That(trades[0].Exit, Is.EqualTo(Order.SellAt(4, slice1.GetTick(4).ClosePrice, Decimal.NaNRenamed)));
            trades = slice2.Run(aStrategy).Trades;
            Assert.That(trades.Count, Is.EqualTo(1));
            Assert.That(trades[0].Entry, Is.EqualTo(Order.BuyAt(5, slice2.GetTick(5).ClosePrice, Decimal.NaNRenamed)));
            Assert.That(trades[0].Exit, Is.EqualTo(Order.SellAt(6, slice2.GetTick(6).ClosePrice, Decimal.NaNRenamed)));
            trades = slice3.Run(aStrategy).Trades;
            Assert.That(trades.Count, Is.EqualTo(1));
            Assert.That(trades[0].Entry, Is.EqualTo(Order.BuyAt(7, slice3.GetTick(7).ClosePrice, Decimal.NaNRenamed)));
            Assert.That(trades[0].Exit, Is.EqualTo(Order.SellAt(9, slice3.GetTick(9).ClosePrice, Decimal.NaNRenamed)));
            trades = slice4.Run(aStrategy).Trades;
            Assert.That(trades.Count == 0, Is.True);
            trades = slice5.Run(aStrategy).Trades;
            Assert.That(trades.Count == 0, Is.True);
        }
    }
}