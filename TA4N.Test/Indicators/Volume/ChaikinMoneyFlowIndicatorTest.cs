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
namespace TA4N.Test.Indicators.Volume
{
	using NodaTime;
    using NUnit.Framework;
    using TA4N.Indicators.Volume;

    public sealed class ChaikinMoneyFlowIndicatorTest
	{
        [Test]
		public void GetValue()
		{
			var now = new LocalDateTime();
			IList<Tick> ticks = new List<Tick>();
			ticks.Add(new Tick(now, "0", "62.34", "61.37", "62.15", "7849.025"));
			ticks.Add(new Tick(now, "0", "62.05", "60.69", "60.81", "11692.075"));
			ticks.Add(new Tick(now, "0", "62.27", "60.10", "60.45", "10575.307"));
			ticks.Add(new Tick(now, "0", "60.79", "58.61", "59.18", "13059.128"));
			ticks.Add(new Tick(now, "0", "59.93", "58.71", "59.24", "20733.508"));
			ticks.Add(new Tick(now, "0", "61.75", "59.86", "60.20", "29630.096"));
			ticks.Add(new Tick(now, "0", "60.00", "57.97", "58.48", "17705.294"));
			ticks.Add(new Tick(now, "0", "59.00", "58.02", "58.24", "7259.203"));
			ticks.Add(new Tick(now, "0", "59.07", "57.48", "58.69", "10474.629"));
			ticks.Add(new Tick(now, "0", "59.22", "58.30", "58.65", "5203.714"));
			ticks.Add(new Tick(now, "0", "58.75", "57.83", "58.47", "3422.865"));
			ticks.Add(new Tick(now, "0", "58.65", "57.86", "58.02", "3962.150"));
			ticks.Add(new Tick(now, "0", "58.47", "57.91", "58.17", "4095.905"));
			ticks.Add(new Tick(now, "0", "58.25", "57.83", "58.07", "3766.006"));
			ticks.Add(new Tick(now, "0", "58.35", "57.53", "58.13", "4239.335"));
			ticks.Add(new Tick(now, "0", "59.86", "58.58", "58.94", "8039.979"));
			ticks.Add(new Tick(now, "0", "59.53", "58.30", "59.10", "6956.717"));
			ticks.Add(new Tick(now, "0", "62.10", "58.53", "61.92", "18171.552"));
			ticks.Add(new Tick(now, "0", "62.16", "59.80", "61.37", "22225.894"));

			ticks.Add(new Tick(now, "0", "62.67", "60.93", "61.68", "14613.509"));
			ticks.Add(new Tick(now, "0", "62.38", "60.15", "62.09", "12319.763"));
			ticks.Add(new Tick(now, "0", "63.73", "62.26", "62.89", "15007.690"));
			ticks.Add(new Tick(now, "0", "63.85", "63.00", "63.53", "8879.667"));
			ticks.Add(new Tick(now, "0", "66.15", "63.58", "64.01", "22693.812"));
			ticks.Add(new Tick(now, "0", "65.34", "64.07", "64.77", "10191.814"));
			ticks.Add(new Tick(now, "0", "66.48", "65.20", "65.22", "10074.152"));
			ticks.Add(new Tick(now, "0", "65.23", "63.21", "63.28", "9411.620"));
			ticks.Add(new Tick(now, "0", "63.40", "61.88", "62.40", "10391.690"));
			ticks.Add(new Tick(now, "0", "63.18", "61.11", "61.55", "8926.512"));
			ticks.Add(new Tick(now, "0", "62.70", "61.25", "62.69", "7459.575"));
			var series = new TimeSeries(ticks);

			var cmf = new ChaikinMoneyFlowIndicator(series, 20);

			TaTestsUtils.AssertDecimalEquals(cmf.GetValue(0), 0.6082);
			TaTestsUtils.AssertDecimalEquals(cmf.GetValue(1), -0.2484);
			TaTestsUtils.AssertDecimalEquals(cmf.GetValue(19), -0.1211);
			TaTestsUtils.AssertDecimalEquals(cmf.GetValue(20), -0.0997);
			TaTestsUtils.AssertDecimalEquals(cmf.GetValue(21), -0.0659);
			TaTestsUtils.AssertDecimalEquals(cmf.GetValue(22), -0.0257);
			TaTestsUtils.AssertDecimalEquals(cmf.GetValue(23), -0.0617);
			TaTestsUtils.AssertDecimalEquals(cmf.GetValue(24), -0.0481);
			TaTestsUtils.AssertDecimalEquals(cmf.GetValue(25), -0.0086);
			TaTestsUtils.AssertDecimalEquals(cmf.GetValue(26), -0.0087);
			TaTestsUtils.AssertDecimalEquals(cmf.GetValue(27), -0.005);
			TaTestsUtils.AssertDecimalEquals(cmf.GetValue(28), -0.0574);
			TaTestsUtils.AssertDecimalEquals(cmf.GetValue(29), -0.0148);
		}
	}
}