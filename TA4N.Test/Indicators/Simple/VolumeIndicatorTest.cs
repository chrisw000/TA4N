/*
The MIT License (MIT)

Copyright (c) 2014-2016 Marc de Verdelhan & respective authors (see AUTHORS)

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System.Collections.Generic;
using NUnit.Framework;
using TA4N.Mocks;
using TA4N.Indicators.Simple;

namespace TA4N.Test.Indicators.Simple
{
	public sealed class VolumeIndicatorTest
	{
        [Test]
		public void IndicatorShouldRetrieveTickVolume()
		{
			TimeSeries series = new MockTimeSeries();
			var volumeIndicator = new VolumeIndicator(series);
			for (var i = 0; i < 10; i++)
			{
				Assert.AreEqual(volumeIndicator.GetValue(i), series.GetTick(i).Volume);
			}
		}

        [Test]
		public void SumOfVolume()
		{
			IList<Tick> ticks = new List<Tick>();
			ticks.Add(new MockTick(0, 10));
			ticks.Add(new MockTick(0, 11));
			ticks.Add(new MockTick(0, 12));
			ticks.Add(new MockTick(0, 13));
			ticks.Add(new MockTick(0, 150));
			ticks.Add(new MockTick(0, 155));
			ticks.Add(new MockTick(0, 160));
			var volumeIndicator = new VolumeIndicator(new MockTimeSeries(ticks), 3);

			TaTestsUtils.AssertDecimalEquals(volumeIndicator.GetValue(0), 10);
			TaTestsUtils.AssertDecimalEquals(volumeIndicator.GetValue(1), 21);
			TaTestsUtils.AssertDecimalEquals(volumeIndicator.GetValue(2), 33);
			TaTestsUtils.AssertDecimalEquals(volumeIndicator.GetValue(3), 36);
			TaTestsUtils.AssertDecimalEquals(volumeIndicator.GetValue(4), 175);
			TaTestsUtils.AssertDecimalEquals(volumeIndicator.GetValue(5), 318);
			TaTestsUtils.AssertDecimalEquals(volumeIndicator.GetValue(6), 465);
		}
	}
}