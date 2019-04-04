using System.Collections.Generic;
using NUnit.Framework;
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
namespace TA4N.Test.Indicators.Oscillators
{
    using TA4N.Indicators.Oscillators;

    public sealed class CciIndicatorTest
	{
		private readonly double[] _typicalPrices = {23.98, 23.92, 23.79, 23.67, 23.54, 23.36, 23.65, 23.72, 24.16, 23.91, 23.81, 23.92, 23.74, 24.68, 24.94, 24.93, 25.10, 25.12, 25.20, 25.06, 24.50, 24.31, 24.57, 24.62, 24.49, 24.37, 24.41, 24.35, 23.75, 24.09};
		private TimeSeries _series;

        [SetUp]
		public void SetUp()
		{
			var ticks = new List<Tick>();
			foreach (var price in _typicalPrices)
			{
				ticks.Add(GenerateTick.From(price, price, price, price));
			}
			_series = GenerateTimeSeries.From(ticks);
		}

        [Test]
		public void GetValueWhenTimeFrameIs20()
		{
			var cci = new CciIndicator(_series, 20);

			// Incomplete time frame
			TaTestsUtils.AssertDecimalEquals(cci.GetValue(0), 0);
			TaTestsUtils.AssertDecimalEquals(cci.GetValue(1), -66.6667);
			TaTestsUtils.AssertDecimalEquals(cci.GetValue(2), -100d);
			TaTestsUtils.AssertDecimalEquals(cci.GetValue(10), 14.365);
			TaTestsUtils.AssertDecimalEquals(cci.GetValue(11), 54.2544);

			// Complete time frame
			var results20To30 = new[] {101.9185, 31.1946, 6.5578, 33.6078, 34.9686, 13.6027, -10.6789, -11.471, -29.2567, -128.6, -72.7273};
			for (var i = 0; i < results20To30.Length; i++)
			{
				TaTestsUtils.AssertDecimalEquals(cci.GetValue(i + 19), results20To30[i]);
			}
		}
	}
}