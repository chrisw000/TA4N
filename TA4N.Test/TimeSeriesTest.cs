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
using NUnit.Framework;
using NodaTime;
using TA4N.Mocks;
using TA4N.Trading.Rules;
using TA4N.Extend;

namespace TA4N
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
		        new MockTick(new LocalDateTime(2014, 6, 13, 0, 0), 1d),
		        new MockTick(new LocalDateTime(2014, 6, 14, 0, 0, 0), 2d),
		        new MockTick(new LocalDateTime(2014, 6, 15, 0, 0, 0), 3d),
		        new MockTick(new LocalDateTime(2014, 6, 20, 0, 0, 0), 4d),
		        new MockTick(new LocalDateTime(2014, 6, 25, 0, 0, 0), 5d),
		        new MockTick(new LocalDateTime(2014, 6, 30, 0, 0, 0), 6d)
		    };

            _date = new LocalDateTime();
		    _defaultName = "Series Name";

			_defaultSeries = new TimeSeries(_defaultName, _ticks);
			_subSeries = _defaultSeries.Subseries(2, 4);
			_emptySeries = new TimeSeries();

			_seriesForRun = new MockTimeSeries(new[]
			                        {
			                            1d, 2d, 3d, 4d, 5d, 6d, 7d, 8d, 9d
			                        }, new[]
			                        {
			                            _date.WithDate(2013,01,01),
                                        _date.WithDate(2013,08,01),
                                        _date.WithDate(2013,10,01),
                                        _date.WithDate(2013,12,01),
                                        _date.WithDate(2014,02,01),
                                        _date.WithDate(2015,01,01),
                                        _date.WithDate(2015,08,01),
                                        _date.WithDate(2015,10,01),
                                        _date.WithDate(2015,12,01)
			                        });

            // Strategy would need a real test class
            _strategy = new Strategy(
                new FixedRule(0, 2, 3, 6), 
                new FixedRule(1, 4, 7, 8))
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
                Assert.AreEqual(0, _defaultSeries.Begin);
                Assert.AreEqual(_ticks.Count - 1, _defaultSeries.End);
                Assert.AreEqual(_ticks.Count, _defaultSeries.TickCount);
                // Sub-series
                Assert.AreEqual(2, _subSeries.Begin);
                Assert.AreEqual(4, _subSeries.End);
                Assert.AreEqual(3, _subSeries.TickCount);
                // Empty series
                Assert.AreEqual(-1, _emptySeries.Begin);
                Assert.AreEqual(-1, _emptySeries.End);
                Assert.AreEqual(0, _emptySeries.TickCount);
            });
        }

        [Test]
		public void GetSeriesPeriodDescription()
        {
            Assert.Multiple(() =>
            {
                // Original series
                Assert.IsTrue(
                    _defaultSeries.SeriesPeriodDescription.EndsWith(
                        _ticks[_defaultSeries.End].EndTime.ToString("HH:mm dd/MM/yyyy", null)));
                Assert.IsTrue(
                    _defaultSeries.SeriesPeriodDescription.StartsWith(
                        _ticks[_defaultSeries.Begin].EndTime.ToString("HH:mm dd/MM/yyyy", null)));
                // Sub-series
                Assert.IsTrue(
                    _subSeries.SeriesPeriodDescription.EndsWith(_ticks[_subSeries.End].EndTime.ToString(
                        "HH:mm dd/MM/yyyy", null)));
                Assert.IsTrue(
                    _subSeries.SeriesPeriodDescription.StartsWith(
                        _ticks[_subSeries.Begin].EndTime.ToString("HH:mm dd/MM/yyyy", null)));
                // Empty series
                Assert.AreEqual(string.Empty, _emptySeries.SeriesPeriodDescription);
            });
        }

        [Test]
		public void GetName()
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(_defaultName, _defaultSeries.Name);
                Assert.AreEqual(_defaultName, _subSeries.Name);
            });
        }

        [Test]
		public void GetTickWithRemovedIndexOnMovingSeriesShouldReturnFirstRemainingTick()
		{
			var tick = _defaultSeries.GetTick(4);
			_defaultSeries.MaximumTickCount = 2;

		    Assert.Multiple(() =>
		    {
		        Assert.AreSame(tick, _defaultSeries.GetTick(0));
		        Assert.AreSame(tick, _defaultSeries.GetTick(1));
		        Assert.AreSame(tick, _defaultSeries.GetTick(2));
		        Assert.AreSame(tick, _defaultSeries.GetTick(3));
		        Assert.AreSame(tick, _defaultSeries.GetTick(4));
		        Assert.AreNotSame(tick, _defaultSeries.GetTick(5));
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
			Assert.AreEqual(tick, _defaultSeries.GetTick(4));
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
		        Assert.AreEqual(0, _defaultSeries.Begin);
		        Assert.AreEqual(_ticks.Count - 1, _defaultSeries.End);
		        Assert.AreEqual(_ticks.Count, _defaultSeries.TickCount);
		    });

			_defaultSeries.MaximumTickCount = 3;

            // After
		    Assert.Multiple(() =>
		    {
		        Assert.AreEqual(0, _defaultSeries.Begin);
		        Assert.AreEqual(5, _defaultSeries.End);
		        Assert.AreEqual(3, _defaultSeries.TickCount);
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
            Assert.Throws<ArgumentException>(() => _defaultSeries.AddTick(new MockTick(_date.WithDate(2000, 1, 1), 99d)));
		}

        [Test]
		public void AddTick()
		{
			_defaultSeries = new TimeSeries();
            Tick firstTick = new MockTick(_date.WithDate(2014, 6, 13), 1d);
			Tick secondTick = new MockTick(_date.WithDate(2014, 6, 14), 2d);

		    Assert.Multiple(() =>
		    {
		        Assert.AreEqual(0, _defaultSeries.TickCount);
		        Assert.AreEqual(-1, _defaultSeries.Begin);
		        Assert.AreEqual(-1, _defaultSeries.End);
		    });

			_defaultSeries.AddTick(firstTick);
		    Assert.Multiple(() =>
		    {
		        Assert.AreEqual(1, _defaultSeries.TickCount);
		        Assert.AreEqual(0, _defaultSeries.Begin);
		        Assert.AreEqual(0, _defaultSeries.End);
		    });

			_defaultSeries.AddTick(secondTick);
		    Assert.Multiple(() =>
		    {
		        Assert.AreEqual(2, _defaultSeries.TickCount);
		        Assert.AreEqual(0, _defaultSeries.Begin);
		        Assert.AreEqual(1, _defaultSeries.End);
		    });
		}

        [Test]
		public void SubseriesWithIndexes()
		{
			var subSeries2 = _defaultSeries.Subseries(2, 5);
		    Assert.Multiple(() =>
		    {
		        Assert.AreEqual(_defaultSeries.Name, subSeries2.Name);
		        Assert.AreEqual(2, subSeries2.Begin);
		        Assert.AreNotEqual(_defaultSeries.Begin, subSeries2.Begin);
		        Assert.AreEqual(5, subSeries2.End);
		        Assert.AreEqual(_defaultSeries.End, subSeries2.End);
		        Assert.AreEqual(4, subSeries2.TickCount);
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
		        Assert.AreEqual(_defaultSeries.Name, subSeries2.Name);
		        Assert.AreEqual(1, subSeries2.Begin);
		        Assert.AreNotEqual(_defaultSeries.Begin, subSeries2.Begin);
		        Assert.AreEqual(4, subSeries2.End);
		        Assert.AreNotEqual(_defaultSeries.End, subSeries2.End);
		        Assert.AreEqual(4, subSeries2.TickCount);
		    });
		}

        [Test]
		public void SplitEvery3Ticks()
		{
			TimeSeries series = new MockTimeSeries(_date.WithYear(2010), _date.WithYear(2011), _date.WithYear(2012), _date.WithYear(2015), _date.WithYear(2016), _date.WithYear(2017), _date.WithYear(2018), _date.WithYear(2019));

			var subseries = series.Split(3);
		    Assert.Multiple(() =>
		    {
		        Assert.AreEqual(3, subseries.Count);

		        Assert.AreEqual(0, subseries[0].Begin);
		        Assert.AreEqual(2, subseries[0].End);

		        Assert.AreEqual(3, subseries[1].Begin);
		        Assert.AreEqual(5, subseries[1].End);

		        Assert.AreEqual(6, subseries[2].Begin);
		        Assert.AreEqual(7, subseries[2].End);
		    });
		}

        [Test]
		public void SplitByYearForTwoYearsSubseries()
		{
			TimeSeries series = new MockTimeSeries(_date.WithYear(2010), _date.WithYear(2011), _date.WithYear(2012), _date.WithYear(2015), _date.WithYear(2016));

			var subseries = series.Split(Period.FromYears(1), Period.FromYears(2));

		    Assert.Multiple(() =>
		    {
		        Assert.AreEqual(5, subseries.Count);

		        Assert.AreEqual(0, subseries[0].Begin);
		        Assert.AreEqual(1, subseries[0].End);

		        Assert.AreEqual(1, subseries[1].Begin);
		        Assert.AreEqual(2, subseries[1].End);

		        Assert.AreEqual(2, subseries[2].Begin);
		        Assert.AreEqual(2, subseries[2].End);

		        Assert.AreEqual(4, subseries[4].Begin);
		        Assert.AreEqual(4, subseries[4].End);
		    });
		}

        [Test]
		public void SplitByMonthForOneWeekSubseries()
		{
			TimeSeries series = new MockTimeSeries(_date.WithMonthOfYear(0x4), _date.WithMonthOfYear(0x5), _date.WithMonthOfYear(0x7));

			var subseries = series.Split(Period.FromMonths(1), Period.FromWeeks(1));
		    Assert.Multiple(() =>
		    {
		        Assert.AreEqual(3, subseries.Count);

		        Assert.AreEqual(0, subseries[0].Begin);
		        Assert.AreEqual(0, subseries[0].End);

		        Assert.AreEqual(1, subseries[1].Begin);
		        Assert.AreEqual(1, subseries[1].End);

		        Assert.AreEqual(2, subseries[2].Begin);
		        Assert.AreEqual(2, subseries[2].End);
		    });
		}

        [Test]
		public void SplitByHour()
		{
            var time = new LocalDateTime(1970, 1, 1, 10, 0, 0);
			TimeSeries series = new MockTimeSeries(time, time.PlusMinutes(1), time.PlusMinutes(2), time.PlusMinutes(10), time.PlusMinutes(15), time.PlusMinutes(25), time.PlusHours(1), time.PlusHours(5), time.PlusHours(10).PlusMinutes(10), time.PlusHours(10).PlusMinutes(20), time.PlusHours(10).PlusMinutes(30));

			var subseries = series.Split(Period.FromHours(1));
		    Assert.Multiple(() =>
		    {
		        Assert.AreEqual(4, subseries.Count);

		        Assert.AreEqual(0, subseries[0].Begin);
		        Assert.AreEqual(5, subseries[0].End);

		        Assert.AreEqual(6, subseries[1].Begin);
		        Assert.AreEqual(6, subseries[1].End);

		        Assert.AreEqual(7, subseries[2].Begin);
		        Assert.AreEqual(7, subseries[2].End);

		        Assert.AreEqual(8, subseries[3].Begin);
		        Assert.AreEqual(10, subseries[3].End);
		    });
		}

        [Test]
		public void RunOnWholeSeries()
		{
			TimeSeries series = new MockTimeSeries(20d, 40d, 60d, 10d, 30d, 50d, 0d, 20d, 40d);

			var allTrades = series.Run(_strategy).Trades;
			Assert.AreEqual(2, allTrades.Count);
		}

        [Test]
		public void RunOnWholeSeriesWithAmount()
		{
			TimeSeries series = new MockTimeSeries(20d, 40d, 60d, 10d, 30d, 50d, 0d, 20d, 40d);

			var allTrades = series.Run(_strategy,OrderType.Buy, Decimal.Hundred).Trades;
		    Assert.Multiple(() =>
		    {
		        Assert.AreEqual(2, allTrades.Count);
		        Assert.AreEqual(Decimal.Hundred, allTrades[0].Entry.Amount);
		        Assert.AreEqual(Decimal.Hundred, allTrades[1].Entry.Amount);
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
		        Assert.AreEqual(2, trades.Count);

		        Assert.AreEqual(Order.BuyAt(2, slice.GetTick(2).ClosePrice, Decimal.NaNRenamed), trades[0].Entry);
		        Assert.AreEqual(Order.SellAt(4, slice.GetTick(4).ClosePrice, Decimal.NaNRenamed), trades[0].Exit);
		        Assert.AreEqual(Order.BuyAt(6, slice.GetTick(6).ClosePrice, Decimal.NaNRenamed), trades[1].Entry);
		        Assert.AreEqual(Order.SellAt(7, slice.GetTick(7).ClosePrice, Decimal.NaNRenamed), trades[1].Exit);
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
		        Assert.AreEqual(1, trades.Count);

		        Assert.AreEqual(Order.BuyAt(1, slice.GetTick(1).ClosePrice, Decimal.NaNRenamed), trades[0].Entry);
		        Assert.AreEqual(Order.SellAt(3, slice.GetTick(3).ClosePrice, Decimal.NaNRenamed), trades[0].Exit);
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
		        Assert.AreEqual(1, trades.Count);

		        Assert.AreEqual(Order.SellAt(1, slice.GetTick(1).ClosePrice, Decimal.NaNRenamed), trades[0].Entry);
		        Assert.AreEqual(Order.BuyAt(3, slice.GetTick(3).ClosePrice, Decimal.NaNRenamed), trades[0].Exit);
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
		    Assert.AreEqual(1, trades.Count);
		    Assert.AreEqual(Order.BuyAt(2, slice0.GetTick(2).ClosePrice, Decimal.NaNRenamed), trades[0].Entry);
		    Assert.AreEqual(Order.SellAt(4, slice0.GetTick(4).ClosePrice, Decimal.NaNRenamed), trades[0].Exit);

			trades = slice1.Run(_strategy).Trades;
			Assert.IsTrue(trades.Count == 0);

			trades = slice2.Run(_strategy).Trades;
		    Assert.AreEqual(1, trades.Count);
	    	Assert.AreEqual(Order.BuyAt(6, slice2.GetTick(6).ClosePrice, Decimal.NaNRenamed), trades[0].Entry);
    		Assert.AreEqual(Order.SellAt(7, slice2.GetTick(7).ClosePrice, Decimal.NaNRenamed), trades[0].Exit);
		}

        [Test]
		public void Splitted()
		{
			var date = new LocalDateTime();
			TimeSeries series = new MockTimeSeries(new double[]{1d, 2d, 3d, 4d, 5d, 6d, 7d, 8d, 9d, 10d}, new LocalDateTime[]{date.WithYear(2000), date.WithYear(2000), date.WithYear(2001), date.WithYear(2001), date.WithYear(2002), date.WithYear(2002), date.WithYear(2002), date.WithYear(2003), date.WithYear(2004), date.WithYear(2005)});

			var aStrategy = new Strategy(new FixedRule(0, 3, 5, 7), new FixedRule(2, 4, 6, 9));

			var subseries = series.Split(Period.FromYears(1));
			var slice0 = subseries[0];
			var slice1 = subseries[1];
			var slice2 = subseries[2];
			var slice3 = subseries[3];
			var slice4 = subseries[4];
			var slice5 = subseries[5];

			var trades = slice0.Run(aStrategy).Trades;
		    Assert.AreEqual(1, trades.Count);
		    Assert.AreEqual(Order.BuyAt(0, slice0.GetTick(0).ClosePrice, Decimal.NaNRenamed), trades[0].Entry);
		    Assert.AreEqual(Order.SellAt(2, slice0.GetTick(2).ClosePrice, Decimal.NaNRenamed), trades[0].Exit);

			trades = slice1.Run(aStrategy).Trades;
            Assert.AreEqual(1, trades.Count);
		    Assert.AreEqual(Order.BuyAt(3, slice1.GetTick(3).ClosePrice, Decimal.NaNRenamed), trades[0].Entry);
		    Assert.AreEqual(Order.SellAt(4, slice1.GetTick(4).ClosePrice, Decimal.NaNRenamed), trades[0].Exit);
		
			trades = slice2.Run(aStrategy).Trades;
			Assert.AreEqual(1, trades.Count);
			Assert.AreEqual(Order.BuyAt(5, slice2.GetTick(5).ClosePrice, Decimal.NaNRenamed), trades[0].Entry);
			Assert.AreEqual(Order.SellAt(6, slice2.GetTick(6).ClosePrice, Decimal.NaNRenamed), trades[0].Exit);

			trades = slice3.Run(aStrategy).Trades;
		    Assert.AreEqual(1, trades.Count);
		    Assert.AreEqual(Order.BuyAt(7, slice3.GetTick(7).ClosePrice, Decimal.NaNRenamed), trades[0].Entry);
		    Assert.AreEqual(Order.SellAt(9, slice3.GetTick(9).ClosePrice, Decimal.NaNRenamed), trades[0].Exit);
		    
			trades = slice4.Run(aStrategy).Trades;
			Assert.IsTrue(trades.Count == 0);

			trades = slice5.Run(aStrategy).Trades;
			Assert.IsTrue(trades.Count == 0);
		}
	}
}