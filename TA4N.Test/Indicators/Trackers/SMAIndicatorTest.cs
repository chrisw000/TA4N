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
namespace TA4N.Test.Indicators.Trackers
{
	using TA4N.Indicators.Simple;
    using TA4N.Mocks;
    using NUnit.Framework;
    using TA4N.Indicators.Trackers;

    public sealed class SmaIndicatorTest
	{
		private TimeSeries _data;
        
        [SetUp]
		public void SetUp()
		{
			_data = new MockTimeSeries(1, 2, 3, 4, 3, 4, 5, 4, 3, 3, 4, 3, 2);
		}
        
        [Test]
		public void SmaUsingTimeFrame3UsingClosePrice()
		{
			var sma = new SmaIndicator(new ClosePriceIndicator(_data), 3);

			TaTestsUtils.AssertDecimalEquals(sma.GetValue(0), 1);
			TaTestsUtils.AssertDecimalEquals(sma.GetValue(1), 1.5);
			TaTestsUtils.AssertDecimalEquals(sma.GetValue(2), 2);
			TaTestsUtils.AssertDecimalEquals(sma.GetValue(3), 3);
			TaTestsUtils.AssertDecimalEquals(sma.GetValue(4), 10d / 3);
			TaTestsUtils.AssertDecimalEquals(sma.GetValue(5), 11d / 3);
			TaTestsUtils.AssertDecimalEquals(sma.GetValue(6), 4);
			TaTestsUtils.AssertDecimalEquals(sma.GetValue(7), 13d / 3);
			TaTestsUtils.AssertDecimalEquals(sma.GetValue(8), 4);
			TaTestsUtils.AssertDecimalEquals(sma.GetValue(9), 10d / 3);
			TaTestsUtils.AssertDecimalEquals(sma.GetValue(10), 10d / 3);
			TaTestsUtils.AssertDecimalEquals(sma.GetValue(11), 10d / 3);
			TaTestsUtils.AssertDecimalEquals(sma.GetValue(12), 3);
		}

        [Test] 
		public void SmaWhenTimeFrameIs1ResultShouldBeIndicatorValue()
		{
			var quoteSma = new SmaIndicator(new ClosePriceIndicator(_data), 1);
			for (var i = 0; i < _data.TickCount; i++)
			{
				Assert.AreEqual(_data.GetTick(i).ClosePrice, quoteSma.GetValue(i));
			}
		}
	}
}