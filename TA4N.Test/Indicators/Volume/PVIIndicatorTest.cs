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
namespace TA4N.Test.Indicators.Volume
{
    using NUnit.Framework;
    using TA4N.Indicators.Volume;

    public sealed class PviIndicatorTest
	{
        [Test]
		public void GetValue()
		{
            IList<Tick> ticks = new List<Tick>();
			ticks.Add(GenerateTick.From(1355.69, 2739.55));
			ticks.Add(GenerateTick.From(1325.51, 3119.46));
			ticks.Add(GenerateTick.From(1335.02, 3466.88));
			ticks.Add(GenerateTick.From(1313.72, 2577.12));
			ticks.Add(GenerateTick.From(1319.99, 2480.45));
			ticks.Add(GenerateTick.From(1331.85, 2329.79));
			ticks.Add(GenerateTick.From(1329.04, 2793.07));
			ticks.Add(GenerateTick.From(1362.16, 3378.78));
			ticks.Add(GenerateTick.From(1365.51, 2417.59));
			ticks.Add(GenerateTick.From(1374.02, 1442.81));
			TimeSeries series = GenerateTimeSeries.From(ticks);

			var pvi = new PviIndicator(series);
			TaTestsUtils.AssertDecimalEquals(pvi.GetValue(0), 1000);
			TaTestsUtils.AssertDecimalEquals(pvi.GetValue(1), 977.7383);
			TaTestsUtils.AssertDecimalEquals(pvi.GetValue(2), 984.7532);
			TaTestsUtils.AssertDecimalEquals(pvi.GetValue(3), 984.7532);
			TaTestsUtils.AssertDecimalEquals(pvi.GetValue(4), 984.7532);
			TaTestsUtils.AssertDecimalEquals(pvi.GetValue(5), 984.7532);
			TaTestsUtils.AssertDecimalEquals(pvi.GetValue(6), 982.6755);
			TaTestsUtils.AssertDecimalEquals(pvi.GetValue(7), 1007.164);
			TaTestsUtils.AssertDecimalEquals(pvi.GetValue(8), 1007.164);
			TaTestsUtils.AssertDecimalEquals(pvi.GetValue(9), 1007.164);
		}
	}
}