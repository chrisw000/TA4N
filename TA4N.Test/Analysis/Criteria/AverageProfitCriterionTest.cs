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

using System;
using TA4N.Mocks;
using NUnit.Framework;

namespace TA4N.Analysis.Criteria
{
	public sealed class AverageProfitCriterionTest
	{
		private MockTimeSeries _series;

        [Test] 
		public void CalculateOnlyWithGainTrades()
		{
			_series = new MockTimeSeries(100d, 105d, 110d, 100d, 95d, 105d);
			var tradingRecord = new TradingRecord(Order.BuyAt(0), Order.SellAt(2), Order.BuyAt(3), Order.SellAt(5));
			IAnalysisCriterion averageProfit = new AverageProfitCriterion();
			Assert.AreEqual(1.0243, TaTestsUtils.TaOffset, averageProfit.Calculate(_series, tradingRecord));
		}

        [Test]
		public void CalculateWithASimpleTrade()
		{
			_series = new MockTimeSeries(100d, 105d, 110d, 100d, 95d, 105d);
			var tradingRecord = new TradingRecord(Order.BuyAt(0), Order.SellAt(2));
			IAnalysisCriterion averageProfit = new AverageProfitCriterion();
			Assert.AreEqual(Math.Pow(110d / 100, 1d / 3), averageProfit.Calculate(_series, tradingRecord), TaTestsUtils.TaOffset);
		}
        
        [Test]
		public void CalculateOnlyWithLossTrades()
		{
			_series = new MockTimeSeries(100, 95, 100, 80, 85, 70);
			var tradingRecord = new TradingRecord(Order.BuyAt(0), Order.SellAt(1), Order.BuyAt(2), Order.SellAt(5));
			IAnalysisCriterion averageProfit = new AverageProfitCriterion();
			Assert.AreEqual(Math.Pow(95d / 100 * 70d / 100, 1d / 6), averageProfit.Calculate(_series, tradingRecord), TaTestsUtils.TaOffset);
		}

        [Test] 
		public void CalculateWithNoTicksShouldReturn1()
		{
			_series = new MockTimeSeries(100, 95, 100, 80, 85, 70);
			IAnalysisCriterion averageProfit = new AverageProfitCriterion();
			Assert.AreEqual(1d, averageProfit.Calculate(_series, new TradingRecord()), TaTestsUtils.TaOffset);
		}

        [Test] 
		public void CalculateWithOneTrade()
		{
			_series = new MockTimeSeries(100, 105);
			var trade = new Trade(Order.BuyAt(0), Order.SellAt(1));
			IAnalysisCriterion average = new AverageProfitCriterion();
			Assert.AreEqual(Math.Pow(105d / 100, 1d / 2), average.Calculate(_series, trade), TaTestsUtils.TaOffset);
		}

        [Test] 
		public void BetterThan()
		{
			IAnalysisCriterion criterion = new AverageProfitCriterion();
			Assert.IsTrue(criterion.BetterThan(2.0, 1.5));
			Assert.IsFalse(criterion.BetterThan(1.5, 2.0));
		}
	}

}