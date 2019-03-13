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
    using TA4N.Indicators.Simple;
    using TA4N.Mocks;
    using NUnit.Framework;
    using TA4N.Indicators.Helpers;

    public sealed class MeanDeviationIndicatorTest
	{
		private TimeSeries _data;

        [SetUp]
		public void SetUp()
		{
			_data = new MockTimeSeries(1, 2, 7, 6, 3, 4, 5, 11, 3, 0, 9);
		}
        
        [Test] 
		public void MeanDeviationUsingTimeFrame5UsingClosePrice()
		{
			var meanDeviation = new MeanDeviationIndicator(new ClosePriceIndicator(_data), 5);

			TaTestsUtils.AssertDecimalEquals(meanDeviation.GetValue(2), 2.44444444444444);
			TaTestsUtils.AssertDecimalEquals(meanDeviation.GetValue(3), 2.5);
			TaTestsUtils.AssertDecimalEquals(meanDeviation.GetValue(7), 2.16);
			TaTestsUtils.AssertDecimalEquals(meanDeviation.GetValue(8), 2.32);
			TaTestsUtils.AssertDecimalEquals(meanDeviation.GetValue(9), 2.72);
		}

        [Test]
		public void FirstValueShouldBeZero()
		{
			var meanDeviation = new MeanDeviationIndicator(new ClosePriceIndicator(_data), 5);
			TaTestsUtils.AssertDecimalEquals(meanDeviation.GetValue(0), 0);
		}

        [Test] 
		public void MeanDeviationShouldBeZeroWhenTimeFrameIs1()
		{
			var meanDeviation = new MeanDeviationIndicator(new ClosePriceIndicator(_data), 1);
			TaTestsUtils.AssertDecimalEquals(meanDeviation.GetValue(2), 0);
			TaTestsUtils.AssertDecimalEquals(meanDeviation.GetValue(7), 0);
		}
	}
}