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
    using TA4N.Indicators.Simple;
    using TA4N.Mocks;
    using NUnit.Framework;
    using TA4N.Indicators.Trackers;

    public sealed class RsiIndicatorTest
	{
        private TimeSeries _data;
        
        [SetUp]
		public void SetUp()
		{
			_data = new MockTimeSeries(50.45, 50.30, 50.20, 50.15, 50.05, 50.06, 50.10, 50.08, 50.03, 50.07, 50.01, 50.14, 50.22, 50.43, 50.50, 50.56, 50.52, 50.70, 50.55, 50.62, 50.90, 50.82, 50.86, 51.20, 51.30, 51.10);
		}
        
        [Test] 
		public void RsiUsingTimeFrame14UsingClosePrice()
		{
			var rsi = new RsiIndicator(new ClosePriceIndicator(_data), 14);

			TaTestsUtils.AssertDecimalEquals(rsi.GetValue(15), 62.7451);
			TaTestsUtils.AssertDecimalEquals(rsi.GetValue(16), 66.6667);
			TaTestsUtils.AssertDecimalEquals(rsi.GetValue(17), 75.2294);
			TaTestsUtils.AssertDecimalEquals(rsi.GetValue(18), 71.9298);
			TaTestsUtils.AssertDecimalEquals(rsi.GetValue(19), 73.3333);
			TaTestsUtils.AssertDecimalEquals(rsi.GetValue(20), 77.7778);
			TaTestsUtils.AssertDecimalEquals(rsi.GetValue(21), 74.6667);
			TaTestsUtils.AssertDecimalEquals(rsi.GetValue(22), 77.8523);
			TaTestsUtils.AssertDecimalEquals(rsi.GetValue(23), 81.5642);
			TaTestsUtils.AssertDecimalEquals(rsi.GetValue(24), 85.2459);
		}
        
        [Test] 
		public void RsiFirstValueShouldBeZero()
		{
			var rsi = new RsiIndicator(new ClosePriceIndicator(_data), 14);

			Assert.AreEqual(Decimal.Zero, rsi.GetValue(0));
		}

        [Test] 
		public void RsiHundredIfNoLoss()
		{
			var rsi = new RsiIndicator(new ClosePriceIndicator(_data), 3);

			Assert.AreEqual(Decimal.Hundred, rsi.GetValue(14));
			Assert.AreEqual(Decimal.Hundred, rsi.GetValue(15));
		}
	}
}