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
using NUnit.Framework;
using TA4N.Analysis.Criteria;
using TA4N.Test.FixtureData;

namespace TA4N.Test.Analysis.Criteria
{
    public sealed class AverageProfitableTradesCriterionTest
    {
        [Test]
        public void Calculate()
        {
            TimeSeries series = GenerateTimeSeries.From(100d, 95d, 102d, 105d, 97d, 113d);
            var tradingRecord = new TradingRecord(Order.BuyAt(0), Order.SellAt(1), Order.BuyAt(2), Order.SellAt(3), Order.BuyAt(4), Order.SellAt(5));
            var average = new AverageProfitableTradesCriterion();
            Assert.That(average.Calculate(series, tradingRecord), Is.EqualTo(2d / 3).Within(TaTestsUtils.TaOffset));
        }

        [Test]
        public void CalculateWithOneTrade()
        {
            TimeSeries series = GenerateTimeSeries.From(100d, 95d, 102d, 105d, 97d, 113d);
            var trade = new Trade(Order.BuyAt(0), Order.SellAt(1));
            var average = new AverageProfitableTradesCriterion();
            Assert.That(average.Calculate(series, trade), Is.EqualTo(0d).Within(TaTestsUtils.TaOffset));
            trade = new Trade(Order.BuyAt(1), Order.SellAt(2));
            Assert.That(average.Calculate(series, trade), Is.EqualTo(1d).Within(TaTestsUtils.TaOffset));
        }

        [Test]
        public void BetterThan()
        {
            IAnalysisCriterion criterion = new AverageProfitableTradesCriterion();
            Assert.That(criterion.BetterThan(12, 8), Is.True);
            Assert.That(criterion.BetterThan(8, 12), Is.False);
        }
    }
}