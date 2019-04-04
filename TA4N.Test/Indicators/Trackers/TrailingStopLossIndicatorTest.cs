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

namespace TA4N.Test.Indicators.Trackers
{
    using TA4N.Indicators.Simple;
    using NUnit.Framework;
    using TA4N.Indicators.Trackers;

    public sealed class TrailingStopLossIndicatorTest
	{
        private TimeSeries _data;
        
        [SetUp]
		public void SetUp()
		{
			_data = GenerateTimeSeries.From(18, 19, 23, 22, 21, 22, 17, 18, 21, 25, 26, 29, 29, 28, 29, 26, 25, 26, 26, 28);
		}

        [Test] 
		public void WithoutInitialLimitUsingClosePrice()
		{
			var price = new ClosePriceIndicator(_data);
			var tsl = new TrailingStopLossIndicator(price, Decimal.ValueOf(4));

			TaTestsUtils.AssertDecimalEquals(tsl.GetValue(1), 15);
			TaTestsUtils.AssertDecimalEquals(tsl.GetValue(2), 19);
			TaTestsUtils.AssertDecimalEquals(tsl.GetValue(3), 19);

			TaTestsUtils.AssertDecimalEquals(tsl.GetValue(8), 19);
			TaTestsUtils.AssertDecimalEquals(tsl.GetValue(9), 21);
			TaTestsUtils.AssertDecimalEquals(tsl.GetValue(10), 22);
			TaTestsUtils.AssertDecimalEquals(tsl.GetValue(11), 25);
			TaTestsUtils.AssertDecimalEquals(tsl.GetValue(12), 25);
		}
        
        [Test] 
		public void WithInitialLimitUsingClosePrice()
		{
			var price = new ClosePriceIndicator(_data);
			var tsl = new TrailingStopLossIndicator(price, Decimal.ValueOf(3), Decimal.ValueOf(21));

			TaTestsUtils.AssertDecimalEquals(tsl.GetValue(0), 21);
			TaTestsUtils.AssertDecimalEquals(tsl.GetValue(1), 21);
			TaTestsUtils.AssertDecimalEquals(tsl.GetValue(2), 21);

			TaTestsUtils.AssertDecimalEquals(tsl.GetValue(8), 21);
			TaTestsUtils.AssertDecimalEquals(tsl.GetValue(9), 22);
			TaTestsUtils.AssertDecimalEquals(tsl.GetValue(10), 23);
			TaTestsUtils.AssertDecimalEquals(tsl.GetValue(11), 26);
			TaTestsUtils.AssertDecimalEquals(tsl.GetValue(12), 26);
			TaTestsUtils.AssertDecimalEquals(tsl.GetValue(13), 26);
		}
	}
}