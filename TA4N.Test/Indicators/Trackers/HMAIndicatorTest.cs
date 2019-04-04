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

    public sealed class HmaIndicatorTest
	{
        private TimeSeries _data;
        
        [SetUp]
		public void SetUp()
		{
			_data = GenerateTimeSeries.From(84.53, 87.39, 84.55, 82.83, 82.58, 83.74, 83.33, 84.57, 86.98, 87.10, 83.11, 83.60, 83.66, 82.76, 79.22, 79.03, 78.18, 77.42, 74.65, 77.48, 76.87);
		}
        
        [Test]
		public void HmaUsingTimeFrame9UsingClosePrice()
		{
			// Example from http://traders.com/Documentation/FEEDbk_docs/2010/12/TradingIndexesWithHullMA.xls
			var hma = new HmaIndicator(new ClosePriceIndicator(_data), 9);
			TaTestsUtils.AssertDecimalEquals(hma.GetValue(10), 86.3204);
			TaTestsUtils.AssertDecimalEquals(hma.GetValue(11), 85.3705);
			TaTestsUtils.AssertDecimalEquals(hma.GetValue(12), 84.1044);
			TaTestsUtils.AssertDecimalEquals(hma.GetValue(13), 83.0197);
			TaTestsUtils.AssertDecimalEquals(hma.GetValue(14), 81.3913);
			TaTestsUtils.AssertDecimalEquals(hma.GetValue(15), 79.6511);
			TaTestsUtils.AssertDecimalEquals(hma.GetValue(16), 78.0443);
			TaTestsUtils.AssertDecimalEquals(hma.GetValue(17), 76.8832);
			TaTestsUtils.AssertDecimalEquals(hma.GetValue(18), 75.5363);
			TaTestsUtils.AssertDecimalEquals(hma.GetValue(19), 75.1713);
			TaTestsUtils.AssertDecimalEquals(hma.GetValue(20), 75.3597);
		}
	}
}