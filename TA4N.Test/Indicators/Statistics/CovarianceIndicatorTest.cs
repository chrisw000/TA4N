using System.Collections.Generic;
using TA4N.Test.FixtureData;

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
	using TA4N.Indicators.Simple;
	using VolumeIndicator = TA4N.Indicators.Simple.VolumeIndicator;
    using NUnit.Framework;
    using TA4N.Indicators.Statistics;

    public sealed class CovarianceIndicatorTest
	{
		private TimeSeries _data;
		private IIndicator<Decimal> _close, _volume;

        [SetUp]
		public void SetUp()
		{
			IList<Tick> ticks = new List<Tick>();
			// close, volume
			ticks.Add(GenerateTick.From(6, 100));
			ticks.Add(GenerateTick.From(7, 105));
			ticks.Add(GenerateTick.From(9, 130));
			ticks.Add(GenerateTick.From(12, 160));
			ticks.Add(GenerateTick.From(11, 150));
			ticks.Add(GenerateTick.From(10, 130));
			ticks.Add(GenerateTick.From(11, 95));
			ticks.Add(GenerateTick.From(13, 120));
			ticks.Add(GenerateTick.From(15, 180));
			ticks.Add(GenerateTick.From(12, 160));
			ticks.Add(GenerateTick.From(8, 150));
			ticks.Add(GenerateTick.From(4, 200));
			ticks.Add(GenerateTick.From(3, 150));
			ticks.Add(GenerateTick.From(4, 85));
			ticks.Add(GenerateTick.From(3, 70));
			ticks.Add(GenerateTick.From(5, 90));
			ticks.Add(GenerateTick.From(8, 100));
			ticks.Add(GenerateTick.From(9, 95));
			ticks.Add(GenerateTick.From(11, 110));
			ticks.Add(GenerateTick.From(10, 95));

			_data = new TimeSeries(ticks);
			_close = new ClosePriceIndicator(_data);
			_volume = new VolumeIndicator(_data, 2);
		}

        [Test] 
		public void UsingTimeFrame5UsingClosePriceAndVolume()
		{
			var covar = new CovarianceIndicator(_close, _volume, 5);

			TaTestsUtils.AssertDecimalEquals(covar.GetValue(0), 0);
			TaTestsUtils.AssertDecimalEquals(covar.GetValue(1), 26.25);
			TaTestsUtils.AssertDecimalEquals(covar.GetValue(2), 63.3333);
			TaTestsUtils.AssertDecimalEquals(covar.GetValue(3), 143.75);
			TaTestsUtils.AssertDecimalEquals(covar.GetValue(4), 156);
			TaTestsUtils.AssertDecimalEquals(covar.GetValue(5), 60.8);
			TaTestsUtils.AssertDecimalEquals(covar.GetValue(6), 15.2);
			TaTestsUtils.AssertDecimalEquals(covar.GetValue(7), -17.6);
			TaTestsUtils.AssertDecimalEquals(covar.GetValue(8), 4);
			TaTestsUtils.AssertDecimalEquals(covar.GetValue(9), 11.6);
			TaTestsUtils.AssertDecimalEquals(covar.GetValue(10), -14.4);
			TaTestsUtils.AssertDecimalEquals(covar.GetValue(11), -100.2);
			TaTestsUtils.AssertDecimalEquals(covar.GetValue(12), -70.0);
			TaTestsUtils.AssertDecimalEquals(covar.GetValue(13), 24.6);
			TaTestsUtils.AssertDecimalEquals(covar.GetValue(14), 35.0);
			TaTestsUtils.AssertDecimalEquals(covar.GetValue(15), -19.0);
			TaTestsUtils.AssertDecimalEquals(covar.GetValue(16), -47.8);
			TaTestsUtils.AssertDecimalEquals(covar.GetValue(17), 11.4);
			TaTestsUtils.AssertDecimalEquals(covar.GetValue(18), 55.8);
			TaTestsUtils.AssertDecimalEquals(covar.GetValue(19), 33.4);
		}
        
        [Test] 
		public void FirstValueShouldBeZero()
		{
			var covar = new CovarianceIndicator(_close, _volume, 5);
			TaTestsUtils.AssertDecimalEquals(covar.GetValue(0), 0);
		}
        
        [Test] 
		public void ShouldBeZeroWhenTimeFrameIs1()
		{
			var covar = new CovarianceIndicator(_close, _volume, 1);
			TaTestsUtils.AssertDecimalEquals(covar.GetValue(3), 0);
			TaTestsUtils.AssertDecimalEquals(covar.GetValue(8), 0);
		}
	}
}