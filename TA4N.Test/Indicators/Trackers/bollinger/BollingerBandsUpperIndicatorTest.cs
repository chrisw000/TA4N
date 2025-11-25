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
    public sealed class BollingerBandsUpperIndicatorTest
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
        public void BollingerBandsUpperUsingSmaAndStandardDeviation()
        {
            var bbmSma = new BollingerBandsMiddleIndicator(_sma);
            var standardDeviation = new StandardDeviationIndicator(_closePrice, _timeFrame);
            var bbuSma = new BollingerBandsUpperIndicator(bbmSma, standardDeviation);
            Assert.That(bbuSma.K.ToDouble(), Is.EqualTo(2));
            Assert.That(bbuSma.GetValue(0), Is.EqualTo(Decimal.ValueOf(1)));
            Assert.That(bbuSma.GetValue(1).ToDouble(), Is.EqualTo(2.5).Within(TaTestsUtils.TaOffset));
            Assert.That(bbuSma.GetValue(2).ToDouble(), Is.EqualTo(3.633).Within(TaTestsUtils.TaOffset));
            Assert.That(bbuSma.GetValue(3).ToDouble(), Is.EqualTo(4.633).Within(TaTestsUtils.TaOffset));
            Assert.That(bbuSma.GetValue(4).ToDouble(), Is.EqualTo(4.2761).Within(TaTestsUtils.TaOffset));
            Assert.That(bbuSma.GetValue(5).ToDouble(), Is.EqualTo(4.6094).Within(TaTestsUtils.TaOffset));
            Assert.That(bbuSma.GetValue(6).ToDouble(), Is.EqualTo(5.633).Within(TaTestsUtils.TaOffset));
            Assert.That(bbuSma.GetValue(7).ToDouble(), Is.EqualTo(5.2761).Within(TaTestsUtils.TaOffset));
            Assert.That(bbuSma.GetValue(8).ToDouble(), Is.EqualTo(5.633).Within(TaTestsUtils.TaOffset));
            Assert.That(bbuSma.GetValue(9).ToDouble(), Is.EqualTo(4.2761).Within(TaTestsUtils.TaOffset));
            var bbuSmAwithK = new BollingerBandsUpperIndicator(bbmSma, standardDeviation, Decimal.ValueOf("1.5"));
            Assert.That(bbuSmAwithK.K.ToDouble(), Is.EqualTo(1.5));
            Assert.That(bbuSmAwithK.GetValue(0), Is.EqualTo(Decimal.ValueOf(1)));
            Assert.That(bbuSmAwithK.GetValue(1).ToDouble(), Is.EqualTo(2.25).Within(TaTestsUtils.TaOffset));
            Assert.That(bbuSmAwithK.GetValue(2).ToDouble(), Is.EqualTo(3.2247).Within(TaTestsUtils.TaOffset));
            Assert.That(bbuSmAwithK.GetValue(3).ToDouble(), Is.EqualTo(4.2247).Within(TaTestsUtils.TaOffset));
            Assert.That(bbuSmAwithK.GetValue(4).ToDouble(), Is.EqualTo(4.0404).Within(TaTestsUtils.TaOffset));
            Assert.That(bbuSmAwithK.GetValue(5).ToDouble(), Is.EqualTo(4.3737).Within(TaTestsUtils.TaOffset));
            Assert.That(bbuSmAwithK.GetValue(6).ToDouble(), Is.EqualTo(5.2247).Within(TaTestsUtils.TaOffset));
            Assert.That(bbuSmAwithK.GetValue(7).ToDouble(), Is.EqualTo(5.0404).Within(TaTestsUtils.TaOffset));
            Assert.That(bbuSmAwithK.GetValue(8).ToDouble(), Is.EqualTo(5.2247).Within(TaTestsUtils.TaOffset));
            Assert.That(bbuSmAwithK.GetValue(9).ToDouble(), Is.EqualTo(4.0404).Within(TaTestsUtils.TaOffset));
        }
    }
}