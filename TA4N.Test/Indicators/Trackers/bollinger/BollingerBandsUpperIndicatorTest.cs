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
    using TA4N.Indicators.Trackers;
    using TA4N.Indicators.Trackers.Bollinger;
    using TA4N.Indicators.Statistics;
	using TA4N.Indicators.Simple;
    using TA4N.Mocks;
    using NUnit.Framework;

	public sealed class BollingerBandsUpperIndicatorTest
	{
        private TimeSeries _data;
        private int _timeFrame;
        private ClosePriceIndicator _closePrice;
        private SmaIndicator _sma;
        
        [SetUp]
		public void SetUp()
		{
			_data = new MockTimeSeries(1, 2, 3, 4, 3, 4, 5, 4, 3, 3, 4, 3, 2);
			_timeFrame = 3;
			_closePrice = new ClosePriceIndicator(_data);
			_sma = new SmaIndicator(_closePrice, _timeFrame);
		}

        [Test] 
		public void BollingerBandsUpperUsingSmaAndStandardDeviation()
		{
            var bbmSma = new BollingerBandsMiddleIndicator(_sma);
			var standardDeviation = new StandardDeviationIndicator(_closePrice, _timeFrame);
			var bbuSma = new BollingerBandsUpperIndicator(bbmSma, standardDeviation);

			TaTestsUtils.AssertDecimalEquals(bbuSma.K, 2);

			TaTestsUtils.AssertDecimalEquals(bbuSma.GetValue(0), 1);
			TaTestsUtils.AssertDecimalEquals(bbuSma.GetValue(1), 2.5);
			TaTestsUtils.AssertDecimalEquals(bbuSma.GetValue(2), 3.633);
			TaTestsUtils.AssertDecimalEquals(bbuSma.GetValue(3), 4.633);
			TaTestsUtils.AssertDecimalEquals(bbuSma.GetValue(4), 4.2761);
			TaTestsUtils.AssertDecimalEquals(bbuSma.GetValue(5), 4.6094);
			TaTestsUtils.AssertDecimalEquals(bbuSma.GetValue(6), 5.633);
			TaTestsUtils.AssertDecimalEquals(bbuSma.GetValue(7), 5.2761);
			TaTestsUtils.AssertDecimalEquals(bbuSma.GetValue(8), 5.633);
			TaTestsUtils.AssertDecimalEquals(bbuSma.GetValue(9), 4.2761);

			var bbuSmAwithK = new BollingerBandsUpperIndicator(bbmSma, standardDeviation, Decimal.ValueOf("1.5"));

			TaTestsUtils.AssertDecimalEquals(bbuSmAwithK.K, 1.5);

			TaTestsUtils.AssertDecimalEquals(bbuSmAwithK.GetValue(0), 1);
			TaTestsUtils.AssertDecimalEquals(bbuSmAwithK.GetValue(1), 2.25);
			TaTestsUtils.AssertDecimalEquals(bbuSmAwithK.GetValue(2), 3.2247);
			TaTestsUtils.AssertDecimalEquals(bbuSmAwithK.GetValue(3), 4.2247);
			TaTestsUtils.AssertDecimalEquals(bbuSmAwithK.GetValue(4), 4.0404);
			TaTestsUtils.AssertDecimalEquals(bbuSmAwithK.GetValue(5), 4.3737);
			TaTestsUtils.AssertDecimalEquals(bbuSmAwithK.GetValue(6), 5.2247);
			TaTestsUtils.AssertDecimalEquals(bbuSmAwithK.GetValue(7), 5.0404);
			TaTestsUtils.AssertDecimalEquals(bbuSmAwithK.GetValue(8), 5.2247);
			TaTestsUtils.AssertDecimalEquals(bbuSmAwithK.GetValue(9), 4.0404);
		}
	}
}