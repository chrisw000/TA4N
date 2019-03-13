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
namespace TA4N.Test.Indicators.Trackers
{
    using TA4N.Indicators.Trackers;
    using TA4N.Indicators.Simple;
    using TA4N.Mocks;

    using NUnit.Framework;

	public sealed class EmaIndicatorTest
	{
		private TimeSeries _data;

        [SetUp]
		public void SetUp()
		{
			_data = new MockTimeSeries(64.75, 63.79, 63.73, 63.73, 63.55, 63.19, 63.91, 63.85, 62.95, 63.37, 61.33, 61.51);
		}

        [Test] 
		public void EmaUsingTimeFrame10UsingClosePrice()
		{
			var ema = new EmaIndicator(new ClosePriceIndicator(_data), 10);

			TaTestsUtils.AssertDecimalEquals(ema.GetValue(9), 63.6536);
			TaTestsUtils.AssertDecimalEquals(ema.GetValue(10), 63.2312);
			TaTestsUtils.AssertDecimalEquals(ema.GetValue(11), 62.9182);
		}

        [Test] 
		public void EmaFirstValueShouldBeEqualsToFirstDataValue()
		{
			var ema = new EmaIndicator(new ClosePriceIndicator(_data), 1);
			TaTestsUtils.AssertDecimalEquals(ema.GetValue(0), "64.75");
		}

        [Test] 
		public void ValuesLessThanTimeFrameMustBeEqualsToSmaValues()
		{
			var ema = new EmaIndicator(new ClosePriceIndicator(_data), 10);
			var sma = new SmaIndicator(new ClosePriceIndicator(_data), 10);

			for (var i = 0; i < 9; i++)
			{
				Assert.AreEqual(sma.GetValue(i), ema.GetValue(i));
			}
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
			var closePrice = new ClosePriceIndicator(bigSeries);
			var ema = new EmaIndicator(closePrice, 10);
			// If a StackOverflowError is thrown here, then the RecursiveCachedIndicator
			// does not work as intended.
			TaTestsUtils.AssertDecimalEquals(ema.GetValue(9999), 9994.5);
		}
	}
}