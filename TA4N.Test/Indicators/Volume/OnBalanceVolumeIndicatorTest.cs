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
namespace TA4N.Test.Indicators.Volume
{
    using TA4N.Mocks;
	using NodaTime;
    using NUnit.Framework;
    using TA4N.Indicators.Volume;

    public sealed class OnBalanceVolumeIndicatorTest
	{
        [Test]
		public void GetValue()
		{
			var now = new LocalDateTime();
			IList<Tick> ticks = new List<Tick>();
			ticks.Add(new MockTick(now, 0, 10, 0, 0, 0, 4, 0));
			ticks.Add(new MockTick(now, 0, 5, 0, 0, 0, 2, 0));
			ticks.Add(new MockTick(now, 0, 6, 0, 0, 0, 3, 0));
			ticks.Add(new MockTick(now, 0, 7, 0, 0, 0, 8, 0));
			ticks.Add(new MockTick(now, 0, 7, 0, 0, 0, 6, 0));
			ticks.Add(new MockTick(now, 0, 6, 0, 0, 0, 10, 0));

			var obv = new OnBalanceVolumeIndicator(new MockTimeSeries(ticks));
			TaTestsUtils.AssertDecimalEquals(obv.GetValue(0), 0);
			TaTestsUtils.AssertDecimalEquals(obv.GetValue(1), -2);
			TaTestsUtils.AssertDecimalEquals(obv.GetValue(2), 1);
			TaTestsUtils.AssertDecimalEquals(obv.GetValue(3), 9);
			TaTestsUtils.AssertDecimalEquals(obv.GetValue(4), 9);
			TaTestsUtils.AssertDecimalEquals(obv.GetValue(5), -1);
		}
        
        [Test]
		public void StackOverflowError()
		{
			IList<Tick> bigListOfTicks = new List<Tick>();
			for (var i = 0; i < 10000; i++)
			{
				bigListOfTicks.Add(new MockTick(i));
			}
			var bigSeries = new MockTimeSeries(bigListOfTicks);
			var obv = new OnBalanceVolumeIndicator(bigSeries);
			// If a StackOverflowError is thrown here, then the RecursiveCachedIndicator
			// does not work as intended.
			TaTestsUtils.AssertDecimalEquals(obv.GetValue(9999), 0);
		}
	}
}