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

    /// <summary>
    /// The Class KAMAIndicatorTest.
    /// </summary>
    /// <see href="http://stockcharts.com/school/data/media/chart_school/technical_indicators_and_overlays/kaufman_s_adaptive_moving_average/cs-kama.xls">StockCharts.com</see>
    public sealed class KamaIndicatorTest
	{
		private TimeSeries _data;

        [SetUp]
		public void SetUp()
		{
			_data = new MockTimeSeries(110.46, 109.80, 110.17, 109.82, 110.15, 109.31, 109.05, 107.94, 107.76, 109.24, 109.40, 108.50, 107.96, 108.55, 108.85, 110.44, 109.89, 110.70, 110.79, 110.22, 110.00, 109.27, 106.69, 107.07, 107.92, 107.95, 107.70, 107.97, 106.09, 106.03, 107.65, 109.54, 110.26, 110.38, 111.94, 113.59, 113.98, 113.91, 112.62, 112.20, 111.10, 110.18, 111.13, 111.55, 112.08, 111.95, 111.60, 111.39, 112.25);
		}
        
        [Test]
		public void Kama()
		{
			var closePrice = new ClosePriceIndicator(_data);
			var kama = new KamaIndicator(closePrice, 10, 2, 30);

			TaTestsUtils.AssertDecimalEquals(kama.GetValue(9), 109.2400);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(10), 109.2449);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(11), 109.2165);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(12), 109.1173);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(13), 109.0981);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(14), 109.0894);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(15), 109.1240);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(16), 109.1376);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(17), 109.2769);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(18), 109.4365);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(19), 109.4569);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(20), 109.4651);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(21), 109.4612);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(22), 109.3904);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(23), 109.3165);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(24), 109.2924);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(25), 109.1836);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(26), 109.0778);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(27), 108.9498);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(28), 108.4230);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(29), 108.0157);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(30), 107.9967);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(31), 108.0069);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(32), 108.2596);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(33), 108.4818);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(34), 108.9119);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(35), 109.6734);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(36), 110.4947);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(37), 111.1077);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(38), 111.4622);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(39), 111.6092);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(40), 111.5663);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(41), 111.5491);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(42), 111.5425);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(43), 111.5426);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(44), 111.5457);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(45), 111.5658);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(46), 111.5688);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(47), 111.5522);
			TaTestsUtils.AssertDecimalEquals(kama.GetValue(48), 111.5595);
		}
	}
}