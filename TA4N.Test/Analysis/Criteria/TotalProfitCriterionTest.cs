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

using TA4N.Mocks;
using NUnit.Framework;

namespace TA4N.Analysis.Criteria
{
	public sealed class TotalProfitCriterionTest
	{
        [Test] 
		public void CalculateOnlyWithGainTrades()
		{
			var series = new MockTimeSeries(100, 105, 110, 100, 95, 105);
			var tradingRecord = new TradingRecord(Order.BuyAt(0), Order.SellAt(2), Order.BuyAt(3), Order.SellAt(5));

			IAnalysisCriterion profit = new TotalProfitCriterion();
			Assert.AreEqual(1.10 * 1.05, profit.Calculate(series, tradingRecord), TaTestsUtils.TaOffset);
		}

        [Test]
		public void CalculateOnlyWithLossTrades()
		{
			var series = new MockTimeSeries(100, 95, 100, 80, 85, 70);
			var tradingRecord = new TradingRecord(Order.BuyAt(0), Order.SellAt(1), Order.BuyAt(2), Order.SellAt(5));

			IAnalysisCriterion profit = new TotalProfitCriterion();
			Assert.AreEqual(0.95 * 0.7, profit.Calculate(series, tradingRecord), TaTestsUtils.TaOffset);
		}

        [Test]
		public void CalculateProfitWithTradesThatStartSelling()
		{
			var series = new MockTimeSeries(100, 95, 100, 80, 85, 70);
			var tradingRecord = new TradingRecord(Order.SellAt(0), Order.BuyAt(1), Order.SellAt(2), Order.BuyAt(5));

			IAnalysisCriterion profit = new TotalProfitCriterion();
			Assert.AreEqual((1 / 0.95) * (1 / 0.7), profit.Calculate(series, tradingRecord), TaTestsUtils.TaOffset);
		}
        
        [Test] 
		public void CalculateWithNoTradesShouldReturn1()
		{
			var series = new MockTimeSeries(100, 95, 100, 80, 85, 70);

			IAnalysisCriterion profit = new TotalProfitCriterion();
			Assert.AreEqual(1d, profit.Calculate(series, new TradingRecord()), TaTestsUtils.TaOffset);
		}

        [Test] 
		public void CalculateWithOpenedTradeShouldReturn1()
		{
			var series = new MockTimeSeries(100, 95, 100, 80, 85, 70);
			IAnalysisCriterion profit = new TotalProfitCriterion();
			var trade = new Trade();
			Assert.AreEqual(1d, profit.Calculate(series, trade), TaTestsUtils.TaOffset);
			trade.Operate(0);
			Assert.AreEqual(1d, profit.Calculate(series, trade), TaTestsUtils.TaOffset);
		}

        [Test]
		public void BetterThan()
		{
			IAnalysisCriterion criterion = new TotalProfitCriterion();
			Assert.IsTrue(criterion.BetterThan(2.0, 1.5));
			Assert.IsFalse(criterion.BetterThan(1.5, 2.0));
		}
	}
}