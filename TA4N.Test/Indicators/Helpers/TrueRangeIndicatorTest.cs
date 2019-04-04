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
namespace TA4N.Test.Indicators.Helpers
{
    using NUnit.Framework;
    using TA4N.Indicators.Helpers;

    public sealed class TrueRangeIndicatorTest
	{
        [Test]
		public void GetValue()
		{
			IList<Tick> ticks = new List<Tick>();
			ticks.Add(GenerateTick.From(0, 12, 15, 8));
			ticks.Add(GenerateTick.From(0, 8, 11, 6));
			ticks.Add(GenerateTick.From(0, 15, 17, 14));
			ticks.Add(GenerateTick.From(0, 15, 17, 14));
			ticks.Add(GenerateTick.From(0, 0, 0, 2));
			var tr = new TrueRangeIndicator(GenerateTimeSeries.From(ticks));

			TaTestsUtils.AssertDecimalEquals(tr.GetValue(0), 7);
			TaTestsUtils.AssertDecimalEquals(tr.GetValue(1), 6);
			TaTestsUtils.AssertDecimalEquals(tr.GetValue(2), 9);
			TaTestsUtils.AssertDecimalEquals(tr.GetValue(3), 3);
			TaTestsUtils.AssertDecimalEquals(tr.GetValue(4), 15);
		}
	}
}