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
    using TA4N.Indicators.Statistics;
    using NUnit.Framework;

    public sealed class StandardErrorIndicatorTest
    {
        private TimeSeries _data;

        [SetUp]
        public void SetUp()
        {
            _data = GenerateTimeSeries.From(10, 20, 30, 40, 50, 40, 40, 50, 40, 30, 20, 10);
        }

        [Test]
        public void UsingTimeFrame5UsingClosePrice()
        {
            var se = new StandardErrorIndicator(new ClosePriceIndicator(_data), 5);
            Assert.That(se.GetValue(0), Is.EqualTo(Decimal.ValueOf(0)));
            Assert.That(se.GetValue(1).ToDouble(), Is.EqualTo(3.5355).Within(TaTestsUtils.TaOffset));
            Assert.That(se.GetValue(2).ToDouble(), Is.EqualTo(4.714).Within(TaTestsUtils.TaOffset));
            Assert.That(se.GetValue(3).ToDouble(), Is.EqualTo(5.5902).Within(TaTestsUtils.TaOffset));
            Assert.That(se.GetValue(4).ToDouble(), Is.EqualTo(6.3246).Within(TaTestsUtils.TaOffset));
            Assert.That(se.GetValue(5).ToDouble(), Is.EqualTo(4.5607).Within(TaTestsUtils.TaOffset));
            Assert.That(se.GetValue(6).ToDouble(), Is.EqualTo(2.8284).Within(TaTestsUtils.TaOffset));
            Assert.That(se.GetValue(7).ToDouble(), Is.EqualTo(2.1909).Within(TaTestsUtils.TaOffset));
            Assert.That(se.GetValue(8).ToDouble(), Is.EqualTo(2.1909).Within(TaTestsUtils.TaOffset));
            Assert.That(se.GetValue(9).ToDouble(), Is.EqualTo(2.8284).Within(TaTestsUtils.TaOffset));
            Assert.That(se.GetValue(10).ToDouble(), Is.EqualTo(4.5607).Within(TaTestsUtils.TaOffset));
            Assert.That(se.GetValue(11).ToDouble(), Is.EqualTo(6.3246).Within(TaTestsUtils.TaOffset));
        }

        [Test]
        public void ShouldBeZeroWhenTimeFrameIs1()
        {
            var se = new StandardErrorIndicator(new ClosePriceIndicator(_data), 1);
            Assert.That(se.GetValue(1), Is.EqualTo(Decimal.ValueOf(0)));
            Assert.That(se.GetValue(3), Is.EqualTo(Decimal.ValueOf(0)));
        }
    }
}