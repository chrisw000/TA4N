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
    using TA4N.Indicators.Trackers;
    using TA4N.Indicators.Simple;
    using NUnit.Framework;

    public sealed class DoubleEmaIndicatorTest
    {
        private TimeSeries _data;
        private ClosePriceIndicator _closePrice;

        [SetUp]
        public void SetUp()
        {
            _data = GenerateTimeSeries.From(0.73, 0.72, 0.86, 0.72, 0.62, 0.76, 0.84, 0.69, 0.65, 0.71, 0.53, 0.73, 0.77, 0.67, 0.68);
            _closePrice = new ClosePriceIndicator(_data);
        }

        [Test]
        public void DoubleEmaUsingTimeFrame5UsingClosePrice()
        {
            var doubleEma = new DoubleEmaIndicator(_closePrice, 5);
            Assert.That(doubleEma.GetValue(0).ToDouble(), Is.EqualTo(0.73));
            Assert.That(doubleEma.GetValue(1).ToDouble(), Is.EqualTo(0.7225));
            Assert.That(doubleEma.GetValue(2).ToDouble(), Is.EqualTo(0.7983));
            Assert.That(doubleEma.GetValue(6).ToDouble(), Is.EqualTo(0.7872));
            Assert.That(doubleEma.GetValue(7).ToDouble(), Is.EqualTo(0.7381));
            Assert.That(doubleEma.GetValue(8).ToDouble(), Is.EqualTo(0.6887));
            Assert.That(doubleEma.GetValue(12).ToDouble(), Is.EqualTo(0.7184));
            Assert.That(doubleEma.GetValue(13).ToDouble(), Is.EqualTo(0.6938));
            Assert.That(doubleEma.GetValue(14).ToDouble(), Is.EqualTo(0.6859));
        }
    }
}