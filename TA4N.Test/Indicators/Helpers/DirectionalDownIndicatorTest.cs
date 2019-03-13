﻿using System.Collections.Generic;

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

    public sealed class DirectionalDownIndicatorTest
	{
        [Test]
		public void AverageDirectionalMovement()
		{
            IList<Tick> ticks = new List<Tick>();
			ticks.Add(new MockTick(0, 0, 13, 7));
			ticks.Add(new MockTick(0, 0, 11, 5));
			ticks.Add(new MockTick(0, 0, 15, 3));
			ticks.Add(new MockTick(0, 0, 14, 2));
			ticks.Add(new MockTick(0, 0, 13, 0.2));

			var series = new MockTimeSeries(ticks);
			var ddown = new DirectionalDownIndicator(series, 3);
			TaTestsUtils.AssertDecimalEquals(ddown.GetValue(0), 1);
			TaTestsUtils.AssertDecimalEquals(ddown.GetValue(1), (4d / 3) / (13d / 3));
			TaTestsUtils.AssertDecimalEquals(ddown.GetValue(2), (4d / 3 * 2d / 3) / (13d / 3 * 2d / 3 + 15d / 3));
			TaTestsUtils.AssertDecimalEquals(ddown.GetValue(3), ((4d / 3 * 2d / 3) * 2d / 3 + 1d / 3) / (((13d / 3 * 2d / 3 + 15d / 3) * 2d / 3) + 14d / 3));
			TaTestsUtils.AssertDecimalEquals(ddown.GetValue(4), (((4d / 3 * 2d / 3) * 2d / 3 + 1d / 3) * 2d / 3 + 1.8 * 1d / 3) / (((((13d / 3 * 2d / 3 + 15d / 3) * 2d / 3) + 14d / 3) * 2d / 3) + 13d / 3));
		}
	}
}