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
using TA4N.Indicators.Trackers.Bollinger;
using TA4N.Test.FixtureData;

namespace TA4N.Test.Indicators.Trackers.Bollinger
{
    public sealed class PercentBIndicatorTest
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
        public void PercentBUsingSmaAndStandardDeviation()
        {
            var pcb = new PercentBIndicator(_closePrice, 5, Decimal.Two);
            Assert.That(pcb.GetValue(0).NaN, Is.True);
            Assert.That(pcb.GetValue(1).ToDouble(), Is.EqualTo(0.75).Within(TaTestsUtils.TaOffset));
            Assert.That(pcb.GetValue(2).ToDouble(), Is.EqualTo(0.8244).Within(TaTestsUtils.TaOffset));
            Assert.That(pcb.GetValue(3).ToDouble(), Is.EqualTo(0.6627).Within(TaTestsUtils.TaOffset));
            Assert.That(pcb.GetValue(4).ToDouble(), Is.EqualTo(0.8517).Within(TaTestsUtils.TaOffset));
            Assert.That(pcb.GetValue(5).ToDouble(), Is.EqualTo(0.90328).Within(TaTestsUtils.TaOffset));
            Assert.That(pcb.GetValue(6).ToDouble(), Is.EqualTo(0.83).Within(TaTestsUtils.TaOffset));
            Assert.That(pcb.GetValue(7).ToDouble(), Is.EqualTo(0.6552).Within(TaTestsUtils.TaOffset));
            Assert.That(pcb.GetValue(8).ToDouble(), Is.EqualTo(0.5737).Within(TaTestsUtils.TaOffset));
            Assert.That(pcb.GetValue(9).ToDouble(), Is.EqualTo(0.1047).Within(TaTestsUtils.TaOffset));
            Assert.That(pcb.GetValue(10).ToDouble(), Is.EqualTo(0.5).Within(TaTestsUtils.TaOffset));
            Assert.That(pcb.GetValue(11).ToDouble(), Is.EqualTo(0.0284).Within(TaTestsUtils.TaOffset));
            Assert.That(pcb.GetValue(12).ToDouble(), Is.EqualTo(0.0344).Within(TaTestsUtils.TaOffset));
            Assert.That(pcb.GetValue(13).ToDouble(), Is.EqualTo(0.2064).Within(TaTestsUtils.TaOffset));
            Assert.That(pcb.GetValue(14).ToDouble(), Is.EqualTo(0.1835).Within(TaTestsUtils.TaOffset));
            Assert.That(pcb.GetValue(15).ToDouble(), Is.EqualTo(0.2131).Within(TaTestsUtils.TaOffset));
            Assert.That(pcb.GetValue(16).ToDouble(), Is.EqualTo(0.3506).Within(TaTestsUtils.TaOffset));
            Assert.That(pcb.GetValue(17).ToDouble(), Is.EqualTo(0.5737).Within(TaTestsUtils.TaOffset));
            Assert.That(pcb.GetValue(18).ToDouble(), Is.EqualTo(0.5).Within(TaTestsUtils.TaOffset));
            Assert.That(pcb.GetValue(19).ToDouble(), Is.EqualTo(0.7673).Within(TaTestsUtils.TaOffset));
        }
    }
}