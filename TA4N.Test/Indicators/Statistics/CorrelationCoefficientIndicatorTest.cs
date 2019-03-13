using System.Collections.Generic;
using TA4N.Mocks;

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
namespace TA4N.Test.Indicators.Statistics
{
    using TA4N.Indicators.Statistics;
    using TA4N.Indicators.Simple;
	using VolumeIndicator = TA4N.Indicators.Simple.VolumeIndicator;

    using NUnit.Framework;

	public sealed class CorrelationCoefficientIndicatorTest
	{
		private TimeSeries _data;
		private IIndicator<Decimal> _close, _volume;

        [SetUp]
		public void SetUp()
		{
			IList<Tick> ticks = new List<Tick>();
			// close, volume
			ticks.Add(new MockTick(6, 100));
			ticks.Add(new MockTick(7, 105));
			ticks.Add(new MockTick(9, 130));
			ticks.Add(new MockTick(12, 160));
			ticks.Add(new MockTick(11, 150));
			ticks.Add(new MockTick(10, 130));
			ticks.Add(new MockTick(11, 95));
			ticks.Add(new MockTick(13, 120));
			ticks.Add(new MockTick(15, 180));
			ticks.Add(new MockTick(12, 160));
			ticks.Add(new MockTick(8, 150));
			ticks.Add(new MockTick(4, 200));
			ticks.Add(new MockTick(3, 150));
			ticks.Add(new MockTick(4, 85));
			ticks.Add(new MockTick(3, 70));
			ticks.Add(new MockTick(5, 90));
			ticks.Add(new MockTick(8, 100));
			ticks.Add(new MockTick(9, 95));
			ticks.Add(new MockTick(11, 110));
			ticks.Add(new MockTick(10, 95));

			_data = new TimeSeries(ticks);
			_close = new ClosePriceIndicator(_data);
			_volume = new VolumeIndicator(_data, 2);
		}
        
        [Test] 
		public void UsingTimeFrame5UsingClosePriceAndVolume()
		{
			var coef = new CorrelationCoefficientIndicator(_close, _volume, 5);

			Assert.IsTrue(coef.GetValue(0).NaN);

			TaTestsUtils.AssertDecimalEquals(coef.GetValue(1), 1);
			TaTestsUtils.AssertDecimalEquals(coef.GetValue(2), 0.8773);
			TaTestsUtils.AssertDecimalEquals(coef.GetValue(3), 0.9073);
			TaTestsUtils.AssertDecimalEquals(coef.GetValue(4), 0.9219);
			TaTestsUtils.AssertDecimalEquals(coef.GetValue(5), 0.9205);
			TaTestsUtils.AssertDecimalEquals(coef.GetValue(6), 0.4565);
			TaTestsUtils.AssertDecimalEquals(coef.GetValue(7), -0.4622);
			TaTestsUtils.AssertDecimalEquals(coef.GetValue(8), 0.05747);
			TaTestsUtils.AssertDecimalEquals(coef.GetValue(9), 0.1442);
			TaTestsUtils.AssertDecimalEquals(coef.GetValue(10), -0.1263);
			TaTestsUtils.AssertDecimalEquals(coef.GetValue(11), -0.5345);
			TaTestsUtils.AssertDecimalEquals(coef.GetValue(12), -0.7275);
			TaTestsUtils.AssertDecimalEquals(coef.GetValue(13), 0.1676);
			TaTestsUtils.AssertDecimalEquals(coef.GetValue(14), 0.2506);
			TaTestsUtils.AssertDecimalEquals(coef.GetValue(15), -0.2938);
			TaTestsUtils.AssertDecimalEquals(coef.GetValue(16), -0.3586);
			TaTestsUtils.AssertDecimalEquals(coef.GetValue(17), 0.1713);
			TaTestsUtils.AssertDecimalEquals(coef.GetValue(18), 0.9841);
			TaTestsUtils.AssertDecimalEquals(coef.GetValue(19), 0.9799);
		}
	}
}