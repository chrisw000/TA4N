using System.Collections.Generic;
using NUnit.Framework;
using TA4N.Analysis.Criteria;
using TA4N.Test.FixtureData;
using TA4N.Trading.Rules;

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


namespace TA4N.Test.Analysis.Criteria
{
    public sealed class AbstractAnalysisCriterionTest
    {
        private Strategy _alwaysStrategy;
        private Strategy _buyAndHoldStrategy;
        private IList<Strategy> _strategies;


        [SetUp]
        public void SetUp()
        {
            _alwaysStrategy = new Strategy(BooleanRule.True, BooleanRule.True);
            _buyAndHoldStrategy = new Strategy(new FixedRule(0), new FixedRule(4));
            _strategies = new List<Strategy>
            {
                _alwaysStrategy,
                _buyAndHoldStrategy
            };
        }

        [Test]
        public void BestShouldBeAlwaysOperateOnProfit()
        {
            var series = GenerateTimeSeries.From(6.0, 9.0, 6.0, 6.0);
            var bestStrategy = (new TotalProfitCriterion()).ChooseBest(series, _strategies);
            Assert.That(bestStrategy, Is.EqualTo(_alwaysStrategy));
        }

        [Test]
        public void BestShouldBeBuyAndHoldOnLoss()
        {
            var series = GenerateTimeSeries.From(6.0, 3.0, 6.0, 6.0);
            var bestStrategy = (new TotalProfitCriterion()).ChooseBest(series, _strategies);
            Assert.That(bestStrategy, Is.EqualTo(_buyAndHoldStrategy));
        }

        [Test]
        public void ToStringMethod()
        {
            AbstractAnalysisCriterion c1 = new AverageProfitCriterion();
            Assert.That(c1.ToString(), Is.EqualTo("Average Profit"));
            AbstractAnalysisCriterion c2 = new BuyAndHoldCriterion();
            Assert.That(c2.ToString(), Is.EqualTo("Buy And Hold"));
            AbstractAnalysisCriterion c3 = new RewardRiskRatioCriterion();
            Assert.That(c3.ToString(), Is.EqualTo("Reward Risk Ratio"));
        }
    }
}