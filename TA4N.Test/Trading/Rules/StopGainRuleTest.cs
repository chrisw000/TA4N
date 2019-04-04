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
using TA4N.Indicators.Simple;
using TA4N.Test.FixtureData;
using TA4N.Trading.Rules;

namespace TA4N.Test.Trading.Rules
{
    public class StopGainRuleTest
	{
		private TradingRecord _tradingRecord;
		private ClosePriceIndicator _closePrice;
		private StopGainRule _rule;

        [SetUp]
		public virtual void SetUp()
		{
			_tradingRecord = new TradingRecord();
			_closePrice = new ClosePriceIndicator(GenerateTimeSeries.From(100, 105, 110, 120, 150, 120, 160, 180));
		}

        [Test]
		public virtual void IsSatisfied()
		{
			Decimal tradedAmount = Decimal.One;

			// 30% stop-gain
			_rule = new StopGainRule(_closePrice, Decimal.ValueOf("30"));

			Assert.IsFalse(_rule.IsSatisfied(0, null));
			Assert.IsFalse(_rule.IsSatisfied(1, _tradingRecord));

			// Enter at 108
			_tradingRecord.Enter(2, Decimal.ValueOf("108"), tradedAmount);
			Assert.IsFalse(_rule.IsSatisfied(2, _tradingRecord));
			Assert.IsFalse(_rule.IsSatisfied(3, _tradingRecord));
			Assert.IsTrue(_rule.IsSatisfied(4, _tradingRecord));
			// Exit
			_tradingRecord.Exit(5);

			// Enter at 118
			_tradingRecord.Enter(5, Decimal.ValueOf("118"), tradedAmount);
			Assert.IsFalse(_rule.IsSatisfied(5, _tradingRecord));
			Assert.IsTrue(_rule.IsSatisfied(6, _tradingRecord));
			Assert.IsTrue(_rule.IsSatisfied(7, _tradingRecord));
		}
	}
}