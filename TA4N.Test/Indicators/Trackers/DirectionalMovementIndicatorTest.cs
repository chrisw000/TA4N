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

    public sealed class DirectionalMovementIndicatorTest
	{
        [Test]
		public void GetValue()
		{
			IList<Tick> ticks = new List<Tick>();
			ticks.Add(GenerateTick.From(0, 0, 10, 2));
			ticks.Add(GenerateTick.From(0, 0, 12, 2));
			ticks.Add(GenerateTick.From(0, 0, 15, 2));
			var series = GenerateTimeSeries.From(ticks);

			var dm = new DirectionalMovementIndicator(series, 3);
			TaTestsUtils.AssertDecimalEquals(dm.GetValue(0), 0);
			var dup = (2d / 3 + 2d / 3) / (2d / 3 + 12d / 3);
			var ddown = (2d / 3) / (2d / 3 + 12d / 3);
			TaTestsUtils.AssertDecimalEquals(dm.GetValue(1), (dup - ddown) / (dup + ddown) * 100d);
			dup = ((2d / 3 + 2d / 3) * 2d / 3 + 1) / ((2d / 3 + 12d / 3) * 2d / 3 + 15d / 3);
			ddown = (4d / 9) / ((2d / 3 + 12d / 3) * 2d / 3 + 15d / 3);
			TaTestsUtils.AssertDecimalEquals(dm.GetValue(2), (dup - ddown) / (dup + ddown) * 100d);
		}
	}
}