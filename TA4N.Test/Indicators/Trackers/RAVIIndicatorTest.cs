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
    using NUnit.Framework;
    using TA4N.Indicators.Simple;
    using TA4N.Mocks;
    using TA4N.Indicators.Trackers;

    public sealed class RaviIndicatorTest
	{
		private TimeSeries _data;

        [SetUp]
		public void SetUp()
		{
			_data = new MockTimeSeries(110.00, 109.27, 104.69, 107.07, 107.92, 107.95, 108.70, 107.97, 106.09, 106.03, 108.65, 109.54, 112.26, 114.38, 117.94);
		}
        
        [Test]
		public void Ravi()
		{
			var closePrice = new ClosePriceIndicator(_data);
			var ravi = new RaviIndicator(closePrice, 3, 8);

			TaTestsUtils.AssertDecimalEquals(ravi.GetValue(0), 0);
			TaTestsUtils.AssertDecimalEquals(ravi.GetValue(1), 0);
			TaTestsUtils.AssertDecimalEquals(ravi.GetValue(2), 0);
			TaTestsUtils.AssertDecimalEquals(ravi.GetValue(3), -0.6937);
			TaTestsUtils.AssertDecimalEquals(ravi.GetValue(4), -1.1411);
			TaTestsUtils.AssertDecimalEquals(ravi.GetValue(5), -0.1577);
			TaTestsUtils.AssertDecimalEquals(ravi.GetValue(6), 0.229);
			TaTestsUtils.AssertDecimalEquals(ravi.GetValue(7), 0.2412);
			TaTestsUtils.AssertDecimalEquals(ravi.GetValue(8), 0.1202);
			TaTestsUtils.AssertDecimalEquals(ravi.GetValue(9), -0.3324);
			TaTestsUtils.AssertDecimalEquals(ravi.GetValue(10), -0.5804);
			TaTestsUtils.AssertDecimalEquals(ravi.GetValue(11), 0.2013);
			TaTestsUtils.AssertDecimalEquals(ravi.GetValue(12), 1.6156);
			TaTestsUtils.AssertDecimalEquals(ravi.GetValue(13), 2.6167);
			TaTestsUtils.AssertDecimalEquals(ravi.GetValue(14), 4.0799);
		}
	}
}