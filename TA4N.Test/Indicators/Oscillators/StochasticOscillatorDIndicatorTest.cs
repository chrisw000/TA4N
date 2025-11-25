using System.Collections.Generic;
using TA4N.Indicators.Trackers;
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
namespace TA4N.Test.Indicators.Oscillators
{
    using NUnit.Framework;
    using TA4N.Indicators.Oscillators;

    public sealed class StochasticOscillatorDIndicatorTest
    {
        private TimeSeries _data;

        [SetUp]
        public void SetUp()
        {
            IList<Tick> ticks = new List<Tick>();
            ticks.Add(GenerateTick.From(44.98, 119.13, 119.50, 116.00));
            ticks.Add(GenerateTick.From(45.05, 116.75, 119.94, 116.00));
            ticks.Add(GenerateTick.From(45.11, 113.50, 118.44, 111.63));
            ticks.Add(GenerateTick.From(45.19, 111.56, 114.19, 110.06));
            ticks.Add(GenerateTick.From(45.12, 112.25, 112.81, 109.63));
            ticks.Add(GenerateTick.From(45.15, 110.00, 113.44, 109.13));
            ticks.Add(GenerateTick.From(45.13, 113.50, 115.81, 110.38));
            ticks.Add(GenerateTick.From(45.12, 117.13, 117.50, 114.06));
            ticks.Add(GenerateTick.From(45.15, 115.63, 118.44, 114.81));
            ticks.Add(GenerateTick.From(45.24, 114.13, 116.88, 113.13));
            ticks.Add(GenerateTick.From(45.43, 118.81, 119.00, 116.19));
            ticks.Add(GenerateTick.From(45.43, 117.38, 119.75, 117.00));
            ticks.Add(GenerateTick.From(45.58, 119.13, 119.13, 116.88));
            ticks.Add(GenerateTick.From(45.58, 115.38, 119.44, 114.56));
            _data = new TimeSeries(ticks);
        }

        [Test]
        public void StochasticOscilatorDParam14UsingSma3AndGenericConstructer()
        {
            var sof = new StochasticOscillatorKIndicator(_data, 14);
            var sma = new SmaIndicator(sof, 3);
            var sos = new StochasticOscillatorDIndicator(sma);
            Assert.That(sos.GetValue(0), Is.EqualTo(sma.GetValue(0)));
            Assert.That(sos.GetValue(1), Is.EqualTo(sma.GetValue(1)));
            Assert.That(sos.GetValue(2), Is.EqualTo(sma.GetValue(2)));
        }

        [Test]
        public void StochasticOscilatorDParam14UsingSma3()
        {
            var sof = new StochasticOscillatorKIndicator(_data, 14);
            var sos = new StochasticOscillatorDIndicator(sof);
            var sma = new SmaIndicator(sof, 3);
            Assert.That(sos.GetValue(0), Is.EqualTo(sma.GetValue(0)));
            Assert.That(sos.GetValue(1), Is.EqualTo(sma.GetValue(1)));
            Assert.That(sos.GetValue(2), Is.EqualTo(sma.GetValue(2)));
            Assert.That(sos.GetValue(13), Is.EqualTo(sma.GetValue(13)));
        }
    }
}