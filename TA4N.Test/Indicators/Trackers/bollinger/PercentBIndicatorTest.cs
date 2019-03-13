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
namespace TA4N.Test.Indicators.Trackers.Bollinger
{
    using TA4N.Indicators.Simple;
    using TA4N.Mocks;
    using NUnit.Framework;
    using TA4N.Indicators.Trackers.Bollinger;
    public sealed class PercentBIndicatorTest
	{
        private TimeSeries _data;
        private ClosePriceIndicator _closePrice;
        
        [SetUp]
		public void SetUp()
		{
			_data = new MockTimeSeries(10, 12, 15, 14, 17, 20, 21, 20, 20, 19, 20, 17, 12, 12, 9, 8, 9, 10, 9, 10);
			_closePrice = new ClosePriceIndicator(_data);
		}
        
        [Test] 
		public void PercentBUsingSmaAndStandardDeviation()
		{
			var pcb = new PercentBIndicator(_closePrice, 5, Decimal.Two);

			Assert.IsTrue(pcb.GetValue(0).NaN);
			TaTestsUtils.AssertDecimalEquals(pcb.GetValue(1), 0.75);
			TaTestsUtils.AssertDecimalEquals(pcb.GetValue(2), 0.8244);
			TaTestsUtils.AssertDecimalEquals(pcb.GetValue(3), 0.6627);
			TaTestsUtils.AssertDecimalEquals(pcb.GetValue(4), 0.8517);
			TaTestsUtils.AssertDecimalEquals(pcb.GetValue(5), 0.90328);
			TaTestsUtils.AssertDecimalEquals(pcb.GetValue(6), 0.83);
			TaTestsUtils.AssertDecimalEquals(pcb.GetValue(7), 0.6552);
			TaTestsUtils.AssertDecimalEquals(pcb.GetValue(8), 0.5737);
			TaTestsUtils.AssertDecimalEquals(pcb.GetValue(9), 0.1047);
			TaTestsUtils.AssertDecimalEquals(pcb.GetValue(10), 0.5);
			TaTestsUtils.AssertDecimalEquals(pcb.GetValue(11), 0.0284);
			TaTestsUtils.AssertDecimalEquals(pcb.GetValue(12), 0.0344);
			TaTestsUtils.AssertDecimalEquals(pcb.GetValue(13), 0.2064);
			TaTestsUtils.AssertDecimalEquals(pcb.GetValue(14), 0.1835);
			TaTestsUtils.AssertDecimalEquals(pcb.GetValue(15), 0.2131);
			TaTestsUtils.AssertDecimalEquals(pcb.GetValue(16), 0.3506);
			TaTestsUtils.AssertDecimalEquals(pcb.GetValue(17), 0.5737);
			TaTestsUtils.AssertDecimalEquals(pcb.GetValue(18), 0.5);
			TaTestsUtils.AssertDecimalEquals(pcb.GetValue(19), 0.7673);
		}
	}
}