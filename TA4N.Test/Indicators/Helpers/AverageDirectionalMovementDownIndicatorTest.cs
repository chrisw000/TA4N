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

    public sealed class AverageDirectionalMovementDownIndicatorTest
	{
        [Test]
		public void AverageDirectionalMovement()
		{
			var tick1 = new MockTick(0, 0, 13, 7);
			var tick2 = new MockTick(0, 0, 11, 5);
			var tick3 = new MockTick(0, 0, 15, 3);
			var tick4 = new MockTick(0, 0, 14, 2);
			var tick5 = new MockTick(0, 0, 13, 0.2);

			IList<Tick> ticks = new List<Tick>();
			ticks.Add(tick1);
			ticks.Add(tick2);
			ticks.Add(tick3);
			ticks.Add(tick4);
			ticks.Add(tick5);

			var series = new MockTimeSeries(ticks);
			var admdown = new AverageDirectionalMovementDownIndicator(series, 3);
			TaTestsUtils.AssertDecimalEquals(admdown.GetValue(0), 1);
			TaTestsUtils.AssertDecimalEquals(admdown.GetValue(1), 4d / 3);
			TaTestsUtils.AssertDecimalEquals(admdown.GetValue(2), 4d / 3 * 2d / 3);
			TaTestsUtils.AssertDecimalEquals(admdown.GetValue(3), (4d / 3 * 2d / 3) * 2d / 3 + 1d / 3);
			TaTestsUtils.AssertDecimalEquals(admdown.GetValue(4), ((4d / 3 * 2d / 3) * 2d / 3 + 1d / 3) * 2d / 3 + 1.8 * 1d / 3);
		}
	}
}