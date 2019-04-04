using System.Collections.Generic;
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

    public sealed class AccelerationDecelerationIndicatorTest
	{
        private TimeSeries _series;
        
        [SetUp]
		public void SetUp()
		{
			IList<Tick> ticks = new List<Tick>();

			ticks.Add(GenerateTick.From(0, 0, 16, 8));
			ticks.Add(GenerateTick.From(0, 0, 12, 6));
			ticks.Add(GenerateTick.From(0, 0, 18, 14));
			ticks.Add(GenerateTick.From(0, 0, 10, 6));
			ticks.Add(GenerateTick.From(0, 0, 8, 4));

			_series = GenerateTimeSeries.From(ticks);
		}

        [Test] 
		public void CalculateWithSma2AndSma3()
		{
			var acceleration = new AccelerationDecelerationIndicator(_series, 2, 3);

			TaTestsUtils.AssertDecimalEquals(acceleration.GetValue(0), 0);
			TaTestsUtils.AssertDecimalEquals(acceleration.GetValue(1), 0);
			TaTestsUtils.AssertDecimalEquals(acceleration.GetValue(2), 0.08333333333);
			TaTestsUtils.AssertDecimalEquals(acceleration.GetValue(3), 0.41666666666);
			TaTestsUtils.AssertDecimalEquals(acceleration.GetValue(4), -2);
		}

        [Test]
		public void WithSma1AndSma2()
		{
			var acceleration = new AccelerationDecelerationIndicator(_series, 1, 2);

			TaTestsUtils.AssertDecimalEquals(acceleration.GetValue(0), 0);
			TaTestsUtils.AssertDecimalEquals(acceleration.GetValue(1), 0);
			TaTestsUtils.AssertDecimalEquals(acceleration.GetValue(2), 0);
			TaTestsUtils.AssertDecimalEquals(acceleration.GetValue(3), 0);
			TaTestsUtils.AssertDecimalEquals(acceleration.GetValue(4), 0);
		}
        
        [Test] 
		public void WithSmaDefault()
		{
			var acceleration = new AccelerationDecelerationIndicator(_series);

			TaTestsUtils.AssertDecimalEquals(acceleration.GetValue(0), 0);
			TaTestsUtils.AssertDecimalEquals(acceleration.GetValue(1), 0);
			TaTestsUtils.AssertDecimalEquals(acceleration.GetValue(2), 0);
			TaTestsUtils.AssertDecimalEquals(acceleration.GetValue(3), 0);
			TaTestsUtils.AssertDecimalEquals(acceleration.GetValue(4), 0);
		}
	}
}