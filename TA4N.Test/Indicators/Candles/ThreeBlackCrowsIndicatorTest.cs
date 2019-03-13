using System.Collections.Generic;

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
    using TA4N.Mocks;
    using NUnit.Framework;
    using TA4N.Indicators.Candles;

    public sealed class ThreeBlackCrowsIndicatorTest
	{
        private TimeSeries _series;
        
        [SetUp]
		public void SetUp()
		{
			IList<Tick> ticks = new List<Tick>();
			// open, close, high, low
			ticks.Add(new MockTick(19, 19, 22, 15));
			ticks.Add(new MockTick(10, 18, 20, 8));
			ticks.Add(new MockTick(17, 20, 21, 17));
			ticks.Add(new MockTick(19, 17, 20, 16.9));
			ticks.Add(new MockTick(17.5, 14, 18, 13.9));
			ticks.Add(new MockTick(15, 11, 15, 11));
			ticks.Add(new MockTick(12, 14, 15, 8));
			ticks.Add(new MockTick(13, 16, 16, 11));
			_series = new MockTimeSeries(ticks);
		}
        
        [Test]
		public void GetValue()
		{
			var tbc = new ThreeBlackCrowsIndicator(_series, 3, Decimal.ValueOf("0.1"));
			Assert.IsFalse(tbc.GetValue(0));
			Assert.IsFalse(tbc.GetValue(1));
			Assert.IsFalse(tbc.GetValue(2));
			Assert.IsFalse(tbc.GetValue(3));
			Assert.IsFalse(tbc.GetValue(4));
			Assert.IsTrue(tbc.GetValue(5));
			Assert.IsFalse(tbc.GetValue(6));
			Assert.IsFalse(tbc.GetValue(7));
		}
	}
}