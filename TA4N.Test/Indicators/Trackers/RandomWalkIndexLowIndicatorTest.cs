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
namespace TA4N.Test.Indicators.Trackers
{
    using NUnit.Framework;
    using TA4N.Mocks;
    using TA4N.Indicators.Trackers;

    /// <summary>
    /// The Class RandomWalkIndexLowIndicatorTest.
    /// </summary>
    public sealed class RandomWalkIndexLowIndicatorTest
	{
	    private TimeSeries _data;
        
        [SetUp]
		public void SetUp()
		{
			IList<Tick> ticks = new List<Tick>();
			ticks.Add(new MockTick(44.98, 45.05, 45.17, 44.96));
			ticks.Add(new MockTick(45.05, 45.10, 45.15, 44.99));
			ticks.Add(new MockTick(45.11, 45.19, 45.32, 45.11));
			ticks.Add(new MockTick(45.19, 45.14, 45.25, 45.04));
			ticks.Add(new MockTick(45.12, 45.15, 45.20, 45.10));
			ticks.Add(new MockTick(45.15, 45.14, 45.20, 45.10));
			ticks.Add(new MockTick(45.13, 45.10, 45.16, 45.07));
			ticks.Add(new MockTick(45.12, 45.15, 45.22, 45.10));
			ticks.Add(new MockTick(45.15, 45.22, 45.27, 45.14));
			ticks.Add(new MockTick(45.24, 45.43, 45.45, 45.20));
			ticks.Add(new MockTick(45.43, 45.44, 45.50, 45.39));
			ticks.Add(new MockTick(45.43, 45.55, 45.60, 45.35));
			ticks.Add(new MockTick(45.58, 45.55, 45.61, 45.39));
			ticks.Add(new MockTick(45.45, 45.01, 45.55, 44.80));
			ticks.Add(new MockTick(45.03, 44.23, 45.04, 44.17));
			ticks.Add(new MockTick(44.23, 43.95, 44.29, 43.81));
			ticks.Add(new MockTick(43.91, 43.08, 43.99, 43.08));
			ticks.Add(new MockTick(43.07, 43.55, 43.65, 43.06));
			ticks.Add(new MockTick(43.56, 43.95, 43.99, 43.53));
			ticks.Add(new MockTick(43.93, 44.47, 44.58, 43.93));
			_data = new MockTimeSeries(ticks);
		}
        
        [Test] 
		public void RandomWalkIndexLow()
		{
			var rwil = new RandomWalkIndexLowIndicator(_data, 5);

			TaTestsUtils.AssertDecimalEquals(rwil.GetValue(6), 0.0997);
			TaTestsUtils.AssertDecimalEquals(rwil.GetValue(7), 0.3162);
			TaTestsUtils.AssertDecimalEquals(rwil.GetValue(8), 0.1789);
			TaTestsUtils.AssertDecimalEquals(rwil.GetValue(9), 0.0000);
			TaTestsUtils.AssertDecimalEquals(rwil.GetValue(10), -0.3571);
			TaTestsUtils.AssertDecimalEquals(rwil.GetValue(11), -0.3535);
			TaTestsUtils.AssertDecimalEquals(rwil.GetValue(12), -0.3217);
			TaTestsUtils.AssertDecimalEquals(rwil.GetValue(13), 0.6200);
			TaTestsUtils.AssertDecimalEquals(rwil.GetValue(14), 1.2857);
			TaTestsUtils.AssertDecimalEquals(rwil.GetValue(15), 1.6714);
			TaTestsUtils.AssertDecimalEquals(rwil.GetValue(16), 2.0726);
			TaTestsUtils.AssertDecimalEquals(rwil.GetValue(17), 2.0622);
			TaTestsUtils.AssertDecimalEquals(rwil.GetValue(18), 1.6905);
		}
	}
}