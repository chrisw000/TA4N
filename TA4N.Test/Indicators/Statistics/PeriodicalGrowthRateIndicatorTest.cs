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
using TA4N.Test.FixtureData;

namespace TA4N.Test.Indicators.Statistics
{
    using TA4N.Indicators.Simple;
    using CrossedDownIndicatorRule = TA4N.Trading.Rules.CrossedDownIndicatorRule;
    using CrossedUpIndicatorRule = TA4N.Trading.Rules.CrossedUpIndicatorRule;
    using NUnit.Framework;
    using TA4N.Indicators.Statistics;

    public sealed class PeriodicalGrowthRateIndicatorTest
    {
        private TimeSeries _mockdata;
        private ClosePriceIndicator _closePrice;

        [SetUp]
        public void SetUp()
        {
            _mockdata = GenerateTimeSeries.From(29.49, 28.30, 27.74, 27.65, 27.60, 28.70, 28.60, 28.19, 27.40, 27.20, 27.28, 27.00, 27.59, 26.20, 25.75, 24.75, 23.33, 24.45, 24.25, 25.02, 23.60, 24.20, 24.28, 25.70, 25.46, 25.10, 25.00, 25.00, 25.85);
            _closePrice = new ClosePriceIndicator(_mockdata);
        }

        [Test]
        public void TestGetTotalReturn()
        {
            var gri = new PeriodicalGrowthRateIndicator(this._closePrice, 5);
            var expResult = 0.9564;
            var result = gri.TotalReturn;
            Assert.That(result, Is.EqualTo(expResult).Within(0.01));
        }

        [Test]
        public void TestCalculation()
        {
            var gri = new PeriodicalGrowthRateIndicator(this._closePrice, 5);
            Assert.That(Decimal.NaNRenamed, Is.EqualTo(gri.GetValue(0)));
            Assert.That(Decimal.NaNRenamed, Is.EqualTo(gri.GetValue(4)));
            Assert.That(gri.GetValue(5).ToDouble(), Is.EqualTo(-0.0268).Within(TaTestsUtils.TaOffset));
            Assert.That(gri.GetValue(6).ToDouble(), Is.EqualTo(0.0541).Within(TaTestsUtils.TaOffset));
            Assert.That(gri.GetValue(10).ToDouble(), Is.EqualTo(-0.0495).Within(TaTestsUtils.TaOffset));
            Assert.That(gri.GetValue(21).ToDouble(), Is.EqualTo(0.2009).Within(TaTestsUtils.TaOffset));
            Assert.That(gri.GetValue(24).ToDouble(), Is.EqualTo(0.0220).Within(TaTestsUtils.TaOffset));
            Assert.That(Decimal.NaNRenamed, Is.EqualTo(gri.GetValue(25)));
            Assert.That(Decimal.NaNRenamed, Is.EqualTo(gri.GetValue(26)));
        }

        [Test]
        public void TestStrategies()
        {
            var gri = new PeriodicalGrowthRateIndicator(this._closePrice, 5);
            // Rules
            IRule buyingRule = new CrossedUpIndicatorRule(gri, Decimal.Zero);
            IRule sellingRule = new CrossedDownIndicatorRule(gri, Decimal.Zero);
            var strategy = new Strategy(buyingRule, sellingRule);
            // Check trades
            var result = _mockdata.Run(strategy).TradeCount;
            var expResult = 3;
            Assert.That(result, Is.EqualTo(expResult));
        }
    }
}