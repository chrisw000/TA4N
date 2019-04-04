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
	public sealed class RewardRiskRatioCriterionTest
	{
		private RewardRiskRatioCriterion _rrc;

        [SetUp]
		public void SetUp()
		{
			_rrc = new RewardRiskRatioCriterion();
		}
        
        [Test]
		public void RewardRiskRatioCriterion()
		{
			var tradingRecord = new TradingRecord(Order.BuyAt(0), Order.SellAt(1), Order.BuyAt(2), Order.SellAt(4), Order.BuyAt(5), Order.SellAt(7));

			var series = GenerateTimeSeries.From(100, 105, 95, 100, 90, 95, 80, 120);

			var totalProfit = (105d / 100) * (90d / 95d) * (120d / 95);
			var peak = (105d / 100) * (100d / 95);
			var low = (105d / 100) * (90d / 95) * (80d / 95);

			Assert.AreEqual(totalProfit / ((peak - low) / peak), _rrc.Calculate(series, tradingRecord), TaTestsUtils.TaOffset);
		}

        [Test]
		public void RewardRiskRatioCriterionOnlyWithGain()
		{
			var series = GenerateTimeSeries.From(1, 2, 3, 6, 8, 20, 3);
			var tradingRecord = new TradingRecord(Order.BuyAt(0), Order.SellAt(1), Order.BuyAt(2), Order.SellAt(5));
			Assert.IsTrue(double.IsInfinity(_rrc.Calculate(series, tradingRecord)));
		}

        [Test] 
		public void RewardRiskRatioCriterionWithNoTrades()
		{
			var series = GenerateTimeSeries.From(1, 2, 3, 6, 8, 20, 3);
			Assert.IsTrue(double.IsInfinity(_rrc.Calculate(series, new TradingRecord())));
		}

        [Test]
		public void WithOneTrade()
		{
			var trade = new Trade(Order.BuyAt(0), Order.SellAt(1));

			var series = GenerateTimeSeries.From(100, 95, 95, 100, 90, 95, 80, 120);

			var ratioCriterion = new RewardRiskRatioCriterion();
			Assert.AreEqual((95d / 100) / ((1d - 0.95d)), TaTestsUtils.TaOffset, ratioCriterion.Calculate(series, trade));
		}

        [Test]
		public void BetterThan()
		{
			IAnalysisCriterion criterion = new RewardRiskRatioCriterion();
			Assert.IsTrue(criterion.BetterThan(3.5, 2.2));
			Assert.IsFalse(criterion.BetterThan(1.5, 2.7));
		}
	}
}