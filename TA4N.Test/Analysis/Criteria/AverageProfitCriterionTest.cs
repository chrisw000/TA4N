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
using NUnit.Framework;
using TA4N.Analysis.Criteria;
using TA4N.Test.FixtureData;

namespace TA4N.Test.Analysis.Criteria
{
    public sealed class AverageProfitCriterionTest
    {
        [Test]
        public void CalculateOnlyWithGainTrades()
        {
            var _series = GenerateTimeSeries.From(100d, 105d, 110d, 100d, 95d, 105d);
            var tradingRecord = new TradingRecord(Order.BuyAt(0), Order.SellAt(2), Order.BuyAt(3), Order.SellAt(5));
            IAnalysisCriterion averageProfit = new AverageProfitCriterion();
            Assert.That(TaTestsUtils.TaOffset, Is.EqualTo(1.0243).Within(averageProfit.Calculate(_series, tradingRecord)));
        }

        [Test]
        public void CalculateWithASimpleTrade()
        {
            var _series = GenerateTimeSeries.From(100d, 105d, 110d, 100d, 95d, 105d);
            var tradingRecord = new TradingRecord(Order.BuyAt(0), Order.SellAt(2));
            IAnalysisCriterion averageProfit = new AverageProfitCriterion();
            Assert.That(averageProfit.Calculate(_series, tradingRecord), Is.EqualTo(Math.Pow(110d / 100, 1d / 3)).Within(TaTestsUtils.TaOffset));
        }

        [Test]
        public void CalculateOnlyWithLossTrades()
        {
            var _series = GenerateTimeSeries.From(100, 95, 100, 80, 85, 70);
            var tradingRecord = new TradingRecord(Order.BuyAt(0), Order.SellAt(1), Order.BuyAt(2), Order.SellAt(5));
            IAnalysisCriterion averageProfit = new AverageProfitCriterion();
            Assert.That(averageProfit.Calculate(_series, tradingRecord), Is.EqualTo(Math.Pow(95d / 100 * 70d / 100, 1d / 6)).Within(TaTestsUtils.TaOffset));
        }

        [Test]
        public void CalculateWithNoTicksShouldReturn1()
        {
            var _series = GenerateTimeSeries.From(100, 95, 100, 80, 85, 70);
            IAnalysisCriterion averageProfit = new AverageProfitCriterion();
            Assert.That(averageProfit.Calculate(_series, new TradingRecord()), Is.EqualTo(1d).Within(TaTestsUtils.TaOffset));
        }

        [Test]
        public void CalculateWithOneTrade()
        {
            var _series = GenerateTimeSeries.From(100, 105);
            var trade = new Trade(Order.BuyAt(0), Order.SellAt(1));
            IAnalysisCriterion average = new AverageProfitCriterion();
            Assert.That(average.Calculate(_series, trade), Is.EqualTo(Math.Pow(105d / 100, 1d / 2)).Within(TaTestsUtils.TaOffset));
        }

        [Test]
        public void BetterThan()
        {
            IAnalysisCriterion criterion = new AverageProfitCriterion();
            Assert.That(criterion.BetterThan(2.0, 1.5), Is.True);
            Assert.That(criterion.BetterThan(1.5, 2.0), Is.False);
        }
    }
}