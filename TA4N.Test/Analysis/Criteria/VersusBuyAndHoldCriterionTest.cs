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
	public sealed class VersusBuyAndHoldCriterionTest
	{
        [Test]
		public void CalculateOnlyWithGainTrades()
		{
			var series = new MockTimeSeries(100, 105, 110, 100, 95, 105);
			var tradingRecord = new TradingRecord(Order.BuyAt(0), Order.SellAt(2), Order.BuyAt(3), Order.SellAt(5));

			IAnalysisCriterion buyAndHold = new VersusBuyAndHoldCriterion(new TotalProfitCriterion());
			Assert.AreEqual(1.10 * 1.05 / 1.05, buyAndHold.Calculate(series, tradingRecord), TaTestsUtils.TaOffset);
		}

        [Test]
		public void CalculateOnlyWithLossTrades()
		{
			var series = new MockTimeSeries(100, 95, 100, 80, 85, 70);
			var tradingRecord = new TradingRecord(Order.BuyAt(0), Order.SellAt(1), Order.BuyAt(2), Order.SellAt(5));

			IAnalysisCriterion buyAndHold = new VersusBuyAndHoldCriterion(new TotalProfitCriterion());
			Assert.AreEqual(0.95 * 0.7 / 0.7, buyAndHold.Calculate(series, tradingRecord), TaTestsUtils.TaOffset);
		}

        [Test] 
		public void CalculateWithOnlyOneTrade()
		{
			var series = new MockTimeSeries(100, 95, 100, 80, 85, 70);
			var trade = new Trade(Order.BuyAt(0), Order.SellAt(1));

			IAnalysisCriterion buyAndHold = new VersusBuyAndHoldCriterion(new TotalProfitCriterion());
			Assert.AreEqual((100d / 70) / (100d / 95), buyAndHold.Calculate(series, trade), TaTestsUtils.TaOffset);
		}

        [Test] 
		public void CalculateWithNoTrades()
		{
			var series = new MockTimeSeries(100, 95, 100, 80, 85, 70);

			IAnalysisCriterion buyAndHold = new VersusBuyAndHoldCriterion(new TotalProfitCriterion());
			Assert.AreEqual(1 / 0.7, buyAndHold.Calculate(series, new TradingRecord()), TaTestsUtils.TaOffset);
		}

        [Test] 
		public void CalculateWithAverageProfit()
		{
			var series = new MockTimeSeries(100, 95, 100, 80, 85, 130);
			var tradingRecord = new TradingRecord(Order.BuyAt(0), Order.SellAt(1), Order.BuyAt(2), Order.SellAt(5));

			IAnalysisCriterion buyAndHold = new VersusBuyAndHoldCriterion(new AverageProfitCriterion());

			Assert.AreEqual(Math.Pow(95d / 100 * 130d / 100, 1d / 6) / Math.Pow(130d / 100, 1d / 6), buyAndHold.Calculate(series, tradingRecord), TaTestsUtils.TaOffset);
		}

        [Test] 
		public void CalculateWithNumberOfTicks()
		{
			var series = new MockTimeSeries(100, 95, 100, 80, 85, 130);
			var tradingRecord = new TradingRecord(Order.BuyAt(0), Order.SellAt(1), Order.BuyAt(2), Order.SellAt(5));

			IAnalysisCriterion buyAndHold = new VersusBuyAndHoldCriterion(new NumberOfTicksCriterion());

			Assert.AreEqual(6d / 6d, buyAndHold.Calculate(series, tradingRecord), TaTestsUtils.TaOffset);
		}

        [Test]
		public void BetterThan()
		{
			IAnalysisCriterion criterion = new VersusBuyAndHoldCriterion(new TotalProfitCriterion());
			Assert.IsTrue(criterion.BetterThan(2.0, 1.5));
			Assert.IsFalse(criterion.BetterThan(1.5, 2.0));
		}
	}
}