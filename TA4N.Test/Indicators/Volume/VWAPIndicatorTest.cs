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
namespace TA4N.Test.Indicators.Volume
{
    using TA4N.Indicators.Volume;
    using NUnit.Framework;

    public sealed class VwapIndicatorTest
    {
        private TimeSeries _data;

        [SetUp]
        public void SetUp()
        {
            // @TODO add volumes
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
            ticks.Add(GenerateTick.From(45.45, 45.01, 45.55, 44.80));
            ticks.Add(GenerateTick.From(45.03, 44.23, 45.04, 44.17));
            ticks.Add(GenerateTick.From(44.23, 43.95, 44.29, 43.81));
            ticks.Add(GenerateTick.From(43.91, 43.08, 43.99, 43.08));
            ticks.Add(GenerateTick.From(43.07, 43.55, 43.65, 43.06));
            ticks.Add(GenerateTick.From(43.56, 43.95, 43.99, 43.53));
            ticks.Add(GenerateTick.From(43.93, 44.47, 44.58, 43.93));
            _data = GenerateTimeSeries.From(ticks);
        }

        [Test]
        public void Vwap()
        {
            var vwap = new VwapIndicator(_data, 5);
            Assert.That(vwap.GetValue(5).ToDouble(), Is.EqualTo(45.1453).Within(TaTestsUtils.TaOffset));
            Assert.That(vwap.GetValue(6).ToDouble(), Is.EqualTo(45.1513).Within(TaTestsUtils.TaOffset));
            Assert.That(vwap.GetValue(7).ToDouble(), Is.EqualTo(45.1413).Within(TaTestsUtils.TaOffset));
            Assert.That(vwap.GetValue(8).ToDouble(), Is.EqualTo(45.1547).Within(TaTestsUtils.TaOffset));
            Assert.That(vwap.GetValue(9).ToDouble(), Is.EqualTo(45.1967).Within(TaTestsUtils.TaOffset));
            Assert.That(vwap.GetValue(10).ToDouble(), Is.EqualTo(45.2560).Within(TaTestsUtils.TaOffset));
            Assert.That(vwap.GetValue(11).ToDouble(), Is.EqualTo(45.3340).Within(TaTestsUtils.TaOffset));
            Assert.That(vwap.GetValue(12).ToDouble(), Is.EqualTo(45.4060).Within(TaTestsUtils.TaOffset));
            Assert.That(vwap.GetValue(13).ToDouble(), Is.EqualTo(45.3880).Within(TaTestsUtils.TaOffset));
            Assert.That(vwap.GetValue(14).ToDouble(), Is.EqualTo(45.2120).Within(TaTestsUtils.TaOffset));
            Assert.That(vwap.GetValue(15).ToDouble(), Is.EqualTo(44.9267).Within(TaTestsUtils.TaOffset));
            Assert.That(vwap.GetValue(16).ToDouble(), Is.EqualTo(44.5033).Within(TaTestsUtils.TaOffset));
            Assert.That(vwap.GetValue(17).ToDouble(), Is.EqualTo(44.0840).Within(TaTestsUtils.TaOffset));
            Assert.That(vwap.GetValue(18).ToDouble(), Is.EqualTo(43.8247).Within(TaTestsUtils.TaOffset));
        }
    }
}