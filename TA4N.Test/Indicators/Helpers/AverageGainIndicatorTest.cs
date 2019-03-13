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
namespace TA4N.Test.Indicators.Helpers
{
	using TA4N.Indicators.Simple;
    using TA4N.Mocks;
    using TA4N.Indicators.Helpers;

    using NUnit.Framework;

	public sealed class AverageGainIndicatorTest
	{
        private TimeSeries _data;
        
        [SetUp]
		public void SetUp()
		{
			_data = new MockTimeSeries(1, 2, 3, 4, 3, 4, 5, 4, 3, 3, 4, 3, 2);
		}

        [Test] 
		public void AverageGainUsingTimeFrame5UsingClosePrice()
		{
			var averageGain = new AverageGainIndicator(new ClosePriceIndicator(_data), 5);

			TaTestsUtils.AssertDecimalEquals(averageGain.GetValue(5), "0.8");
			TaTestsUtils.AssertDecimalEquals(averageGain.GetValue(6), "0.8");
			TaTestsUtils.AssertDecimalEquals(averageGain.GetValue(7), "0.6");
			TaTestsUtils.AssertDecimalEquals(averageGain.GetValue(8), "0.4");
			TaTestsUtils.AssertDecimalEquals(averageGain.GetValue(9), "0.4");
			TaTestsUtils.AssertDecimalEquals(averageGain.GetValue(10), "0.4");
			TaTestsUtils.AssertDecimalEquals(averageGain.GetValue(11), "0.2");
			TaTestsUtils.AssertDecimalEquals(averageGain.GetValue(12), "0.2");
		}
        
        [Test] 
		public void AverageGainMustReturnZeroWhenTheDataDoesntGain()
		{
			var averageGain = new AverageGainIndicator(new ClosePriceIndicator(_data), 3);
			TaTestsUtils.AssertDecimalEquals(averageGain.GetValue(9), 0);
		}
        
        [Test]
		public void AverageGainWhenTimeFrameIsGreaterThanIndicatorDataShouldBeCalculatedWithDataSize()
		{
			var averageGain = new AverageGainIndicator(new ClosePriceIndicator(_data), 1000);
			TaTestsUtils.AssertDecimalEquals(averageGain.GetValue(12), 6d / _data.TickCount);
		}
        
        [Test]
		public void AverageGainWhenIndexIsZeroMustBeZero()
		{
			var averageGain = new AverageGainIndicator(new ClosePriceIndicator(_data), 10);
			TaTestsUtils.AssertDecimalEquals(averageGain.GetValue(0), 0);
		}
	}
}