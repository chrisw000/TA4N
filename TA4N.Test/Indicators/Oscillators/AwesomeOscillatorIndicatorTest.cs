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
namespace TA4N.Test.Indicators.Oscillators
{
    using MedianPriceIndicator = TA4N.Indicators.Simple.MedianPriceIndicator;
    using TA4N.Mocks;
    using NUnit.Framework;
    using TA4N.Indicators.Oscillators;

    public sealed class AwesomeOscillatorIndicatorTest
	{
		private TimeSeries _series;
        
        [SetUp]
		public void SetUp()
		{
			IList<Tick> ticks = new List<Tick>();
			ticks.Add(new MockTick(0, 0, 16, 8));
			ticks.Add(new MockTick(0, 0, 12, 6));
			ticks.Add(new MockTick(0, 0, 18, 14));
			ticks.Add(new MockTick(0, 0, 10, 6));
			ticks.Add(new MockTick(0, 0, 8, 4));

			this._series = new MockTimeSeries(ticks);
		}

        [Test] 
		public void CalculateWithSma2AndSma3()
		{
			var awesome = new AwesomeOscillatorIndicator(new MedianPriceIndicator(_series), 2, 3);
            TaTestsUtils.AssertDecimalEquals(awesome.GetValue(0), 0);
			TaTestsUtils.AssertDecimalEquals(awesome.GetValue(1), 0);
			TaTestsUtils.AssertDecimalEquals(awesome.GetValue(2), 1d / 6);
			TaTestsUtils.AssertDecimalEquals(awesome.GetValue(3), 1);
			TaTestsUtils.AssertDecimalEquals(awesome.GetValue(4), -3);
		}

        [Test] 
		public void WithSma1AndSma2()
		{
			var awesome = new AwesomeOscillatorIndicator(new MedianPriceIndicator(_series), 1, 2);
            TaTestsUtils.AssertDecimalEquals(awesome.GetValue(0), 0);
			TaTestsUtils.AssertDecimalEquals(awesome.GetValue(1), "-1.5");
			TaTestsUtils.AssertDecimalEquals(awesome.GetValue(2), "3.5");
			TaTestsUtils.AssertDecimalEquals(awesome.GetValue(3), -4);
			TaTestsUtils.AssertDecimalEquals(awesome.GetValue(4), -1);
		}
        
        [Test]
		public void WithSmaDefault()
		{
			var awesome = new AwesomeOscillatorIndicator(new MedianPriceIndicator(_series));
			TaTestsUtils.AssertDecimalEquals(awesome.GetValue(0), 0);
			TaTestsUtils.AssertDecimalEquals(awesome.GetValue(1), 0);
			TaTestsUtils.AssertDecimalEquals(awesome.GetValue(2), 0);
			TaTestsUtils.AssertDecimalEquals(awesome.GetValue(3), 0);
			TaTestsUtils.AssertDecimalEquals(awesome.GetValue(4), 0);
		}
	}
}