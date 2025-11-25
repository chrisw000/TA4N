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
    using TA4N.Indicators.Simple;
    using NUnit.Framework;
    using TA4N.Indicators.Trackers;

    public sealed class TrailingStopLossIndicatorTest
    {
        private TimeSeries _data;

        [SetUp]
        public void SetUp()
        {
            _data = GenerateTimeSeries.From(18, 19, 23, 22, 21, 22, 17, 18, 21, 25, 26, 29, 29, 28, 29, 26, 25, 26, 26, 28);
        }

        [Test]
        public void WithoutInitialLimitUsingClosePrice()
        {
            var price = new ClosePriceIndicator(_data);
            var tsl = new TrailingStopLossIndicator(price, Decimal.ValueOf(4));
            Assert.That(tsl.GetValue(1), Is.EqualTo(Decimal.ValueOf(15)));
            Assert.That(tsl.GetValue(2), Is.EqualTo(Decimal.ValueOf(19)));
            Assert.That(tsl.GetValue(3), Is.EqualTo(Decimal.ValueOf(19)));
            Assert.That(tsl.GetValue(8), Is.EqualTo(Decimal.ValueOf(19)));
            Assert.That(tsl.GetValue(9), Is.EqualTo(Decimal.ValueOf(21)));
            Assert.That(tsl.GetValue(10), Is.EqualTo(Decimal.ValueOf(22)));
            Assert.That(tsl.GetValue(11), Is.EqualTo(Decimal.ValueOf(25)));
            Assert.That(tsl.GetValue(12), Is.EqualTo(Decimal.ValueOf(25)));
        }

        [Test]
        public void WithInitialLimitUsingClosePrice()
        {
            var price = new ClosePriceIndicator(_data);
            var tsl = new TrailingStopLossIndicator(price, Decimal.ValueOf(3), Decimal.ValueOf(21));
            Assert.That(tsl.GetValue(0), Is.EqualTo(Decimal.ValueOf(21)));
            Assert.That(tsl.GetValue(1), Is.EqualTo(Decimal.ValueOf(21)));
            Assert.That(tsl.GetValue(2), Is.EqualTo(Decimal.ValueOf(21)));
            Assert.That(tsl.GetValue(8), Is.EqualTo(Decimal.ValueOf(21)));
            Assert.That(tsl.GetValue(9), Is.EqualTo(Decimal.ValueOf(22)));
            Assert.That(tsl.GetValue(10), Is.EqualTo(Decimal.ValueOf(23)));
            Assert.That(tsl.GetValue(11), Is.EqualTo(Decimal.ValueOf(26)));
            Assert.That(tsl.GetValue(12), Is.EqualTo(Decimal.ValueOf(26)));
            Assert.That(tsl.GetValue(13), Is.EqualTo(Decimal.ValueOf(26)));
        }
    }
}