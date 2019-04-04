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
namespace TA4N.Test.Indicators.Trackers
{
    using NUnit.Framework;
    using TA4N.Indicators.Trackers;

    public sealed class ParabolicSarIndicatorTest
	{
        [Test] 
		public void TrendSwitchTest()
		{
			IList<Tick> ticks = new List<Tick>();
			ticks.Add(GenerateTick.From(0, 10, 13, 8));
			ticks.Add(GenerateTick.From(0, 8, 11, 6));
			ticks.Add(GenerateTick.From(0, 6, 9, 4));
			ticks.Add(GenerateTick.From(0, 11, 15, 9));
			ticks.Add(GenerateTick.From(0, 13, 15, 9));
			var sar = new ParabolicSarIndicator(GenerateTimeSeries.From(ticks), 1);

			TaTestsUtils.AssertDecimalEquals(sar.GetValue(0), 10);
			TaTestsUtils.AssertDecimalEquals(sar.GetValue(1), 8);
			TaTestsUtils.AssertDecimalEquals(sar.GetValue(2), 11);
			TaTestsUtils.AssertDecimalEquals(sar.GetValue(3), 4);
			TaTestsUtils.AssertDecimalEquals(sar.GetValue(4), 4);
		}
        
        [Test]
		public void TrendSwitchTest2()
		{
			IList<Tick> ticks = new List<Tick>();
			ticks.Add(GenerateTick.From(0, 10, 13, 11));
			ticks.Add(GenerateTick.From(0, 10, 15, 13));
			ticks.Add(GenerateTick.From(0, 12, 18, 11));
			ticks.Add(GenerateTick.From(0, 10, 15, 9));
			ticks.Add(GenerateTick.From(0, 9, 15, 9));

			var sar = new ParabolicSarIndicator(GenerateTimeSeries.From(ticks), 1);

			TaTestsUtils.AssertDecimalEquals(sar.GetValue(0), 10);
			TaTestsUtils.AssertDecimalEquals(sar.GetValue(1), 10);
			TaTestsUtils.AssertDecimalEquals(sar.GetValue(2), 0.04 * (18 - 10) + 10);
			TaTestsUtils.AssertDecimalEquals(sar.GetValue(3), 18);
			TaTestsUtils.AssertDecimalEquals(sar.GetValue(4), 18);
		}
        
        [Test]
		public void UpTrendTest()
		{
			IList<Tick> ticks = new List<Tick>();
			ticks.Add(GenerateTick.From(0, 10, 13, 11));
			ticks.Add(GenerateTick.From(0, 17, 15, 11.38));
			ticks.Add(GenerateTick.From(0, 18, 16, 14));
			ticks.Add(GenerateTick.From(0, 19, 17, 12));
			ticks.Add(GenerateTick.From(0, 20, 18, 9));

			var sar = new ParabolicSarIndicator(GenerateTimeSeries.From(ticks), 1);

			TaTestsUtils.AssertDecimalEquals(sar.GetValue(0), 10);
			TaTestsUtils.AssertDecimalEquals(sar.GetValue(1), 17);
			TaTestsUtils.AssertDecimalEquals(sar.GetValue(2), 11.38);
			TaTestsUtils.AssertDecimalEquals(sar.GetValue(3), 11.38);
			TaTestsUtils.AssertDecimalEquals(sar.GetValue(4), 18);
		}
        
        [Test]
		public void DownTrendTest()
		{
			IList<Tick> ticks = new List<Tick>();
			ticks.Add(GenerateTick.From(0, 20, 18, 9));
			ticks.Add(GenerateTick.From(0, 19, 17, 12));
			ticks.Add(GenerateTick.From(0, 18, 16, 14));
			ticks.Add(GenerateTick.From(0, 17, 15, 11.38));
			ticks.Add(GenerateTick.From(0, 10, 13, 11));
			ticks.Add(GenerateTick.From(0, 10, 30, 11));

			var sar = new ParabolicSarIndicator(GenerateTimeSeries.From(ticks), 1);

			TaTestsUtils.AssertDecimalEquals(sar.GetValue(0), 20);
			TaTestsUtils.AssertDecimalEquals(sar.GetValue(1), 19);
			TaTestsUtils.AssertDecimalEquals(sar.GetValue(2), 0.04 * (14 - 19) + 19);
			var value = 0.06 * (11.38 - 18.8) + 18.8;
			TaTestsUtils.AssertDecimalEquals(sar.GetValue(3), value);
			TaTestsUtils.AssertDecimalEquals(sar.GetValue(4), 0.08 * (11 - value) + value);
			TaTestsUtils.AssertDecimalEquals(sar.GetValue(5), 11);
		}
	}
}