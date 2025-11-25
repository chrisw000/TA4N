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

namespace TA4N.Test.Indicators.Trackers
{
    using NUnit.Framework;
    using TA4N.Indicators.Simple;
    using TA4N.Indicators.Trackers;

    public sealed class RaviIndicatorTest
    {
        private TimeSeries _data;

        [SetUp]
        public void SetUp()
        {
            _data = GenerateTimeSeries.From(110.00, 109.27, 104.69, 107.07, 107.92, 107.95, 108.70, 107.97, 106.09, 106.03, 108.65, 109.54, 112.26, 114.38, 117.94);
        }

        [Test]
        public void Ravi()
        {
            var closePrice = new ClosePriceIndicator(_data);
            var ravi = new RaviIndicator(closePrice, 3, 8);
            Assert.That(ravi.GetValue(0), Is.EqualTo(Decimal.ValueOf(0)));
            Assert.That(ravi.GetValue(1), Is.EqualTo(Decimal.ValueOf(0)));
            Assert.That(ravi.GetValue(2), Is.EqualTo(Decimal.ValueOf(0)));
            Assert.That(ravi.GetValue(3).ToDouble(), Is.EqualTo(-0.6937).Within(TaTestsUtils.TaOffset));
            Assert.That(ravi.GetValue(4).ToDouble(), Is.EqualTo(-1.1411).Within(TaTestsUtils.TaOffset));
            Assert.That(ravi.GetValue(5).ToDouble(), Is.EqualTo(-0.1577).Within(TaTestsUtils.TaOffset));
            Assert.That(ravi.GetValue(6).ToDouble(), Is.EqualTo(0.229).Within(TaTestsUtils.TaOffset));
            Assert.That(ravi.GetValue(7).ToDouble(), Is.EqualTo(0.2412).Within(TaTestsUtils.TaOffset));
            Assert.That(ravi.GetValue(8).ToDouble(), Is.EqualTo(0.1202).Within(TaTestsUtils.TaOffset));
            Assert.That(ravi.GetValue(9).ToDouble(), Is.EqualTo(-0.3324).Within(TaTestsUtils.TaOffset));
            Assert.That(ravi.GetValue(10).ToDouble(), Is.EqualTo(-0.5804).Within(TaTestsUtils.TaOffset));
            Assert.That(ravi.GetValue(11).ToDouble(), Is.EqualTo(0.2013).Within(TaTestsUtils.TaOffset));
            Assert.That(ravi.GetValue(12).ToDouble(), Is.EqualTo(1.6156).Within(TaTestsUtils.TaOffset));
            Assert.That(ravi.GetValue(13).ToDouble(), Is.EqualTo(2.6167).Within(TaTestsUtils.TaOffset));
            Assert.That(ravi.GetValue(14).ToDouble(), Is.EqualTo(4.0799).Within(TaTestsUtils.TaOffset));
        }
    }
}