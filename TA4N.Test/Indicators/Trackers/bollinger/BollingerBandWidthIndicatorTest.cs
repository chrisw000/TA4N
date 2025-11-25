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
    public sealed class BollingerBandWidthIndicatorTest
    {
        private TimeSeries _data;
        private ClosePriceIndicator _closePrice;

        [SetUp]
        public void SetUp()
        {
            _data = GenerateTimeSeries.From(10, 12, 15, 14, 17, 20, 21, 20, 20, 19, 20, 17, 12, 12, 9, 8, 9, 10, 9, 10);
            _closePrice = new ClosePriceIndicator(_data);
        }

        [Test]
        public void BollingerBandWidthUsingSmaAndStandardDeviation()
        {
            var sma = new SmaIndicator(_closePrice, 5);
            var standardDeviation = new StandardDeviationIndicator(_closePrice, 5);
            var bbmSma = new BollingerBandsMiddleIndicator(sma);
            var bbuSma = new BollingerBandsUpperIndicator(bbmSma, standardDeviation);
            var bblSma = new BollingerBandsLowerIndicator(bbmSma, standardDeviation);
            var bandwidth = new BollingerBandWidthIndicator(bbuSma, bbmSma, bblSma);
            Assert.That(bandwidth.GetValue(0).ToDouble(), Is.EqualTo(0.0).Within(TaTestsUtils.TaOffset));
            Assert.That(bandwidth.GetValue(1).ToDouble(), Is.EqualTo(36.3636).Within(TaTestsUtils.TaOffset));
            Assert.That(bandwidth.GetValue(2).ToDouble(), Is.EqualTo(66.6423).Within(TaTestsUtils.TaOffset));
            Assert.That(bandwidth.GetValue(3).ToDouble(), Is.EqualTo(60.2443).Within(TaTestsUtils.TaOffset));
            Assert.That(bandwidth.GetValue(4).ToDouble(), Is.EqualTo(71.0767).Within(TaTestsUtils.TaOffset));
            Assert.That(bandwidth.GetValue(5).ToDouble(), Is.EqualTo(69.9394).Within(TaTestsUtils.TaOffset));
            Assert.That(bandwidth.GetValue(6).ToDouble(), Is.EqualTo(62.7043).Within(TaTestsUtils.TaOffset));
            Assert.That(bandwidth.GetValue(7).ToDouble(), Is.EqualTo(56.0178).Within(TaTestsUtils.TaOffset));
            Assert.That(bandwidth.GetValue(8).ToDouble(), Is.EqualTo(27.683).Within(TaTestsUtils.TaOffset));
            Assert.That(bandwidth.GetValue(9).ToDouble(), Is.EqualTo(12.6491).Within(TaTestsUtils.TaOffset));
            Assert.That(bandwidth.GetValue(10).ToDouble(), Is.EqualTo(12.6491).Within(TaTestsUtils.TaOffset));
            Assert.That(bandwidth.GetValue(11).ToDouble(), Is.EqualTo(24.2956).Within(TaTestsUtils.TaOffset));
            Assert.That(bandwidth.GetValue(12).ToDouble(), Is.EqualTo(68.3332).Within(TaTestsUtils.TaOffset));
            Assert.That(bandwidth.GetValue(13).ToDouble(), Is.EqualTo(85.1469).Within(TaTestsUtils.TaOffset));
            Assert.That(bandwidth.GetValue(14).ToDouble(), Is.EqualTo(112.8481).Within(TaTestsUtils.TaOffset));
            Assert.That(bandwidth.GetValue(15).ToDouble(), Is.EqualTo(108.1682).Within(TaTestsUtils.TaOffset));
            Assert.That(bandwidth.GetValue(16).ToDouble(), Is.EqualTo(66.9328).Within(TaTestsUtils.TaOffset));
            Assert.That(bandwidth.GetValue(17).ToDouble(), Is.EqualTo(56.5194).Within(TaTestsUtils.TaOffset));
            Assert.That(bandwidth.GetValue(18).ToDouble(), Is.EqualTo(28.1091).Within(TaTestsUtils.TaOffset));
            Assert.That(bandwidth.GetValue(19).ToDouble(), Is.EqualTo(32.5362).Within(TaTestsUtils.TaOffset));
        }
    }
}