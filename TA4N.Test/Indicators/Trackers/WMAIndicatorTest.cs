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

    public sealed class WmaIndicatorTest
	{
        [Test] // calculate()
		public void Calculate()
		{
			var series = GenerateTimeSeries.From(1d, 2d, 3d, 4d, 5d, 6d);
			IIndicator<Decimal> close = new ClosePriceIndicator(series);
			IIndicator<Decimal> wmaIndicator = new WmaIndicator(close, 3);

			TaTestsUtils.AssertDecimalEquals(wmaIndicator.GetValue(0), 1);
			TaTestsUtils.AssertDecimalEquals(wmaIndicator.GetValue(1), 1.6667);
			TaTestsUtils.AssertDecimalEquals(wmaIndicator.GetValue(2), 2.3333);
			TaTestsUtils.AssertDecimalEquals(wmaIndicator.GetValue(3), 3.3333);
			TaTestsUtils.AssertDecimalEquals(wmaIndicator.GetValue(4), 4.3333);
			TaTestsUtils.AssertDecimalEquals(wmaIndicator.GetValue(5), 5.3333);
		}

        [Test]
		public void WmaWithTimeFrameGreaterThanSeriesSize()
		{
			var series = GenerateTimeSeries.From(1d, 2d, 3d, 4d, 5d, 6d);
			IIndicator<Decimal> close = new ClosePriceIndicator(series);
			IIndicator<Decimal> wmaIndicator = new WmaIndicator(close, 55);

			TaTestsUtils.AssertDecimalEquals(wmaIndicator.GetValue(0), 1);
			TaTestsUtils.AssertDecimalEquals(wmaIndicator.GetValue(1), 1.6667);
			TaTestsUtils.AssertDecimalEquals(wmaIndicator.GetValue(2), 2.3333);
			TaTestsUtils.AssertDecimalEquals(wmaIndicator.GetValue(3), 3);
			TaTestsUtils.AssertDecimalEquals(wmaIndicator.GetValue(4), 3.6666);
			TaTestsUtils.AssertDecimalEquals(wmaIndicator.GetValue(5), 4.3333);
		}
        
        [Test]
		public void WmaUsingTimeFrame9UsingClosePrice()
		{
			// Example from http://traders.com/Documentation/FEEDbk_docs/2010/12/TradingIndexesWithHullMA.xls
			TimeSeries data = GenerateTimeSeries.From(84.53, 87.39, 84.55, 82.83, 82.58, 83.74, 83.33, 84.57, 86.98, 87.10, 83.11, 83.60, 83.66, 82.76, 79.22, 79.03, 78.18, 77.42, 74.65, 77.48, 76.87);

			var wma = new WmaIndicator(new ClosePriceIndicator(data), 9);
			TaTestsUtils.AssertDecimalEquals(wma.GetValue(8), 84.4958);
			TaTestsUtils.AssertDecimalEquals(wma.GetValue(9), 85.0158);
			TaTestsUtils.AssertDecimalEquals(wma.GetValue(10), 84.6807);
			TaTestsUtils.AssertDecimalEquals(wma.GetValue(11), 84.5387);
			TaTestsUtils.AssertDecimalEquals(wma.GetValue(12), 84.4298);
			TaTestsUtils.AssertDecimalEquals(wma.GetValue(13), 84.1224);
			TaTestsUtils.AssertDecimalEquals(wma.GetValue(14), 83.1031);
			TaTestsUtils.AssertDecimalEquals(wma.GetValue(15), 82.1462);
			TaTestsUtils.AssertDecimalEquals(wma.GetValue(16), 81.1149);
			TaTestsUtils.AssertDecimalEquals(wma.GetValue(17), 80.0736);
			TaTestsUtils.AssertDecimalEquals(wma.GetValue(18), 78.6907);
			TaTestsUtils.AssertDecimalEquals(wma.GetValue(19), 78.1504);
			TaTestsUtils.AssertDecimalEquals(wma.GetValue(20), 77.6133);
		}
	}
}