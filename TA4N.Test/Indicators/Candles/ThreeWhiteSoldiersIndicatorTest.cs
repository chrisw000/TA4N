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

    public sealed class ThreeWhiteSoldiersIndicatorTest
	{
        private TimeSeries _series;
        
        [SetUp]
		public void SetUp()
		{
			IList<Tick> ticks = new List<Tick>();
			// open, close, high, low
			ticks.Add(new MockTick(19, 19, 22, 15));
			ticks.Add(new MockTick(10, 18, 20, 8));
			ticks.Add(new MockTick(17, 16, 21, 15));
			ticks.Add(new MockTick(15.6, 18, 18.1, 14));
			ticks.Add(new MockTick(16, 19.9, 20, 15));
			ticks.Add(new MockTick(16.8, 23, 23, 16.7));
			ticks.Add(new MockTick(17, 25, 25, 17));
			ticks.Add(new MockTick(23, 16.8, 24, 15));
			_series = new MockTimeSeries(ticks);
		}
        
        [Test]
		public void GetValue()
		{
			var tws = new ThreeWhiteSoldiersIndicator(_series, 3, Decimal.ValueOf("0.1"));
			Assert.IsFalse(tws.GetValue(0));
			Assert.IsFalse(tws.GetValue(1));
			Assert.IsFalse(tws.GetValue(2));
			Assert.IsFalse(tws.GetValue(3));
			Assert.IsFalse(tws.GetValue(4));
			Assert.IsTrue(tws.GetValue(5));
			Assert.IsFalse(tws.GetValue(6));
			Assert.IsFalse(tws.GetValue(7));
		}
	}
}