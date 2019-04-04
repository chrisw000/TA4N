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
namespace TA4N.Test.Indicators.Volume
{
    using NUnit.Framework;
    using TA4N.Indicators.Volume;

    public sealed class MvwapIndicatorTest
	{
	    private TimeSeries _data;
        
        [SetUp]
		public void SetUp()
		{
			IList<Tick> ticks = new List<Tick>();
			ticks.Add(GenerateTick.From(44.98, 45.05, 45.17, 44.96, 1));
			ticks.Add(GenerateTick.From(45.05, 45.10, 45.15, 44.99, 2));
			ticks.Add(GenerateTick.From(45.11, 45.19, 45.32, 45.11, 1));
			ticks.Add(GenerateTick.From(45.19, 45.14, 45.25, 45.04, 3));
			ticks.Add(GenerateTick.From(45.12, 45.15, 45.20, 45.10, 1));
			ticks.Add(GenerateTick.From(45.15, 45.14, 45.20, 45.10, 2));
			ticks.Add(GenerateTick.From(45.13, 45.10, 45.16, 45.07, 1));
			ticks.Add(GenerateTick.From(45.12, 45.15, 45.22, 45.10, 5));
			ticks.Add(GenerateTick.From(45.15, 45.22, 45.27, 45.14, 1));
			ticks.Add(GenerateTick.From(45.24, 45.43, 45.45, 45.20, 1));
			ticks.Add(GenerateTick.From(45.43, 45.44, 45.50, 45.39, 1));
			ticks.Add(GenerateTick.From(45.43, 45.55, 45.60, 45.35, 5));
			ticks.Add(GenerateTick.From(45.58, 45.55, 45.61, 45.39, 7));
			ticks.Add(GenerateTick.From(45.45, 45.01, 45.55, 44.80, 6));
			ticks.Add(GenerateTick.From(45.03, 44.23, 45.04, 44.17, 1));
			ticks.Add(GenerateTick.From(44.23, 43.95, 44.29, 43.81, 2));
			ticks.Add(GenerateTick.From(43.91, 43.08, 43.99, 43.08, 1));
			ticks.Add(GenerateTick.From(43.07, 43.55, 43.65, 43.06, 7));
			ticks.Add(GenerateTick.From(43.56, 43.95, 43.99, 43.53, 6));
			ticks.Add(GenerateTick.From(43.93, 44.47, 44.58, 43.93, 1));
			_data = GenerateTimeSeries.From(ticks);
		}
        
        [Test]
		public void Mvwap()
		{
			var vwap = new VwapIndicator(_data, 5);
			var mvwap = new MvwapIndicator(vwap, 8);

			TaTestsUtils.AssertDecimalEquals(mvwap.GetValue(8), 45.1271);
			TaTestsUtils.AssertDecimalEquals(mvwap.GetValue(9), 45.1399);
			TaTestsUtils.AssertDecimalEquals(mvwap.GetValue(10), 45.1530);
			TaTestsUtils.AssertDecimalEquals(mvwap.GetValue(11), 45.1790);
			TaTestsUtils.AssertDecimalEquals(mvwap.GetValue(12), 45.2227);
			TaTestsUtils.AssertDecimalEquals(mvwap.GetValue(13), 45.2533);
			TaTestsUtils.AssertDecimalEquals(mvwap.GetValue(14), 45.2769);
			TaTestsUtils.AssertDecimalEquals(mvwap.GetValue(15), 45.2844);
			TaTestsUtils.AssertDecimalEquals(mvwap.GetValue(16), 45.2668);
			TaTestsUtils.AssertDecimalEquals(mvwap.GetValue(17), 45.1386);
			TaTestsUtils.AssertDecimalEquals(mvwap.GetValue(18), 44.9487);
		}
	}
}