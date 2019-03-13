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
	public sealed class MaximumDrawdownCriterionTest
	{
        [Test] 
		public void CalculateWithNoTrades()
		{
			var series = new MockTimeSeries(1, 2, 3, 6, 5, 20, 3);
			var mdd = new MaximumDrawdownCriterion();

			Assert.AreEqual(0d, mdd.Calculate(series, new TradingRecord()), TaTestsUtils.TaOffset);
		}

        [Test] 
		public void CalculateWithOnlyGains()
		{
			var series = new MockTimeSeries(1, 2, 3, 6, 8, 20, 3);
			var mdd = new MaximumDrawdownCriterion();
			var tradingRecord = new TradingRecord(Order.BuyAt(0), Order.SellAt(1), Order.BuyAt(2), Order.SellAt(5));

			Assert.AreEqual(0d, mdd.Calculate(series, tradingRecord), TaTestsUtils.TaOffset);
		}

        [Test] 
		public void CalculateShouldWork()
		{
			var series = new MockTimeSeries(1, 2, 3, 6, 5, 20, 3);
			var mdd = new MaximumDrawdownCriterion();
			var tradingRecord = new TradingRecord(Order.BuyAt(0), Order.SellAt(1), Order.BuyAt(3), Order.SellAt(4), Order.BuyAt(5), Order.SellAt(6));

			Assert.AreEqual(.875d, mdd.Calculate(series, tradingRecord), TaTestsUtils.TaOffset);

		}

        [Test] 
		public void CalculateWithNullSeriesSizeShouldReturn0()
		{
			var series = new MockTimeSeries(new double[] {});
			var mdd = new MaximumDrawdownCriterion();
			Assert.AreEqual(0d, mdd.Calculate(series, new TradingRecord()), TaTestsUtils.TaOffset);
		}

        [Test] 
		public void WithTradesThatSellBeforeBuying()
		{
			var series = new MockTimeSeries(2, 1, 3, 5, 6, 3, 20);
			var mdd = new MaximumDrawdownCriterion();
			var tradingRecord = new TradingRecord(Order.BuyAt(0), Order.SellAt(1), Order.BuyAt(3), Order.SellAt(4), Order.SellAt(5), Order.BuyAt(6));
			Assert.AreEqual(.91, mdd.Calculate(series, tradingRecord), TaTestsUtils.TaOffset);
		}

        [Test] 
		public void WithSimpleTrades()
		{
			var series = new MockTimeSeries(1, 10, 5, 6, 1);
			var mdd = new MaximumDrawdownCriterion();
			var tradingRecord = new TradingRecord(Order.BuyAt(0), Order.SellAt(1), Order.BuyAt(1), Order.SellAt(2), Order.BuyAt(2), Order.SellAt(3), Order.BuyAt(3), Order.SellAt(4));
			Assert.AreEqual(.9d, mdd.Calculate(series, tradingRecord), TaTestsUtils.TaOffset);
		}

        [Test] 
		public void WithConstrainedTimeSeries()
		{
			var sampleSeries = new MockTimeSeries(new double[] {1, 1, 1, 1, 1, 10, 5, 6, 1, 1, 1});
			var subSeries = sampleSeries.Subseries(4, 8);
			var mdd = new MaximumDrawdownCriterion();
			var tradingRecord = new TradingRecord(Order.BuyAt(4), Order.SellAt(5), Order.BuyAt(5), Order.SellAt(6), Order.BuyAt(6), Order.SellAt(7), Order.BuyAt(7), Order.SellAt(8));
			Assert.AreEqual(.9d, mdd.Calculate(subSeries, tradingRecord), TaTestsUtils.TaOffset);
		}

        [Test] 
		public void BetterThan()
		{
			IAnalysisCriterion criterion = new MaximumDrawdownCriterion();
			Assert.IsTrue(criterion.BetterThan(0.9, 1.5));
			Assert.IsFalse(criterion.BetterThan(1.2, 0.4));
		}
	}
}