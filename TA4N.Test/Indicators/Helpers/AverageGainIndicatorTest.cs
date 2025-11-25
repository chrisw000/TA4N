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

namespace TA4N.Test.Indicators.Helpers
{
    using TA4N.Indicators.Simple;
    using TA4N.Indicators.Helpers;
    using NUnit.Framework;

    public sealed class AverageGainIndicatorTest
    {
        private TimeSeries _data;

        [SetUp]
        public void SetUp()
        {
            _data = GenerateTimeSeries.From(1, 2, 3, 4, 3, 4, 5, 4, 3, 3, 4, 3, 2);
        }

        [Test]
        public void AverageGainUsingTimeFrame5UsingClosePrice()
        {
            var averageGain = new AverageGainIndicator(new ClosePriceIndicator(_data), 5);
            Assert.That(averageGain.GetValue(5), Is.EqualTo(Decimal.ValueOf("0.8")));
            Assert.That(averageGain.GetValue(6), Is.EqualTo(Decimal.ValueOf("0.8")));
            Assert.That(averageGain.GetValue(7), Is.EqualTo(Decimal.ValueOf("0.6")));
            Assert.That(averageGain.GetValue(8), Is.EqualTo(Decimal.ValueOf("0.4")));
            Assert.That(averageGain.GetValue(9), Is.EqualTo(Decimal.ValueOf("0.4")));
            Assert.That(averageGain.GetValue(10), Is.EqualTo(Decimal.ValueOf("0.4")));
            Assert.That(averageGain.GetValue(11), Is.EqualTo(Decimal.ValueOf("0.2")));
            Assert.That(averageGain.GetValue(12), Is.EqualTo(Decimal.ValueOf("0.2")));
        }

        [Test]
        public void AverageGainMustReturnZeroWhenTheDataDoesntGain()
        {
            var averageGain = new AverageGainIndicator(new ClosePriceIndicator(_data), 3);
            Assert.That(averageGain.GetValue(9), Is.EqualTo(Decimal.ValueOf(0)));
        }

        [Test]
        public void AverageGainWhenTimeFrameIsGreaterThanIndicatorDataShouldBeCalculatedWithDataSize()
        {
            var averageGain = new AverageGainIndicator(new ClosePriceIndicator(_data), 1000);
            Assert.That(averageGain.GetValue(12).ToDouble(), Is.EqualTo(6d / _data.TickCount).Within(TaTestsUtils.TaOffset));
        }

        [Test]
        public void AverageGainWhenIndexIsZeroMustBeZero()
        {
            var averageGain = new AverageGainIndicator(new ClosePriceIndicator(_data), 10);
            Assert.That(averageGain.GetValue(0), Is.EqualTo(Decimal.ValueOf(0)));
        }
    }
}