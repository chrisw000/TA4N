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
    public sealed class NumberOfTradesCriterionTest
    {
        [Test]
        public void CalculateWithNoTrades()
        {
            var series = GenerateTimeSeries.From(100, 105, 110, 100, 95, 105);
            IAnalysisCriterion buyAndHold = new NumberOfTradesCriterion();
            Assert.That(buyAndHold.Calculate(series, new TradingRecord()), Is.EqualTo(0d).Within(TaTestsUtils.TaOffset));
        }

        [Test]
        public void CalculateWithTwoTrades()
        {
            var series = GenerateTimeSeries.From(100, 105, 110, 100, 95, 105);
            var tradingRecord = new TradingRecord(Order.BuyAt(0), Order.SellAt(2), Order.BuyAt(3), Order.SellAt(5));
            IAnalysisCriterion buyAndHold = new NumberOfTradesCriterion();
            Assert.That(buyAndHold.Calculate(series, tradingRecord), Is.EqualTo(2d).Within(TaTestsUtils.TaOffset));
        }

        [Test]
        public void CalculateWithOneTrade()
        {
            var trade = new Trade();
            var tradesCriterion = new NumberOfTradesCriterion();
            Assert.That(tradesCriterion.Calculate(null, trade), Is.EqualTo(1d).Within(TaTestsUtils.TaOffset));
        }

        [Test]
        public void BetterThan()
        {
            IAnalysisCriterion criterion = new NumberOfTradesCriterion();
            Assert.That(criterion.BetterThan(3, 6), Is.True);
            Assert.That(criterion.BetterThan(7, 4), Is.False);
        }
    }
}