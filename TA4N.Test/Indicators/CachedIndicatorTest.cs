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
using TA4N.Test.FixtureData;

namespace TA4N.Test.Indicators
{
    using TA4N.Indicators.Simple;
    using OverIndicatorRule = TA4N.Trading.Rules.OverIndicatorRule;
    using UnderIndicatorRule = TA4N.Trading.Rules.UnderIndicatorRule;
    using NUnit.Framework;

    public sealed class CachedIndicatorTest
    {
        private TimeSeries _series;

        [SetUp]
        public void SetUp()
        {
            _series = GenerateTimeSeries.From(1, 2, 3, 4, 3, 4, 5, 4, 3, 3, 4, 3, 2);
        }

        [Test]
        public void IfCacheWorks()
        {
            var sma = new SmaIndicator(new ClosePriceIndicator(_series), 3);
            var firstTime = sma.GetValue(4);
            var secondTime = sma.GetValue(4);
            Assert.That(secondTime, Is.EqualTo(firstTime));
        }

        [Test]
        public void GetValueWithNullTimeSeries()
        {
            var constant = new ConstantIndicator<Decimal>(Decimal.Ten);
            Assert.That(constant.GetValue(0), Is.EqualTo(Decimal.Ten));
            Assert.That(constant.GetValue(100), Is.EqualTo(Decimal.Ten));
            Assert.That(constant.TimeSeries, Is.Null);
            var sma = new SmaIndicator(constant, 10);
            Assert.That(sma.GetValue(0), Is.EqualTo(Decimal.Ten));
            Assert.That(sma.GetValue(100), Is.EqualTo(Decimal.Ten));
            Assert.That(sma.TimeSeries, Is.Null);
        }

        [Test]
        public void GetValueWithCacheLengthIncrease()
        {
            var data = new double[200];
            TaTestsUtils.ArraysFill(data, 10);
            var sma = new SmaIndicator(new ClosePriceIndicator(GenerateTimeSeries.From(data)), 100);
            Assert.That(sma.GetValue(105), Is.EqualTo(Decimal.ValueOf(10)));
        }

        [Test]
        public void GetValueWithOldResultsRemoval()
        {
            var data = new double[20];
            TaTestsUtils.ArraysFill(data, 1);
            TimeSeries timeSeries = GenerateTimeSeries.From(data);
            var sma = new SmaIndicator(new ClosePriceIndicator(timeSeries), 10);
            Assert.That(sma.GetValue(5), Is.EqualTo(Decimal.ValueOf(1)));
            Assert.That(sma.GetValue(10), Is.EqualTo(Decimal.ValueOf(1)));
            timeSeries.MaximumTickCount = 12;
            Assert.That(sma.GetValue(19), Is.EqualTo(Decimal.ValueOf(1)));
        }

        [Test]
        public void StrategyExecutionOnCachedIndicatorAndLimitedTimeSeries()
        {
            TimeSeries timeSeries = GenerateTimeSeries.From(0, 1, 2, 3, 4, 5, 6, 7);
            var sma = new SmaIndicator(new ClosePriceIndicator(timeSeries), 2);
            // Theoretical values for SMA(2) cache: 0, 0.5, 1.5, 2.5, 3.5, 4.5, 5.5, 6.5
            timeSeries.MaximumTickCount = 6;
            // Theoretical values for SMA(2) cache: null, null, 2, 2.5, 3.5, 4.5, 5.5, 6.5
            var strategy = new Strategy(new OverIndicatorRule(sma, Decimal.Three), new UnderIndicatorRule(sma, Decimal.Three));
            // Theoretical shouldEnter results: false, false, false, false, true, true, true, true
            // Theoretical shouldExit results: false, false, true, true, false, false, false, false
            // As we return the first tick/result found for the removed ticks:
            // -> Approximated values for ClosePrice cache: 2, 2, 2, 3, 4, 5, 6, 7
            // -> Approximated values for SMA(2) cache: 2, 2, 2, 2.5, 3.5, 4.5, 5.5, 6.5
            // Then enters/exits are also approximated:
            // -> shouldEnter results: false, false, false, false, true, true, true, true
            // -> shouldExit results: true, true, true, true, false, false, false, false
            Assert.That(strategy.ShouldEnter(0), Is.False);
            Assert.That(strategy.ShouldExit(0), Is.True);
            Assert.That(strategy.ShouldEnter(1), Is.False);
            Assert.That(strategy.ShouldExit(1), Is.True);
            Assert.That(strategy.ShouldEnter(2), Is.False);
            Assert.That(strategy.ShouldExit(2), Is.True);
            Assert.That(strategy.ShouldEnter(3), Is.False);
            Assert.That(strategy.ShouldExit(3), Is.True);
            Assert.That(strategy.ShouldEnter(4), Is.True);
            Assert.That(strategy.ShouldExit(4), Is.False);
            Assert.That(strategy.ShouldEnter(5), Is.True);
            Assert.That(strategy.ShouldExit(5), Is.False);
            Assert.That(strategy.ShouldEnter(6), Is.True);
            Assert.That(strategy.ShouldExit(6), Is.False);
            Assert.That(strategy.ShouldEnter(7), Is.True);
            Assert.That(strategy.ShouldExit(7), Is.False);
        }

        [Test]
        public void GetValueOnResultsCalculatedFromRemovedTicksShouldReturnFirstRemainingResult()
        {
            TimeSeries timeSeries = GenerateTimeSeries.From(1, 1, 1, 1, 1);
            timeSeries.MaximumTickCount = 3;
            Assert.That(timeSeries.RemovedTicksCount, Is.EqualTo(2));
            var sma = new SmaIndicator(new ClosePriceIndicator(timeSeries), 2);
            for (var i = 0; i < 5; i++)
            {
                Assert.That(sma.GetValue(i), Is.EqualTo(Decimal.ValueOf(1)));
            }
        }
    }
}