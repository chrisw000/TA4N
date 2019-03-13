using System.Collections.Generic;

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
namespace TA4N.Test.Indicators.Helpers
{
    using TA4N.Mocks;
    using NUnit.Framework;
    using TA4N.Indicators.Helpers;

    public sealed class AverageDirectionalMovementUpIndicatorTest
	{
        [Test] 
		public void AverageDirectionalMovement()
		{
			IList<Tick> ticks = new List<Tick>();
			ticks.Add(new MockTick(0, 0, 10, 2));
			ticks.Add(new MockTick(0, 0, 12, 2));
			ticks.Add(new MockTick(0, 0, 15, 2));
			ticks.Add(new MockTick(0, 0, 11, 2));
			ticks.Add(new MockTick(0, 0, 13, 7));

			var series = new MockTimeSeries(ticks);
			var admup = new AverageDirectionalMovementUpIndicator(series, 3);
			TaTestsUtils.AssertDecimalEquals(admup.GetValue(0), 1);
			TaTestsUtils.AssertDecimalEquals(admup.GetValue(1), 4d / 3);
			TaTestsUtils.AssertDecimalEquals(admup.GetValue(2), 4d / 3 * 2d / 3 + 1);
			TaTestsUtils.AssertDecimalEquals(admup.GetValue(3), (4d / 3 * 2d / 3 + 1) * 2d / 3);
			TaTestsUtils.AssertDecimalEquals(admup.GetValue(4), (4d / 3 * 2d / 3 + 1) * 2d / 3 * 2d / 3 + 2d / 3);
		}
	}
}