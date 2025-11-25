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
    using NUnit.Framework;
    using TA4N.Indicators.Simple;
    using TA4N.Indicators.Trackers;

    /// <summary>
    /// The Class KAMAIndicatorTest.
    /// </summary>
    /// <see href="http://stockcharts.com/school/data/media/chart_school/technical_indicators_and_overlays/kaufman_s_adaptive_moving_average/cs-kama.xls">StockCharts.com</see>
    public sealed class KamaIndicatorTest
    {
        private TimeSeries _data;

        [SetUp]
        public void SetUp()
        {
            _data = GenerateTimeSeries.From(110.46, 109.80, 110.17, 109.82, 110.15, 109.31, 109.05, 107.94, 107.76, 109.24, 109.40, 108.50, 107.96, 108.55, 108.85, 110.44, 109.89, 110.70, 110.79, 110.22, 110.00, 109.27, 106.69, 107.07, 107.92, 107.95, 107.70, 107.97, 106.09, 106.03, 107.65, 109.54, 110.26, 110.38, 111.94, 113.59, 113.98, 113.91, 112.62, 112.20, 111.10, 110.18, 111.13, 111.55, 112.08, 111.95, 111.60, 111.39, 112.25);
        }

        [Test]
        public void Kama()
        {
            var closePrice = new ClosePriceIndicator(_data);
            var kama = new KamaIndicator(closePrice, 10, 2, 30);
            Assert.That(kama.GetValue(9).ToDouble(), Is.EqualTo(109.2400).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(10).ToDouble(), Is.EqualTo(109.2449).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(11).ToDouble(), Is.EqualTo(109.2165).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(12).ToDouble(), Is.EqualTo(109.1173).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(13).ToDouble(), Is.EqualTo(109.0981).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(14).ToDouble(), Is.EqualTo(109.0894).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(15).ToDouble(), Is.EqualTo(109.1240).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(16).ToDouble(), Is.EqualTo(109.1376).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(17).ToDouble(), Is.EqualTo(109.2769).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(18).ToDouble(), Is.EqualTo(109.4365).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(19).ToDouble(), Is.EqualTo(109.4569).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(20).ToDouble(), Is.EqualTo(109.4651).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(21).ToDouble(), Is.EqualTo(109.4612).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(22).ToDouble(), Is.EqualTo(109.3904).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(23).ToDouble(), Is.EqualTo(109.3165).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(24).ToDouble(), Is.EqualTo(109.2924).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(25).ToDouble(), Is.EqualTo(109.1836).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(26).ToDouble(), Is.EqualTo(109.0778).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(27).ToDouble(), Is.EqualTo(108.9498).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(28).ToDouble(), Is.EqualTo(108.4230).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(29).ToDouble(), Is.EqualTo(108.0157).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(30).ToDouble(), Is.EqualTo(107.9967).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(31).ToDouble(), Is.EqualTo(108.0069).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(32).ToDouble(), Is.EqualTo(108.2596).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(33).ToDouble(), Is.EqualTo(108.4818).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(34).ToDouble(), Is.EqualTo(108.9119).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(35).ToDouble(), Is.EqualTo(109.6734).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(36).ToDouble(), Is.EqualTo(110.4947).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(37).ToDouble(), Is.EqualTo(111.1077).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(38).ToDouble(), Is.EqualTo(111.4622).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(39).ToDouble(), Is.EqualTo(111.6092).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(40).ToDouble(), Is.EqualTo(111.5663).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(41).ToDouble(), Is.EqualTo(111.5491).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(42).ToDouble(), Is.EqualTo(111.5425).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(43).ToDouble(), Is.EqualTo(111.5426).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(44).ToDouble(), Is.EqualTo(111.5457).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(45).ToDouble(), Is.EqualTo(111.5658).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(46).ToDouble(), Is.EqualTo(111.5688).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(47).ToDouble(), Is.EqualTo(111.5522).Within(TaTestsUtils.TaOffset));
            Assert.That(kama.GetValue(48).ToDouble(), Is.EqualTo(111.5595).Within(TaTestsUtils.TaOffset));
        }
    }
}