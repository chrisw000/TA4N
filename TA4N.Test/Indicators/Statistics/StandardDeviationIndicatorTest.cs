using System;

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
namespace TA4N.Test.Indicators.Statistics
{
	using TA4N.Indicators.Simple;
    using TA4N.Mocks;
    using NUnit.Framework;
    using TA4N.Indicators.Statistics;

    public sealed class StandardDeviationIndicatorTest
	{
		private TimeSeries _data;
        
        [SetUp]
		public void SetUp()
		{
			_data = new MockTimeSeries(1, 2, 3, 4, 3, 4, 5, 4, 3, 0, 9);
		}
        
        [Test] 
		public void StandardDeviationUsingTimeFrame4UsingClosePrice()
		{
			var sdv = new StandardDeviationIndicator(new ClosePriceIndicator(_data), 4);

			TaTestsUtils.AssertDecimalEquals(sdv.GetValue(0), 0);
			TaTestsUtils.AssertDecimalEquals(sdv.GetValue(1), Math.Sqrt(0.25));
			TaTestsUtils.AssertDecimalEquals(sdv.GetValue(2), Math.Sqrt(2.0 / 3));
			TaTestsUtils.AssertDecimalEquals(sdv.GetValue(3), Math.Sqrt(1.25));
			TaTestsUtils.AssertDecimalEquals(sdv.GetValue(4), Math.Sqrt(0.5));
			TaTestsUtils.AssertDecimalEquals(sdv.GetValue(5), Math.Sqrt(0.25));
			TaTestsUtils.AssertDecimalEquals(sdv.GetValue(6), Math.Sqrt(0.5));
			TaTestsUtils.AssertDecimalEquals(sdv.GetValue(7), Math.Sqrt(0.5));
			TaTestsUtils.AssertDecimalEquals(sdv.GetValue(8), Math.Sqrt(0.5));
			TaTestsUtils.AssertDecimalEquals(sdv.GetValue(9), Math.Sqrt(3.5));
			TaTestsUtils.AssertDecimalEquals(sdv.GetValue(10), Math.Sqrt(10.5));
		}
        
        [Test]
		public void StandardDeviationShouldBeZeroWhenTimeFrameIs1()
		{
			var sdv = new StandardDeviationIndicator(new ClosePriceIndicator(_data), 1);
			TaTestsUtils.AssertDecimalEquals(sdv.GetValue(3), 0);
			TaTestsUtils.AssertDecimalEquals(sdv.GetValue(8), 0);
		}
	}
}