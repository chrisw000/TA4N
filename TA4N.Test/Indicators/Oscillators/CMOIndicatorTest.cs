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
    using NUnit.Framework;
    using TA4N.Indicators.Simple;
    using TA4N.Indicators.Oscillators;

    public sealed class CmoIndicatorTest
    {
        private TimeSeries _series;

        [SetUp]
        public void SetUp()
        {
            _series = GenerateTimeSeries.From(21.27, 22.19, 22.08, 22.47, 22.48, 22.53, 22.23, 21.43, 21.24, 21.29, 22.15, 22.39, 22.38, 22.61, 23.36, 24.05, 24.75, 24.83, 23.95, 23.63, 23.82, 23.87, 23.15, 23.19, 23.10, 22.65, 22.48, 22.87, 22.93, 22.91);
        }

        [Test]
        public void Dpo()
        {
            var cmo = new CmoIndicator(new ClosePriceIndicator(_series), 9);
            Assert.That(cmo.GetValue(5).ToDouble(), Is.EqualTo(85.1351).Within(TaTestsUtils.TaOffset));
            Assert.That(cmo.GetValue(6).ToDouble(), Is.EqualTo(53.9326).Within(TaTestsUtils.TaOffset));
            Assert.That(cmo.GetValue(7).ToDouble(), Is.EqualTo(6.2016).Within(TaTestsUtils.TaOffset));
            Assert.That(cmo.GetValue(8).ToDouble(), Is.EqualTo(-1.083).Within(TaTestsUtils.TaOffset));
            Assert.That(cmo.GetValue(9).ToDouble(), Is.EqualTo(0.7092).Within(TaTestsUtils.TaOffset));
            Assert.That(cmo.GetValue(10).ToDouble(), Is.EqualTo(-1.4493).Within(TaTestsUtils.TaOffset));
            Assert.That(cmo.GetValue(11).ToDouble(), Is.EqualTo(10.7266).Within(TaTestsUtils.TaOffset));
            Assert.That(cmo.GetValue(12).ToDouble(), Is.EqualTo(-3.5857).Within(TaTestsUtils.TaOffset));
            Assert.That(cmo.GetValue(13).ToDouble(), Is.EqualTo(4.7619).Within(TaTestsUtils.TaOffset));
            Assert.That(cmo.GetValue(14).ToDouble(), Is.EqualTo(24.1983).Within(TaTestsUtils.TaOffset));
            Assert.That(cmo.GetValue(15).ToDouble(), Is.EqualTo(47.644).Within(TaTestsUtils.TaOffset));
        }
    }
}