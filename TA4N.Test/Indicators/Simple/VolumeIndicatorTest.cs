/*
The MIT License (MIT)

Copyright (c) 2014-2016 Marc de Verdelhan & respective authors (see AUTHORS)

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
using System.Collections.Generic;
using NUnit.Framework;
using TA4N.Indicators.Simple;
using TA4N.Test.FixtureData;

namespace TA4N.Test.Indicators.Simple
{
    public sealed class VolumeIndicatorTest
    {
        [Test]
        public void IndicatorShouldRetrieveTickVolume()
        {
            TimeSeries series = GenerateTimeSeries.WithArbitraryTicks();
            var volumeIndicator = new VolumeIndicator(series);
            for (var i = 0; i < 10; i++)
            {
                Assert.That(series.GetTick(i).Volume, Is.EqualTo(volumeIndicator.GetValue(i)));
            }
        }

        [Test]
        public void SumOfVolume()
        {
            IList<Tick> ticks = new List<Tick>();
            ticks.Add(GenerateTick.From(0, 10));
            ticks.Add(GenerateTick.From(0, 11));
            ticks.Add(GenerateTick.From(0, 12));
            ticks.Add(GenerateTick.From(0, 13));
            ticks.Add(GenerateTick.From(0, 150));
            ticks.Add(GenerateTick.From(0, 155));
            ticks.Add(GenerateTick.From(0, 160));
            var volumeIndicator = new VolumeIndicator(GenerateTimeSeries.From(ticks), 3);
            Assert.That(volumeIndicator.GetValue(0), Is.EqualTo(Decimal.ValueOf(10)));
            Assert.That(volumeIndicator.GetValue(1), Is.EqualTo(Decimal.ValueOf(21)));
            Assert.That(volumeIndicator.GetValue(2), Is.EqualTo(Decimal.ValueOf(33)));
            Assert.That(volumeIndicator.GetValue(3), Is.EqualTo(Decimal.ValueOf(36)));
            Assert.That(volumeIndicator.GetValue(4), Is.EqualTo(Decimal.ValueOf(175)));
            Assert.That(volumeIndicator.GetValue(5), Is.EqualTo(Decimal.ValueOf(318)));
            Assert.That(volumeIndicator.GetValue(6), Is.EqualTo(Decimal.ValueOf(465)));
        }
    }
}