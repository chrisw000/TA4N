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

using TA4N.Test.FixtureData;

namespace TA4N.Test.Indicators.Statistics
{
	using TA4N.Indicators.Simple;
    using NUnit.Framework;
    using TA4N.Indicators.Statistics;

    public sealed class VarianceIndicatorTest
	{
		private TimeSeries _data;
        
        [SetUp]
		public void SetUp()
		{
			_data = GenerateTimeSeries.From(1, 2, 3, 4, 3, 4, 5, 4, 3, 0, 9);
		}
        
        [Test]
        public void VarianceUsingTimeFrame4UsingClosePrice()
		{
			var variance = new VarianceIndicator(new ClosePriceIndicator(_data), 4);

			TaTestsUtils.AssertDecimalEquals(variance.GetValue(0), 0);
			TaTestsUtils.AssertDecimalEquals(variance.GetValue(1), 0.25);
			TaTestsUtils.AssertDecimalEquals(variance.GetValue(2), 2.0 / 3);
			TaTestsUtils.AssertDecimalEquals(variance.GetValue(3), 1.25);
			TaTestsUtils.AssertDecimalEquals(variance.GetValue(4), 0.5);
			TaTestsUtils.AssertDecimalEquals(variance.GetValue(5), 0.25);
			TaTestsUtils.AssertDecimalEquals(variance.GetValue(6), 0.5);
			TaTestsUtils.AssertDecimalEquals(variance.GetValue(7), 0.5);
			TaTestsUtils.AssertDecimalEquals(variance.GetValue(8), 0.5);
			TaTestsUtils.AssertDecimalEquals(variance.GetValue(9), 3.5);
			TaTestsUtils.AssertDecimalEquals(variance.GetValue(10), 10.5);
		}
        
        [Test]
		public void FirstValueShouldBeZero()
		{
			var variance = new VarianceIndicator(new ClosePriceIndicator(_data), 4);
			TaTestsUtils.AssertDecimalEquals(variance.GetValue(0), 0);
		}

        [Test]
		public void VarianceShouldBeZeroWhenTimeFrameIs1()
		{
			var variance = new VarianceIndicator(new ClosePriceIndicator(_data), 1);
			TaTestsUtils.AssertDecimalEquals(variance.GetValue(3), 0);
			TaTestsUtils.AssertDecimalEquals(variance.GetValue(8), 0);
		}

        [Test] 
		public void VarianceUsingTimeFrame2UsingClosePrice()
		{
			var variance = new VarianceIndicator(new ClosePriceIndicator(_data), 2);

			TaTestsUtils.AssertDecimalEquals(variance.GetValue(0), 0);
			TaTestsUtils.AssertDecimalEquals(variance.GetValue(1), 0.25);
			TaTestsUtils.AssertDecimalEquals(variance.GetValue(2), 0.25);
			TaTestsUtils.AssertDecimalEquals(variance.GetValue(3), 0.25);
			TaTestsUtils.AssertDecimalEquals(variance.GetValue(9), 2.25);
			TaTestsUtils.AssertDecimalEquals(variance.GetValue(10), 20.25);
		}
	}
}