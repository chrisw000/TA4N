/// <summary>
/// The MIT License (MIT)
/// 
/// Copyright (c) 2014-2016 Marc de Verdelhan & respective authors (see AUTHORS)
/// 
/// Permission is hereby granted, free of charge, to any person obtaining a copy of
/// this software and associated documentation files (the "Software"), to deal in
/// the Software without restriction, including without limitation the rights to
/// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
/// the Software, and to permit persons to whom the Software is furnished to do so,
/// subject to the following conditions:
/// 
/// The above copyright notice and this permission notice shall be included in all
/// copies or substantial portions of the Software.
/// 
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
/// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
/// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
/// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
/// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
/// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
/// </summary>

using TA4N.Indicators.Trackers;

namespace TA4N.Test.Indicators
{  
	using TA4N.Indicators.Simple;
	using TA4N.Mocks;
	using OverIndicatorRule = TA4N.Trading.Rules.OverIndicatorRule;
	using UnderIndicatorRule = TA4N.Trading.Rules.UnderIndicatorRule;
    using NUnit.Framework;

	public sealed class CachedIndicatorTest
	{
        private TimeSeries _series;
        
        [SetUp]
		public void SetUp()
		{
			_series = new MockTimeSeries(1, 2, 3, 4, 3, 4, 5, 4, 3, 3, 4, 3, 2);
		}


        [Test]
		public void IfCacheWorks()
		{
			var sma = new SmaIndicator(new ClosePriceIndicator(_series), 3);
			var firstTime = sma.GetValue(4);
			var secondTime = sma.GetValue(4);
			Assert.AreEqual(firstTime, secondTime);
		}


        [Test]
		public void GetValueWithNullTimeSeries()
		{
			var constant = new ConstantIndicator<Decimal>(Decimal.Ten);
			Assert.AreEqual(Decimal.Ten, constant.GetValue(0));
			Assert.AreEqual(Decimal.Ten, constant.GetValue(100));
			Assert.IsNull(constant.TimeSeries);

			var sma = new SmaIndicator(constant, 10);
			Assert.AreEqual(Decimal.Ten, sma.GetValue(0));
			Assert.AreEqual(Decimal.Ten, sma.GetValue(100));
			Assert.IsNull(sma.TimeSeries);
		}

        [Test]
		public void GetValueWithCacheLengthIncrease()
		{
			var data = new double[200];

			TaTestsUtils.ArraysFill(data, 10);
			var sma = new SmaIndicator(new ClosePriceIndicator(new MockTimeSeries(data)), 100);
			TaTestsUtils.AssertDecimalEquals(sma.GetValue(105), 10);
		}
        
        [Test]
		public void GetValueWithOldResultsRemoval()
		{
			var data = new double[20];
            TaTestsUtils.ArraysFill(data, 1);
            TimeSeries timeSeries = new MockTimeSeries(data);
			var sma = new SmaIndicator(new ClosePriceIndicator(timeSeries), 10);
			TaTestsUtils.AssertDecimalEquals(sma.GetValue(5), 1);
			TaTestsUtils.AssertDecimalEquals(sma.GetValue(10), 1);
			timeSeries.MaximumTickCount = 12;
			TaTestsUtils.AssertDecimalEquals(sma.GetValue(19), 1);
		}
        
        [Test]
		public void StrategyExecutionOnCachedIndicatorAndLimitedTimeSeries()
		{
			TimeSeries timeSeries = new MockTimeSeries(0, 1, 2, 3, 4, 5, 6, 7);
			var sma = new SmaIndicator(new ClosePriceIndicator(timeSeries), 2);
			// Theoretical values for SMA(2) cache: 0, 0.5, 1.5, 2.5, 3.5, 4.5, 5.5, 6.5
			timeSeries.MaximumTickCount = 6;
			// Theoretical values for SMA(2) cache: null, null, 2, 2.5, 3.5, 4.5, 5.5, 6.5

			var strategy = new Strategy(new OverIndicatorRule(sma, Decimal.Three), new UnderIndicatorRule(sma, Decimal.Three)
		   );
			// Theoretical shouldEnter results: false, false, false, false, true, true, true, true
			// Theoretical shouldExit results: false, false, true, true, false, false, false, false

			// As we return the first tick/result found for the removed ticks:
			// -> Approximated values for ClosePrice cache: 2, 2, 2, 3, 4, 5, 6, 7
			// -> Approximated values for SMA(2) cache: 2, 2, 2, 2.5, 3.5, 4.5, 5.5, 6.5

			// Then enters/exits are also approximated:
			// -> shouldEnter results: false, false, false, false, true, true, true, true
			// -> shouldExit results: true, true, true, true, false, false, false, false

			Assert.IsFalse(strategy.ShouldEnter(0));
			Assert.IsTrue(strategy.ShouldExit(0));
			Assert.IsFalse(strategy.ShouldEnter(1));
			Assert.IsTrue(strategy.ShouldExit(1));
			Assert.IsFalse(strategy.ShouldEnter(2));
			Assert.IsTrue(strategy.ShouldExit(2));
			Assert.IsFalse(strategy.ShouldEnter(3));
			Assert.IsTrue(strategy.ShouldExit(3));
			Assert.IsTrue(strategy.ShouldEnter(4));
			Assert.IsFalse(strategy.ShouldExit(4));
			Assert.IsTrue(strategy.ShouldEnter(5));
			Assert.IsFalse(strategy.ShouldExit(5));
			Assert.IsTrue(strategy.ShouldEnter(6));
			Assert.IsFalse(strategy.ShouldExit(6));
			Assert.IsTrue(strategy.ShouldEnter(7));
			Assert.IsFalse(strategy.ShouldExit(7));
		}

        [Test] 
		public void GetValueOnResultsCalculatedFromRemovedTicksShouldReturnFirstRemainingResult()
		{
			TimeSeries timeSeries = new MockTimeSeries(1, 1, 1, 1, 1);
			timeSeries.MaximumTickCount = 3;
			Assert.AreEqual(2, timeSeries.RemovedTicksCount);

			var sma = new SmaIndicator(new ClosePriceIndicator(timeSeries), 2);
			for (var i = 0; i < 5; i++)
			{
				TaTestsUtils.AssertDecimalEquals(sma.GetValue(i), 1);
			}
		}
	}
}