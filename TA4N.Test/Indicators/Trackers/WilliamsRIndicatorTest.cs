using System.Collections.Generic;
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
namespace TA4N.Test.Indicators.Trackers
{
    using TA4N.Indicators.Simple;
    using MaxPriceIndicator = TA4N.Indicators.Simple.MaxPriceIndicator;
    using MinPriceIndicator = TA4N.Indicators.Simple.MinPriceIndicator;
    using NUnit.Framework;
    using TA4N.Indicators.Trackers;

    public sealed class WilliamsRIndicatorTest
    {
        private TimeSeries _data;

        [SetUp]
        public void SetUp()
        {
            IList<Tick> ticks = new List<Tick>();
            ticks.Add(GenerateTick.From(44.98, 45.05, 45.17, 44.96));
            ticks.Add(GenerateTick.From(45.05, 45.10, 45.15, 44.99));
            ticks.Add(GenerateTick.From(45.11, 45.19, 45.32, 45.11));
            ticks.Add(GenerateTick.From(45.19, 45.14, 45.25, 45.04));
            ticks.Add(GenerateTick.From(45.12, 45.15, 45.20, 45.10));
            ticks.Add(GenerateTick.From(45.15, 45.14, 45.20, 45.10));
            ticks.Add(GenerateTick.From(45.13, 45.10, 45.16, 45.07));
            ticks.Add(GenerateTick.From(45.12, 45.15, 45.22, 45.10));
            ticks.Add(GenerateTick.From(45.15, 45.22, 45.27, 45.14));
            ticks.Add(GenerateTick.From(45.24, 45.43, 45.45, 45.20));
            ticks.Add(GenerateTick.From(45.43, 45.44, 45.50, 45.39));
            ticks.Add(GenerateTick.From(45.43, 45.55, 45.60, 45.35));
            ticks.Add(GenerateTick.From(45.58, 45.55, 45.61, 45.39));
            _data = new TimeSeries(ticks);
        }

        [Test]
        public void WilliamsRUsingTimeFrame5UsingClosePrice()
        {
            var wr = new WilliamsRIndicator(new ClosePriceIndicator(_data), 5, new MaxPriceIndicator(_data), new MinPriceIndicator(_data));
            Assert.That(wr.GetValue(4).ToDouble(), Is.EqualTo(-47.2222).Within(TaTestsUtils.TaOffset));
            Assert.That(wr.GetValue(5).ToDouble(), Is.EqualTo(-54.5454).Within(TaTestsUtils.TaOffset));
            Assert.That(wr.GetValue(6).ToDouble(), Is.EqualTo(-78.5714).Within(TaTestsUtils.TaOffset));
            Assert.That(wr.GetValue(7).ToDouble(), Is.EqualTo(-47.6190).Within(TaTestsUtils.TaOffset));
            Assert.That(wr.GetValue(8).ToDouble(), Is.EqualTo(-25d).Within(TaTestsUtils.TaOffset));
            Assert.That(wr.GetValue(9).ToDouble(), Is.EqualTo(-5.2632).Within(TaTestsUtils.TaOffset));
            Assert.That(wr.GetValue(10).ToDouble(), Is.EqualTo(-13.9535).Within(TaTestsUtils.TaOffset));
        }

        [Test]
        public void WilliamsRUsingTimeFrame10UsingClosePrice()
        {
            var wr = new WilliamsRIndicator(new ClosePriceIndicator(_data), 10, new MaxPriceIndicator(_data), new MinPriceIndicator(_data));
            Assert.That(wr.GetValue(9).ToDouble(), Is.EqualTo(-4.0816).Within(TaTestsUtils.TaOffset));
            Assert.That(wr.GetValue(10).ToDouble(), Is.EqualTo(-11.7647).Within(TaTestsUtils.TaOffset));
            Assert.That(wr.GetValue(11).ToDouble(), Is.EqualTo(-8.9286).Within(TaTestsUtils.TaOffset));
            Assert.That(wr.GetValue(12).ToDouble(), Is.EqualTo(-10.5263).Within(TaTestsUtils.TaOffset));
        }

        [Test]
        public void ValueLessThenTimeFrame()
        {
            var wr = new WilliamsRIndicator(new ClosePriceIndicator(_data), 100, new MaxPriceIndicator(_data), new MinPriceIndicator(_data));
            Assert.That(wr.GetValue(0).ToDouble(), Is.EqualTo(-100d * (0.12 / 0.21)).Within(TaTestsUtils.TaOffset));
            Assert.That(wr.GetValue(1).ToDouble(), Is.EqualTo(-100d * (0.07 / 0.21)).Within(TaTestsUtils.TaOffset));
            Assert.That(wr.GetValue(2).ToDouble(), Is.EqualTo(-100d * (0.13 / 0.36)).Within(TaTestsUtils.TaOffset));
            Assert.That(wr.GetValue(3).ToDouble(), Is.EqualTo(-100d * (0.18 / 0.36)).Within(TaTestsUtils.TaOffset));
        }
    }
}