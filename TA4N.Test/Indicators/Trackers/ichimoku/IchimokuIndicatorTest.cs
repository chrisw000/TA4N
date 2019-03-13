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
namespace TA4N.Test.Indicators.Trackers.Ichimoku
{
    using NUnit.Framework;
    using TA4N.Mocks;
    using TA4N.Indicators.Trackers.Ichimoku;

    public sealed class IchimokuIndicatorTest
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
		public void Ichimoku()
		{
			var tenkanSen = new IchimokuTenkanSenIndicator(_data, 3);
			var kijunSen = new IchimokuKijunSenIndicator(_data, 5);
			var senkouSpanA = new IchimokuSenkouSpanAIndicator(_data, tenkanSen, kijunSen);
			var senkouSpanB = new IchimokuSenkouSpanBIndicator(_data, 9);
			var chikouSpan = new IchimokuChikouSpanIndicator(_data, 5);

			TaTestsUtils.AssertDecimalEquals(tenkanSen.GetValue(3), 45.155);
			TaTestsUtils.AssertDecimalEquals(tenkanSen.GetValue(4), 45.18);
			TaTestsUtils.AssertDecimalEquals(tenkanSen.GetValue(5), 45.145);
			TaTestsUtils.AssertDecimalEquals(tenkanSen.GetValue(6), 45.135);
			TaTestsUtils.AssertDecimalEquals(tenkanSen.GetValue(7), 45.145);
			TaTestsUtils.AssertDecimalEquals(tenkanSen.GetValue(8), 45.17);
			TaTestsUtils.AssertDecimalEquals(tenkanSen.GetValue(16), 44.06);
			TaTestsUtils.AssertDecimalEquals(tenkanSen.GetValue(17), 43.675);
			TaTestsUtils.AssertDecimalEquals(tenkanSen.GetValue(18), 43.525);

			TaTestsUtils.AssertDecimalEquals(kijunSen.GetValue(3), 45.14);
			TaTestsUtils.AssertDecimalEquals(kijunSen.GetValue(4), 45.14);
			TaTestsUtils.AssertDecimalEquals(kijunSen.GetValue(5), 45.155);
			TaTestsUtils.AssertDecimalEquals(kijunSen.GetValue(6), 45.18);
			TaTestsUtils.AssertDecimalEquals(kijunSen.GetValue(7), 45.145);
			TaTestsUtils.AssertDecimalEquals(kijunSen.GetValue(8), 45.17);
			TaTestsUtils.AssertDecimalEquals(kijunSen.GetValue(16), 44.345);
			TaTestsUtils.AssertDecimalEquals(kijunSen.GetValue(17), 44.305);
			TaTestsUtils.AssertDecimalEquals(kijunSen.GetValue(18), 44.05);

			TaTestsUtils.AssertDecimalEquals(senkouSpanA.GetValue(3), 45.1475);
			TaTestsUtils.AssertDecimalEquals(senkouSpanA.GetValue(4), 45.16);
			TaTestsUtils.AssertDecimalEquals(senkouSpanA.GetValue(5), 45.15);
			TaTestsUtils.AssertDecimalEquals(senkouSpanA.GetValue(6), 45.1575);
			TaTestsUtils.AssertDecimalEquals(senkouSpanA.GetValue(7), 45.145);
			TaTestsUtils.AssertDecimalEquals(senkouSpanA.GetValue(8), 45.17);
			TaTestsUtils.AssertDecimalEquals(senkouSpanA.GetValue(16), 44.2025);
			TaTestsUtils.AssertDecimalEquals(senkouSpanA.GetValue(17), 43.99);
			TaTestsUtils.AssertDecimalEquals(senkouSpanA.GetValue(18), 43.7875);

			TaTestsUtils.AssertDecimalEquals(senkouSpanB.GetValue(3), 45.14);
			TaTestsUtils.AssertDecimalEquals(senkouSpanB.GetValue(4), 45.14);
			TaTestsUtils.AssertDecimalEquals(senkouSpanB.GetValue(5), 45.14);
			TaTestsUtils.AssertDecimalEquals(senkouSpanB.GetValue(6), 45.14);
			TaTestsUtils.AssertDecimalEquals(senkouSpanB.GetValue(7), 45.14);
			TaTestsUtils.AssertDecimalEquals(senkouSpanB.GetValue(8), 45.14);
			TaTestsUtils.AssertDecimalEquals(senkouSpanB.GetValue(16), 44.345);
			TaTestsUtils.AssertDecimalEquals(senkouSpanB.GetValue(17), 44.335);
			TaTestsUtils.AssertDecimalEquals(senkouSpanB.GetValue(18), 44.335);

			TaTestsUtils.AssertDecimalEquals(chikouSpan.GetValue(5), 45.05);
			TaTestsUtils.AssertDecimalEquals(chikouSpan.GetValue(6), 45.10);
			TaTestsUtils.AssertDecimalEquals(chikouSpan.GetValue(7), 45.19);
			TaTestsUtils.AssertDecimalEquals(chikouSpan.GetValue(8), 45.14);
			TaTestsUtils.AssertDecimalEquals(chikouSpan.GetValue(19), 44.23);
		}
	}
}