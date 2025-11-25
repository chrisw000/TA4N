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
using TA4N.Indicators.Simple;
using System.Collections.Generic;
using NUnit.Framework;
using TA4N.Test.FixtureData;

namespace TA4N.Test.Indicators.Simple
{
    public sealed class MedianPriceIndicatorTest
    {
        private MedianPriceIndicator _average;
        private TimeSeries _timeSeries;

        [SetUp]
        public void SetUp()
        {
            IList<Tick> ticks = new List<Tick>();
            ticks.Add(GenerateTick.From(0, 0, 16, 8));
            ticks.Add(GenerateTick.From(0, 0, 12, 6));
            ticks.Add(GenerateTick.From(0, 0, 18, 14));
            ticks.Add(GenerateTick.From(0, 0, 10, 6));
            ticks.Add(GenerateTick.From(0, 0, 32, 6));
            ticks.Add(GenerateTick.From(0, 0, 2, 2));
            ticks.Add(GenerateTick.From(0, 0, 0, 0));
            ticks.Add(GenerateTick.From(0, 0, 8, 1));
            ticks.Add(GenerateTick.From(0, 0, 83, 32));
            ticks.Add(GenerateTick.From(0, 0, 9, 3));
            _timeSeries = GenerateTimeSeries.From(ticks);
            _average = new MedianPriceIndicator(_timeSeries);
        }

        [Test]
        public void IndicatorShouldRetrieveTickClosePrice()
        {
            for (var i = 0; i < 10; i++)
            {
                var result = _timeSeries.GetTick(i).MaxPrice.Plus(_timeSeries.GetTick(i).MinPrice).DividedBy(Decimal.Two);
                Assert.That(result, Is.EqualTo(_average.GetValue(i)));
            }
        }
    }
}