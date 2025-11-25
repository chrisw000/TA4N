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
namespace TA4N.Test.Indicators.Statistics
{
    using TA4N.Indicators.Statistics;
    using TA4N.Indicators.Simple;
    using VolumeIndicator = TA4N.Indicators.Simple.VolumeIndicator;
    using NUnit.Framework;

    public sealed class CorrelationCoefficientIndicatorTest
    {
        private TimeSeries _data;
        private IIndicator<Decimal> _close, _volume;

        [SetUp]
        public void SetUp()
        {
            IList<Tick> ticks = new List<Tick>();
            // close, volume
            ticks.Add(GenerateTick.From(6, 100));
            ticks.Add(GenerateTick.From(7, 105));
            ticks.Add(GenerateTick.From(9, 130));
            ticks.Add(GenerateTick.From(12, 160));
            ticks.Add(GenerateTick.From(11, 150));
            ticks.Add(GenerateTick.From(10, 130));
            ticks.Add(GenerateTick.From(11, 95));
            ticks.Add(GenerateTick.From(13, 120));
            ticks.Add(GenerateTick.From(15, 180));
            ticks.Add(GenerateTick.From(12, 160));
            ticks.Add(GenerateTick.From(8, 150));
            ticks.Add(GenerateTick.From(4, 200));
            ticks.Add(GenerateTick.From(3, 150));
            ticks.Add(GenerateTick.From(4, 85));
            ticks.Add(GenerateTick.From(3, 70));
            ticks.Add(GenerateTick.From(5, 90));
            ticks.Add(GenerateTick.From(8, 100));
            ticks.Add(GenerateTick.From(9, 95));
            ticks.Add(GenerateTick.From(11, 110));
            ticks.Add(GenerateTick.From(10, 95));
            _data = new TimeSeries(ticks);
            _close = new ClosePriceIndicator(_data);
            _volume = new VolumeIndicator(_data, 2);
        }

        [Test]
        public void UsingTimeFrame5UsingClosePriceAndVolume()
        {
            var coef = new CorrelationCoefficientIndicator(_close, _volume, 5);
            Assert.That(coef.GetValue(0).NaN, Is.True);
            Assert.That(coef.GetValue(1), Is.EqualTo(Decimal.ValueOf(1)));
            Assert.That(coef.GetValue(2).ToDouble(), Is.EqualTo(0.8773).Within(TaTestsUtils.TaOffset));
            Assert.That(coef.GetValue(3).ToDouble(), Is.EqualTo(0.9073).Within(TaTestsUtils.TaOffset));
            Assert.That(coef.GetValue(4).ToDouble(), Is.EqualTo(0.9219).Within(TaTestsUtils.TaOffset));
            Assert.That(coef.GetValue(5).ToDouble(), Is.EqualTo(0.9205).Within(TaTestsUtils.TaOffset));
            Assert.That(coef.GetValue(6).ToDouble(), Is.EqualTo(0.4565).Within(TaTestsUtils.TaOffset));
            Assert.That(coef.GetValue(7).ToDouble(), Is.EqualTo(-0.4622).Within(TaTestsUtils.TaOffset));
            Assert.That(coef.GetValue(8).ToDouble(), Is.EqualTo(0.05747).Within(TaTestsUtils.TaOffset));
            Assert.That(coef.GetValue(9).ToDouble(), Is.EqualTo(0.1442).Within(TaTestsUtils.TaOffset));
            Assert.That(coef.GetValue(10).ToDouble(), Is.EqualTo(-0.1263).Within(TaTestsUtils.TaOffset));
            Assert.That(coef.GetValue(11).ToDouble(), Is.EqualTo(-0.5345).Within(TaTestsUtils.TaOffset));
            Assert.That(coef.GetValue(12).ToDouble(), Is.EqualTo(-0.7275).Within(TaTestsUtils.TaOffset));
            Assert.That(coef.GetValue(13).ToDouble(), Is.EqualTo(0.1676).Within(TaTestsUtils.TaOffset));
            Assert.That(coef.GetValue(14).ToDouble(), Is.EqualTo(0.2506).Within(TaTestsUtils.TaOffset));
            Assert.That(coef.GetValue(15).ToDouble(), Is.EqualTo(-0.2938).Within(TaTestsUtils.TaOffset));
            Assert.That(coef.GetValue(16).ToDouble(), Is.EqualTo(-0.3586).Within(TaTestsUtils.TaOffset));
            Assert.That(coef.GetValue(17).ToDouble(), Is.EqualTo(0.1713).Within(TaTestsUtils.TaOffset));
            Assert.That(coef.GetValue(18).ToDouble(), Is.EqualTo(0.9841).Within(TaTestsUtils.TaOffset));
            Assert.That(coef.GetValue(19).ToDouble(), Is.EqualTo(0.9799).Within(TaTestsUtils.TaOffset));
        }
    }
}