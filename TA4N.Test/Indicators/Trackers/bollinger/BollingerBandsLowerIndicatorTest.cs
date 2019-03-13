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

	public sealed class BollingerBandsLowerIndicatorTest
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
		public void BollingerBandsLowerUsingSmaAndStandardDeviation()
		{
            var bbmSma = new BollingerBandsMiddleIndicator(_sma);
			var standardDeviation = new StandardDeviationIndicator(_closePrice, _timeFrame);
			var bblSma = new BollingerBandsLowerIndicator(bbmSma, standardDeviation);

			TaTestsUtils.AssertDecimalEquals(bblSma.K, 2);

			TaTestsUtils.AssertDecimalEquals(bblSma.GetValue(0), 1);
			TaTestsUtils.AssertDecimalEquals(bblSma.GetValue(1), 0.5);
			TaTestsUtils.AssertDecimalEquals(bblSma.GetValue(2), 0.367);
			TaTestsUtils.AssertDecimalEquals(bblSma.GetValue(3), 1.367);
			TaTestsUtils.AssertDecimalEquals(bblSma.GetValue(4), 2.3905);
			TaTestsUtils.AssertDecimalEquals(bblSma.GetValue(5), 2.7239);
			TaTestsUtils.AssertDecimalEquals(bblSma.GetValue(6), 2.367);

			var bblSmAwithK = new BollingerBandsLowerIndicator(bbmSma, standardDeviation, Decimal.ValueOf("1.5"));

			TaTestsUtils.AssertDecimalEquals(bblSmAwithK.K, 1.5);

			TaTestsUtils.AssertDecimalEquals(bblSmAwithK.GetValue(0), 1);
			TaTestsUtils.AssertDecimalEquals(bblSmAwithK.GetValue(1), 0.75);
			TaTestsUtils.AssertDecimalEquals(bblSmAwithK.GetValue(2), 0.7752);
			TaTestsUtils.AssertDecimalEquals(bblSmAwithK.GetValue(3), 1.7752);
			TaTestsUtils.AssertDecimalEquals(bblSmAwithK.GetValue(4), 2.6262);
			TaTestsUtils.AssertDecimalEquals(bblSmAwithK.GetValue(5), 2.9595);
			TaTestsUtils.AssertDecimalEquals(bblSmAwithK.GetValue(6), 2.7752);
		}
	}
}