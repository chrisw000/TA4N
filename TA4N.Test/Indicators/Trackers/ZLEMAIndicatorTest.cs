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

using TA4N.Test.FixtureData;

namespace TA4N.Test.Indicators.Trackers
{
    using TA4N.Indicators.Simple;
    using NUnit.Framework;
    using TA4N.Indicators.Trackers;

    public sealed class ZlemaIndicatorTest
	{
		private TimeSeries _data;

        [SetUp]
		public void SetUp()
		{
			_data = GenerateTimeSeries.From(10, 15, 20, 18, 17, 18, 15, 12, 10, 8, 5, 2);
		}

        [Test] 
		public void ZlemaUsingTimeFrame10UsingClosePrice()
		{
			var zlema = new ZlemaIndicator(new ClosePriceIndicator(_data), 10);

			TaTestsUtils.AssertDecimalEquals(zlema.GetValue(9), 11.9091);
			TaTestsUtils.AssertDecimalEquals(zlema.GetValue(10), 8.8347);
			TaTestsUtils.AssertDecimalEquals(zlema.GetValue(11), 5.7739);
		}

        [Test] 
		public void ZlemaFirstValueShouldBeEqualsToFirstDataValue()
		{
			var zlema = new ZlemaIndicator(new ClosePriceIndicator(_data), 10);
			TaTestsUtils.AssertDecimalEquals(zlema.GetValue(0), "10");
		}

        [Test] 
		public void ValuesLessThanTimeFrameMustBeEqualsToSmaValues()
		{
			var zlema = new ZlemaIndicator(new ClosePriceIndicator(_data), 10);
			var sma = new SmaIndicator(new ClosePriceIndicator(_data), 10);

			for (var i = 0; i < 9; i++)
			{
				Assert.AreEqual(sma.GetValue(i), zlema.GetValue(i));
			}
		}

        [Test] 
		public void SmallTimeFrame()
		{
			var zlema = new ZlemaIndicator(new ClosePriceIndicator(_data), 1);
			TaTestsUtils.AssertDecimalEquals(zlema.GetValue(0), "10");
		}
	}
}