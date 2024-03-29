﻿using System.Collections.Generic;
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

    public sealed class ChandelierExitShortIndicatorTest
	{
        private TimeSeries _data;
        
        [SetUp]
		public void SetUp()
		{
			IList<Tick> ticks = new List<Tick>();
			// open, close, high, low
			ticks.Add(GenerateTick.From(44.98, 45.05, 45.17, 44.96));
			ticks.Add(GenerateTick.From(45.05, 45.10, 45.15, 44.99));
			ticks.Add(GenerateTick.From(45.11, 45.19, 45.32, 45.11));
			ticks.Add(GenerateTick.From(45.19, 45.14, 45.25, 45.04));
			ticks.Add(GenerateTick.From(45.12, 45.15, 45.20, 45.10));
			ticks.Add(GenerateTick.From(45.15, 45.14, 45.20, 45.10));
			ticks.Add(GenerateTick.From(45.13, 45.10, 45.16, 45.07));
			ticks.Add(GenerateTick.From(45.12, 45.15, 45.22, 45.10));
			ticks.Add(GenerateTick.From(45.15, 45.22, 45.27, 45.14));
			ticks.Add(GenerateTick.From(45.24, 45.43, 45.45, 45.20));
			ticks.Add(GenerateTick.From(45.43, 45.44, 45.50, 45.39));
			ticks.Add(GenerateTick.From(45.43, 45.55, 45.60, 45.35));
			ticks.Add(GenerateTick.From(45.58, 45.55, 45.61, 45.39));
			ticks.Add(GenerateTick.From(45.45, 45.01, 45.55, 44.80));
			ticks.Add(GenerateTick.From(45.03, 44.23, 45.04, 44.17));

			_data = new TimeSeries(ticks);
		}

        [Test]
		public void MassIndexUsing3And8TimeFrames()
		{
			var ces = new ChandelierExitShortIndicator(_data, 5, Decimal.Two);

			TaTestsUtils.AssertDecimalEquals(ces.GetValue(5), 45.8424);
			TaTestsUtils.AssertDecimalEquals(ces.GetValue(6), 45.7579);
			TaTestsUtils.AssertDecimalEquals(ces.GetValue(7), 45.6623);
			TaTestsUtils.AssertDecimalEquals(ces.GetValue(8), 45.6199);
			TaTestsUtils.AssertDecimalEquals(ces.GetValue(9), 45.6099);
			TaTestsUtils.AssertDecimalEquals(ces.GetValue(10), 45.5459);
			TaTestsUtils.AssertDecimalEquals(ces.GetValue(11), 45.5807);
			TaTestsUtils.AssertDecimalEquals(ces.GetValue(12), 45.6126);
			TaTestsUtils.AssertDecimalEquals(ces.GetValue(13), 45.4781);
			TaTestsUtils.AssertDecimalEquals(ces.GetValue(14), 45.0605);
		}
	}
}