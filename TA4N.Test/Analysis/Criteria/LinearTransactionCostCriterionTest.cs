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

using NUnit.Framework;
using TA4N.Analysis.Criteria;
using TA4N.Test.FixtureData;

namespace TA4N.Test.Analysis.Criteria
{
	public sealed class LinearTransactionCostCriterionTest
	{
        [Test] 
		public void CalculateLinearCost()
		{
			var series = GenerateTimeSeries.From(100, 150, 200, 100, 50, 100);
			IAnalysisCriterion transactionCost = new LinearTransactionCostCriterion(1000, 0.005, 0.2);

			var tradingRecord = new TradingRecord(Order.BuyAt(0), Order.SellAt(1));
			Assert.AreEqual(12.861, transactionCost.Calculate(series, tradingRecord), TaTestsUtils.TaOffset);

			tradingRecord.Operate(2);
			tradingRecord.Operate(3);
			Assert.AreEqual(24.3473, transactionCost.Calculate(series, tradingRecord), TaTestsUtils.TaOffset);

			tradingRecord.Operate(5);
			Assert.AreEqual(28.2204, transactionCost.Calculate(series, tradingRecord), TaTestsUtils.TaOffset);
		}

        [Test] 
		public void CalculateFixedCost()
		{
			var series = GenerateTimeSeries.From(100, 105, 110, 100, 95, 105);
			IAnalysisCriterion transactionCost = new LinearTransactionCostCriterion(1000, 0, 1.3d);

			var tradingRecord = new TradingRecord(Order.BuyAt(0), Order.SellAt(1));
			Assert.AreEqual(2.6d, transactionCost.Calculate(series, tradingRecord), TaTestsUtils.TaOffset);

			tradingRecord.Operate(2);
			tradingRecord.Operate(3);
			Assert.AreEqual(5.2d, transactionCost.Calculate(series, tradingRecord), TaTestsUtils.TaOffset);

			tradingRecord.Operate(0);
			Assert.AreEqual(6.5d, transactionCost.Calculate(series, tradingRecord), TaTestsUtils.TaOffset);
		}

        [Test] 
		public void CalculateFixedCostWithOneTrade()
		{
			var series = GenerateTimeSeries.From(100, 95, 100, 80, 85, 70);
			var trade = new Trade();
			IAnalysisCriterion transactionCost = new LinearTransactionCostCriterion(1000, 0, 0.75d);

			Assert.AreEqual(0, (int) transactionCost.Calculate(series, trade));

			trade.Operate(1);
			Assert.AreEqual(0.75d, transactionCost.Calculate(series, trade), TaTestsUtils.TaOffset);

			trade.Operate(3);
			Assert.AreEqual(1.5d, transactionCost.Calculate(series, trade), TaTestsUtils.TaOffset);

			trade.Operate(4);
			Assert.AreEqual(1.5d, transactionCost.Calculate(series, trade), TaTestsUtils.TaOffset);
		}

        [Test] 
		public void BetterThan()
		{
			IAnalysisCriterion criterion = new LinearTransactionCostCriterion(1000, 0.5);
			Assert.IsTrue(criterion.BetterThan(3.1, 4.2));
			Assert.IsFalse(criterion.BetterThan(2.1, 1.9));
		}
	}
}