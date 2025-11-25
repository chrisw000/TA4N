using System;
using TA4N.Test.FixtureData;

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
namespace TA4N.Test.Indicators.Statistics
{
    using TA4N.Indicators.Simple;
    using NUnit.Framework;
    using TA4N.Indicators.Statistics;

    public sealed class StandardDeviationIndicatorTest
    {
        private TimeSeries _data;


        [SetUp]
        public void SetUp()
        {
            _data = GenerateTimeSeries.From(1, 2, 3, 4, 3, 4, 5, 4, 3, 0, 9);
        }

        [Test]
        public void StandardDeviationUsingTimeFrame4UsingClosePrice()
        {
            var sdv = new StandardDeviationIndicator(new ClosePriceIndicator(_data), 4);

            Assert.That(sdv.GetValue(0), Is.EqualTo(Decimal.ValueOf(0)));
            Assert.That(sdv.GetValue(1).ToDouble(), Is.EqualTo(Math.Sqrt(0.25)).Within(TaTestsUtils.TaOffset));
            Assert.That(sdv.GetValue(2).ToDouble(), Is.EqualTo(Math.Sqrt(2.0 / 3)).Within(TaTestsUtils.TaOffset));
            Assert.That(sdv.GetValue(3).ToDouble(), Is.EqualTo(Math.Sqrt(1.25)).Within(TaTestsUtils.TaOffset));
            Assert.That(sdv.GetValue(4).ToDouble(), Is.EqualTo(Math.Sqrt(0.5)).Within(TaTestsUtils.TaOffset));
            Assert.That(sdv.GetValue(5).ToDouble(), Is.EqualTo(Math.Sqrt(0.25)).Within(TaTestsUtils.TaOffset));
            Assert.That(sdv.GetValue(6).ToDouble(), Is.EqualTo(Math.Sqrt(0.5)).Within(TaTestsUtils.TaOffset));
            Assert.That(sdv.GetValue(7).ToDouble(), Is.EqualTo(Math.Sqrt(0.5)).Within(TaTestsUtils.TaOffset));
            Assert.That(sdv.GetValue(8).ToDouble(), Is.EqualTo(Math.Sqrt(0.5)).Within(TaTestsUtils.TaOffset));
            Assert.That(sdv.GetValue(9).ToDouble(), Is.EqualTo(Math.Sqrt(3.5)).Within(TaTestsUtils.TaOffset));
            Assert.That(sdv.GetValue(10).ToDouble(), Is.EqualTo(Math.Sqrt(10.5)).Within(TaTestsUtils.TaOffset));
        }

        [Test]
        public void StandardDeviationShouldBeZeroWhenTimeFrameIs1()
        {
            var sdv = new StandardDeviationIndicator(new ClosePriceIndicator(_data), 1);
            Assert.That(sdv.GetValue(3), Is.EqualTo(Decimal.ValueOf(0)));
            Assert.That(sdv.GetValue(8), Is.EqualTo(Decimal.ValueOf(0)));
        }
    }
}