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
namespace TA4N.Test.Indicators.Candles
{
    using NUnit.Framework;
    using TA4N.Indicators.Candles;

    public sealed class BullishHaramiIndicatorTest
	{
        private TimeSeries _series;
        
        [SetUp]
		public void SetUp()
		{
			IList<Tick> ticks = new List<Tick>();
			// open, close, high, low
			ticks.Add(GenerateTick.From(10, 18, 20, 10));
			ticks.Add(GenerateTick.From(21, 15, 22, 14));
			ticks.Add(GenerateTick.From(17, 20, 21, 17));
			ticks.Add(GenerateTick.From(15, 11, 15, 8));
			ticks.Add(GenerateTick.From(11, 12, 12, 10));
			_series = GenerateTimeSeries.From(ticks);
		}
        
        [Test]
		public void GetValue()
		{
			var bhp = new BullishHaramiIndicator(_series);
			Assert.IsFalse(bhp.GetValue(0));
			Assert.IsFalse(bhp.GetValue(1));
			Assert.IsTrue(bhp.GetValue(2));
			Assert.IsFalse(bhp.GetValue(3));
			Assert.IsFalse(bhp.GetValue(4));
		}
	}
}