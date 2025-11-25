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

namespace TA4N.Test.Indicators.Volatility
{
    using TA4N.Indicators.Simple;
    using TA4N.Indicators.Volatility;
    using NUnit.Framework;

    public sealed class UlcerIndexIndicatorTest
    {
        private TimeSeries _ibmData;

        [SetUp]
        public void SetUp()
        {
            _ibmData = GenerateTimeSeries.From(194.75, 195.00, 195.10, 194.46, 190.60, 188.86, 185.47, 184.46, 182.31, 185.22, 184.00, 182.87, 187.45, 194.51, 191.63, 190.02, 189.53, 190.27, 193.13, 195.55, 195.84, 195.15, 194.35, 193.62, 197.68, 197.91, 199.08, 199.03, 198.42, 199.29, 199.01, 198.29, 198.40, 200.84, 201.22, 200.50, 198.65, 197.25, 195.70, 197.77, 195.69, 194.87, 195.08);
        }

        [Test]
        public void UlcerIndexUsingTimeFrame14UsingIbmData()
        {
            var ulcer = new UlcerIndexIndicator(new ClosePriceIndicator(_ibmData), 14);
            Assert.That(ulcer.GetValue(0), Is.EqualTo(Decimal.ValueOf(0)));
            // From: http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:ulcer_index
            Assert.That(ulcer.GetValue(26).ToDouble(), Is.EqualTo(1.3047).Within(TaTestsUtils.TaOffset));
            Assert.That(ulcer.GetValue(27).ToDouble(), Is.EqualTo(1.3022).Within(TaTestsUtils.TaOffset));
            Assert.That(ulcer.GetValue(28).ToDouble(), Is.EqualTo(1.2156).Within(TaTestsUtils.TaOffset));
            Assert.That(ulcer.GetValue(29).ToDouble(), Is.EqualTo(0.9967).Within(TaTestsUtils.TaOffset));
            Assert.That(ulcer.GetValue(30).ToDouble(), Is.EqualTo(0.7257).Within(TaTestsUtils.TaOffset));
            Assert.That(ulcer.GetValue(31).ToDouble(), Is.EqualTo(0.453).Within(TaTestsUtils.TaOffset));
            Assert.That(ulcer.GetValue(32).ToDouble(), Is.EqualTo(0.4284).Within(TaTestsUtils.TaOffset));
            Assert.That(ulcer.GetValue(33).ToDouble(), Is.EqualTo(0.4284).Within(TaTestsUtils.TaOffset));
            Assert.That(ulcer.GetValue(34).ToDouble(), Is.EqualTo(0.4284).Within(TaTestsUtils.TaOffset));
            Assert.That(ulcer.GetValue(35).ToDouble(), Is.EqualTo(0.4287).Within(TaTestsUtils.TaOffset));
            Assert.That(ulcer.GetValue(36).ToDouble(), Is.EqualTo(0.5089).Within(TaTestsUtils.TaOffset));
            Assert.That(ulcer.GetValue(37).ToDouble(), Is.EqualTo(0.6673).Within(TaTestsUtils.TaOffset));
            Assert.That(ulcer.GetValue(38).ToDouble(), Is.EqualTo(0.9914).Within(TaTestsUtils.TaOffset));
            Assert.That(ulcer.GetValue(39).ToDouble(), Is.EqualTo(1.0921).Within(TaTestsUtils.TaOffset));
            Assert.That(ulcer.GetValue(40).ToDouble(), Is.EqualTo(1.3161).Within(TaTestsUtils.TaOffset));
            Assert.That(ulcer.GetValue(41).ToDouble(), Is.EqualTo(1.5632).Within(TaTestsUtils.TaOffset));
            Assert.That(ulcer.GetValue(42).ToDouble(), Is.EqualTo(1.7609).Within(TaTestsUtils.TaOffset));
        }
    }
}