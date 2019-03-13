using System.Collections.Generic;
using TA4N.Mocks;

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
    using TA4N.Indicators.Simple;
	using MaxPriceIndicator = TA4N.Indicators.Simple.MaxPriceIndicator;
	using MinPriceIndicator = TA4N.Indicators.Simple.MinPriceIndicator;
    using NUnit.Framework;
    using TA4N.Indicators.Trackers;

    public sealed class WilliamsRIndicatorTest
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

			_data = new TimeSeries(ticks);
		}

        [Test] 
		public void WilliamsRUsingTimeFrame5UsingClosePrice()
		{
			var wr = new WilliamsRIndicator(new ClosePriceIndicator(_data), 5, new MaxPriceIndicator(_data), new MinPriceIndicator(_data));

			TaTestsUtils.AssertDecimalEquals(wr.GetValue(4), -47.2222);
			TaTestsUtils.AssertDecimalEquals(wr.GetValue(5), -54.5454);
			TaTestsUtils.AssertDecimalEquals(wr.GetValue(6), -78.5714);
			TaTestsUtils.AssertDecimalEquals(wr.GetValue(7), -47.6190);
			TaTestsUtils.AssertDecimalEquals(wr.GetValue(8), -25d);
			TaTestsUtils.AssertDecimalEquals(wr.GetValue(9), -5.2632);
			TaTestsUtils.AssertDecimalEquals(wr.GetValue(10), -13.9535);
		}

        [Test] 
		public void WilliamsRUsingTimeFrame10UsingClosePrice()
		{
			var wr = new WilliamsRIndicator(new ClosePriceIndicator(_data), 10, new MaxPriceIndicator(_data), new MinPriceIndicator(_data));

			TaTestsUtils.AssertDecimalEquals(wr.GetValue(9), -4.0816);
			TaTestsUtils.AssertDecimalEquals(wr.GetValue(10), -11.7647);
			TaTestsUtils.AssertDecimalEquals(wr.GetValue(11), -8.9286);
			TaTestsUtils.AssertDecimalEquals(wr.GetValue(12), -10.5263);
		}

        [Test] 
		public void ValueLessThenTimeFrame()
		{
			var wr = new WilliamsRIndicator(new ClosePriceIndicator(_data), 100, new MaxPriceIndicator(_data), new MinPriceIndicator(_data));

			TaTestsUtils.AssertDecimalEquals(wr.GetValue(0), -100d * (0.12 / 0.21));
			TaTestsUtils.AssertDecimalEquals(wr.GetValue(1), -100d * (0.07 / 0.21));
			TaTestsUtils.AssertDecimalEquals(wr.GetValue(2), -100d * (0.13 / 0.36));
			TaTestsUtils.AssertDecimalEquals(wr.GetValue(3), -100d * (0.18 / 0.36));
		}
	}
}