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
    using TA4N.Indicators.Simple;
    using NUnit.Framework;
    using TA4N.Indicators.Trackers;

    public sealed class RocIndicatorTest
    {
        private readonly double[] _closePriceValues = new double[]
        {
            11045.27,
            11167.32,
            11008.61,
            11151.83,
            10926.77,
            10868.12,
            10520.32,
            10380.43,
            10785.14,
            10748.26,
            10896.91,
            10782.95,
            10620.16,
            10625.83,
            10510.95,
            10444.37,
            10068.01,
            10193.39,
            10066.57,
            10043.75
        };
        private ClosePriceIndicator _closePrice;

        [SetUp]
        public void SetUp()
        {
            _closePrice = new ClosePriceIndicator(GenerateTimeSeries.From(_closePriceValues));
        }

        [Test]
        public void GetValueWhenTimeFrameIs12()
        {
            var roc = new RocIndicator(_closePrice, 12);
            // Incomplete time frame
            Assert.That(roc.GetValue(0), Is.EqualTo(Decimal.ValueOf(0)));
            Assert.That(roc.GetValue(1).ToDouble(), Is.EqualTo(1.105).Within(TaTestsUtils.TaOffset));
            Assert.That(roc.GetValue(2).ToDouble(), Is.EqualTo(-0.3319).Within(TaTestsUtils.TaOffset));
            Assert.That(roc.GetValue(3).ToDouble(), Is.EqualTo(0.9648).Within(TaTestsUtils.TaOffset));
            // Complete time frame
            var results13To20 = new double[]
            {
                -3.8488,
                -4.8489,
                -4.5206,
                -6.3439,
                -7.8592,
                -6.2083,
                -4.3131,
                -3.2434
            };
            for (var i = 0; i < results13To20.Length; i++)
            {
                Assert.That(roc.GetValue(i + 12).ToDouble(), Is.EqualTo(results13To20[i]).Within(TaTestsUtils.TaOffset));
            }
        }
    }
}