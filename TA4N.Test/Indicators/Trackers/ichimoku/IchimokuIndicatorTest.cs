using System.Collections.Generic;
using NUnit.Framework;
using TA4N.Indicators.Trackers.Ichimoku;
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
namespace TA4N.Test.Indicators.Trackers.Ichimoku
{
    public sealed class IchimokuIndicatorTest
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
        public void Ichimoku()
        {
            var tenkanSen = new IchimokuTenkanSenIndicator(_data, 3);
            var kijunSen = new IchimokuKijunSenIndicator(_data, 5);
            var senkouSpanA = new IchimokuSenkouSpanAIndicator(_data, tenkanSen, kijunSen);
            var senkouSpanB = new IchimokuSenkouSpanBIndicator(_data, 9);
            var chikouSpan = new IchimokuChikouSpanIndicator(_data, 5);
            Assert.That(tenkanSen.GetValue(3).ToDouble(), Is.EqualTo(45.155));
            Assert.That(tenkanSen.GetValue(4).ToDouble(), Is.EqualTo(45.18));
            Assert.That(tenkanSen.GetValue(5).ToDouble(), Is.EqualTo(45.145));
            Assert.That(tenkanSen.GetValue(6).ToDouble(), Is.EqualTo(45.135));
            Assert.That(tenkanSen.GetValue(7).ToDouble(), Is.EqualTo(45.145));
            Assert.That(tenkanSen.GetValue(8).ToDouble(), Is.EqualTo(45.17));
            Assert.That(tenkanSen.GetValue(16).ToDouble(), Is.EqualTo(44.06));
            Assert.That(tenkanSen.GetValue(17).ToDouble(), Is.EqualTo(43.675));
            Assert.That(tenkanSen.GetValue(18).ToDouble(), Is.EqualTo(43.525));
            Assert.That(kijunSen.GetValue(3).ToDouble(), Is.EqualTo(45.14));
            Assert.That(kijunSen.GetValue(4).ToDouble(), Is.EqualTo(45.14));
            Assert.That(kijunSen.GetValue(5).ToDouble(), Is.EqualTo(45.155));
            Assert.That(kijunSen.GetValue(6).ToDouble(), Is.EqualTo(45.18));
            Assert.That(kijunSen.GetValue(7).ToDouble(), Is.EqualTo(45.145));
            Assert.That(kijunSen.GetValue(8).ToDouble(), Is.EqualTo(45.17));
            Assert.That(kijunSen.GetValue(16).ToDouble(), Is.EqualTo(44.345));
            Assert.That(kijunSen.GetValue(17).ToDouble(), Is.EqualTo(44.305));
            Assert.That(kijunSen.GetValue(18).ToDouble(), Is.EqualTo(44.05));
            Assert.That(senkouSpanA.GetValue(3).ToDouble(), Is.EqualTo(45.1475));
            Assert.That(senkouSpanA.GetValue(4).ToDouble(), Is.EqualTo(45.16));
            Assert.That(senkouSpanA.GetValue(5).ToDouble(), Is.EqualTo(45.15));
            Assert.That(senkouSpanA.GetValue(6).ToDouble(), Is.EqualTo(45.1575));
            Assert.That(senkouSpanA.GetValue(7).ToDouble(), Is.EqualTo(45.145));
            Assert.That(senkouSpanA.GetValue(8).ToDouble(), Is.EqualTo(45.17));
            Assert.That(senkouSpanA.GetValue(16).ToDouble(), Is.EqualTo(44.2025));
            Assert.That(senkouSpanA.GetValue(17).ToDouble(), Is.EqualTo(43.99));
            Assert.That(senkouSpanA.GetValue(18).ToDouble(), Is.EqualTo(43.7875));
            Assert.That(senkouSpanB.GetValue(3).ToDouble(), Is.EqualTo(45.14));
            Assert.That(senkouSpanB.GetValue(4).ToDouble(), Is.EqualTo(45.14));
            Assert.That(senkouSpanB.GetValue(5).ToDouble(), Is.EqualTo(45.14));
            Assert.That(senkouSpanB.GetValue(6).ToDouble(), Is.EqualTo(45.14));
            Assert.That(senkouSpanB.GetValue(7).ToDouble(), Is.EqualTo(45.14));
            Assert.That(senkouSpanB.GetValue(8).ToDouble(), Is.EqualTo(45.14));
            Assert.That(senkouSpanB.GetValue(16).ToDouble(), Is.EqualTo(44.345));
            Assert.That(senkouSpanB.GetValue(17).ToDouble(), Is.EqualTo(44.335));
            Assert.That(senkouSpanB.GetValue(18).ToDouble(), Is.EqualTo(44.335));
            Assert.That(chikouSpan.GetValue(5).ToDouble(), Is.EqualTo(45.05));
            Assert.That(chikouSpan.GetValue(6).ToDouble(), Is.EqualTo(45.10));
            Assert.That(chikouSpan.GetValue(7).ToDouble(), Is.EqualTo(45.19));
            Assert.That(chikouSpan.GetValue(8).ToDouble(), Is.EqualTo(45.14));
            Assert.That(chikouSpan.GetValue(19).ToDouble(), Is.EqualTo(44.23));
        }
    }
}