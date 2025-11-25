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
using NUnit.Framework;
using TA4N.Indicators.Simple;
using TA4N.Indicators.Statistics;
using TA4N.Indicators.Trackers;
using TA4N.Indicators.Trackers.Bollinger;
using TA4N.Test.FixtureData;

namespace TA4N.Test.Indicators.Trackers.Bollinger
{
    public sealed class BollingerBandsLowerIndicatorTest
    {
        private TimeSeries _data;
        private int _timeFrame;
        private ClosePriceIndicator _closePrice;
        private SmaIndicator _sma;

        [SetUp]
        public void SetUp()
        {
            _data = GenerateTimeSeries.From(1, 2, 3, 4, 3, 4, 5, 4, 3, 3, 4, 3, 2);
            _timeFrame = 3;
            _closePrice = new ClosePriceIndicator(_data);
            _sma = new SmaIndicator(_closePrice, _timeFrame);
        }

        [Test]
        public void BollingerBandsLowerUsingSmaAndStandardDeviation()
        {
            var bbmSma = new BollingerBandsMiddleIndicator(_sma);
            var standardDeviation = new StandardDeviationIndicator(_closePrice, _timeFrame);
            var bblSma = new BollingerBandsLowerIndicator(bbmSma, standardDeviation);
            Assert.That(bblSma.K.ToDouble(), Is.EqualTo(2));
            Assert.That(bblSma.GetValue(0), Is.EqualTo(Decimal.ValueOf(1)));
            Assert.That(bblSma.GetValue(1).ToDouble(), Is.EqualTo(0.5).Within(TaTestsUtils.TaOffset));
            Assert.That(bblSma.GetValue(2).ToDouble(), Is.EqualTo(0.367).Within(TaTestsUtils.TaOffset));
            Assert.That(bblSma.GetValue(3).ToDouble(), Is.EqualTo(1.367).Within(TaTestsUtils.TaOffset));
            Assert.That(bblSma.GetValue(4).ToDouble(), Is.EqualTo(2.3905).Within(TaTestsUtils.TaOffset));
            Assert.That(bblSma.GetValue(5).ToDouble(), Is.EqualTo(2.7239).Within(TaTestsUtils.TaOffset));
            Assert.That(bblSma.GetValue(6).ToDouble(), Is.EqualTo(2.367).Within(TaTestsUtils.TaOffset));
            var bblSmAwithK = new BollingerBandsLowerIndicator(bbmSma, standardDeviation, Decimal.ValueOf("1.5"));
            Assert.That(bblSmAwithK.K.ToDouble(), Is.EqualTo(1.5));
            Assert.That(bblSmAwithK.GetValue(0), Is.EqualTo(Decimal.ValueOf(1)));
            Assert.That(bblSmAwithK.GetValue(1).ToDouble(), Is.EqualTo(0.75).Within(TaTestsUtils.TaOffset));
            Assert.That(bblSmAwithK.GetValue(2).ToDouble(), Is.EqualTo(0.7752).Within(TaTestsUtils.TaOffset));
            Assert.That(bblSmAwithK.GetValue(3).ToDouble(), Is.EqualTo(1.7752).Within(TaTestsUtils.TaOffset));
            Assert.That(bblSmAwithK.GetValue(4).ToDouble(), Is.EqualTo(2.6262).Within(TaTestsUtils.TaOffset));
            Assert.That(bblSmAwithK.GetValue(5).ToDouble(), Is.EqualTo(2.9595).Within(TaTestsUtils.TaOffset));
            Assert.That(bblSmAwithK.GetValue(6).ToDouble(), Is.EqualTo(2.7752).Within(TaTestsUtils.TaOffset));
        }
    }
}