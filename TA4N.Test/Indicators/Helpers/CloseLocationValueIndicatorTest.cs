﻿using System.Collections.Generic;

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
namespace TA4N.Test.Indicators.Helpers
{
    using TA4N.Mocks;
    using NUnit.Framework;
    using TA4N.Indicators.Helpers;

    public sealed class CloseLocationValueIndicatorTest
	{
        private TimeSeries _series;
        
        [SetUp]
		public void SetUp()
		{
			IList<Tick> ticks = new List<Tick>();
			// open, close, high, low
			ticks.Add(new MockTick(10, 18, 20, 10));
			ticks.Add(new MockTick(17, 20, 21, 17));
			ticks.Add(new MockTick(15, 15, 16, 14));
			ticks.Add(new MockTick(15, 11, 15, 8));
			ticks.Add(new MockTick(11, 12, 12, 10));
			_series = new MockTimeSeries(ticks);
		}
        
        [Test]
		public void GetValue()
		{
			var clv = new CloseLocationValueIndicator(_series);
			TaTestsUtils.AssertDecimalEquals(clv.GetValue(0), 0.6);
			TaTestsUtils.AssertDecimalEquals(clv.GetValue(1), 0.5);
			TaTestsUtils.AssertDecimalEquals(clv.GetValue(2), 0);
			TaTestsUtils.AssertDecimalEquals(clv.GetValue(3), -1d / 7);
			TaTestsUtils.AssertDecimalEquals(clv.GetValue(4), 1);
		}
	}
}