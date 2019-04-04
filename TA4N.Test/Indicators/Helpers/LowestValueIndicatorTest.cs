﻿/// <summary>
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

namespace TA4N.Test.Indicators.Helpers
{
	using TA4N.Indicators.Simple;
    using NUnit.Framework;
    using TA4N.Indicators.Helpers;

    public sealed class LowestValueIndicatorTest
	{
		private TimeSeries _data;

        [SetUp]
		public void SetUp()
		{
			_data = GenerateTimeSeries.From(1, 2, 3, 4, 3, 4, 5, 6, 4, 3, 2, 4, 3, 1);
		}
        
        [Test] 
		public void LowestValueIndicatorUsingTimeFrame5UsingClosePrice()
		{
			var lowestValue = new LowestValueIndicator(new ClosePriceIndicator(_data), 5);

			TaTestsUtils.AssertDecimalEquals(lowestValue.GetValue(4), "1");
			TaTestsUtils.AssertDecimalEquals(lowestValue.GetValue(5), "2");
			TaTestsUtils.AssertDecimalEquals(lowestValue.GetValue(6), "3");
			TaTestsUtils.AssertDecimalEquals(lowestValue.GetValue(7), "3");
			TaTestsUtils.AssertDecimalEquals(lowestValue.GetValue(8), "3");
			TaTestsUtils.AssertDecimalEquals(lowestValue.GetValue(9), "3");
			TaTestsUtils.AssertDecimalEquals(lowestValue.GetValue(10), "2");
			TaTestsUtils.AssertDecimalEquals(lowestValue.GetValue(11), "2");
			TaTestsUtils.AssertDecimalEquals(lowestValue.GetValue(12), "2");
		}

        [Test]
		public void LowestValueIndicatorValueShouldBeEqualsToFirstDataValue()
		{
			var lowestValue = new LowestValueIndicator(new ClosePriceIndicator(_data), 5);
			TaTestsUtils.AssertDecimalEquals(lowestValue.GetValue(0), "1");
		}

        [Test]
		public void LowestValueIndicatorWhenTimeFrameIsGreaterThanIndex()
		{
			var lowestValue = new LowestValueIndicator(new ClosePriceIndicator(_data), 500);
			TaTestsUtils.AssertDecimalEquals(lowestValue.GetValue(12), "1");
		}
	}
}