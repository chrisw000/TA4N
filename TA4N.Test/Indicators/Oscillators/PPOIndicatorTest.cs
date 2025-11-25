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

namespace TA4N.Test.Indicators.Oscillators
{
    using TA4N.Indicators.Oscillators;
    using TA4N.Indicators.Simple;
    using NUnit.Framework;

    public sealed class PpoIndicatorTest
    {
        private TimeSeries _series;
        private ClosePriceIndicator _closePriceIndicator;

        [SetUp]
        public void SetUp()
        {
            _series = GenerateTimeSeries.From(22.27, 22.19, 22.08, 22.17, 22.18, 22.13, 22.23, 22.43, 22.24, 22.29, 22.15, 22.39, 22.38, 22.61, 23.36, 24.05, 23.75, 23.83, 23.95, 23.63, 23.82, 23.87, 23.65, 23.19, 23.10, 23.33, 22.68, 23.10, 22.40, 22.17);
            _closePriceIndicator = new ClosePriceIndicator(_series);
        }

        [Test]
        public void GetValueWithEma10AndEma20()
        {
            var ppo = new PpoIndicator(_closePriceIndicator, 10, 20);
            Assert.That(ppo.GetValue(21).ToDouble(), Is.EqualTo(2.4043).Within(TaTestsUtils.TaOffset));
            Assert.That(ppo.GetValue(22).ToDouble(), Is.EqualTo(2.2224).Within(TaTestsUtils.TaOffset));
            Assert.That(ppo.GetValue(23).ToDouble(), Is.EqualTo(1.88).Within(TaTestsUtils.TaOffset));
            Assert.That(ppo.GetValue(28).ToDouble(), Is.EqualTo(0.4408).Within(TaTestsUtils.TaOffset));
            Assert.That(ppo.GetValue(29).ToDouble(), Is.EqualTo(0.0559).Within(TaTestsUtils.TaOffset));
        }
    }
}