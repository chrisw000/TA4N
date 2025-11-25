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
using TA4N.Trading.Rules;

namespace TA4N.Test.Trading.Rules
{
    public class WaitForRuleTest
    {
        private TradingRecord _tradingRecord;
        private WaitForRule _rule;

        [SetUp]
        public virtual void SetUp()
        {
            _tradingRecord = new TradingRecord();
        }

        [Test]
        public virtual void WaitForSinceLastBuyRuleIsSatisfied()
        {
            // Waits for 3 ticks since last buy order
            _rule = new WaitForRule(OrderType.Buy, 3);
            Assert.That(_rule.IsSatisfied(0, null), Is.False);
            Assert.That(_rule.IsSatisfied(1, _tradingRecord), Is.False);
            _tradingRecord.Enter(10);
            Assert.That(_rule.IsSatisfied(10, _tradingRecord), Is.False);
            Assert.That(_rule.IsSatisfied(11, _tradingRecord), Is.False);
            Assert.That(_rule.IsSatisfied(12, _tradingRecord), Is.False);
            Assert.That(_rule.IsSatisfied(13, _tradingRecord), Is.True);
            Assert.That(_rule.IsSatisfied(14, _tradingRecord), Is.True);
            _tradingRecord.Exit(15);
            Assert.That(_rule.IsSatisfied(15, _tradingRecord), Is.True);
            Assert.That(_rule.IsSatisfied(16, _tradingRecord), Is.True);
            _tradingRecord.Enter(17);
            Assert.That(_rule.IsSatisfied(17, _tradingRecord), Is.False);
            Assert.That(_rule.IsSatisfied(18, _tradingRecord), Is.False);
            Assert.That(_rule.IsSatisfied(19, _tradingRecord), Is.False);
            Assert.That(_rule.IsSatisfied(20, _tradingRecord), Is.True);
        }

        [Test]
        public virtual void WaitForSinceLastSellRuleIsSatisfied()
        {
            // Waits for 2 ticks since last sell order
            _rule = new WaitForRule(OrderType.Sell, 2);
            Assert.That(_rule.IsSatisfied(0, null), Is.False);
            Assert.That(_rule.IsSatisfied(1, _tradingRecord), Is.False);
            _tradingRecord.Enter(10);
            Assert.That(_rule.IsSatisfied(10, _tradingRecord), Is.False);
            Assert.That(_rule.IsSatisfied(11, _tradingRecord), Is.False);
            Assert.That(_rule.IsSatisfied(12, _tradingRecord), Is.False);
            Assert.That(_rule.IsSatisfied(13, _tradingRecord), Is.False);
            _tradingRecord.Exit(15);
            Assert.That(_rule.IsSatisfied(15, _tradingRecord), Is.False);
            Assert.That(_rule.IsSatisfied(16, _tradingRecord), Is.False);
            Assert.That(_rule.IsSatisfied(17, _tradingRecord), Is.True);
            _tradingRecord.Enter(17);
            Assert.That(_rule.IsSatisfied(17, _tradingRecord), Is.True);
            Assert.That(_rule.IsSatisfied(18, _tradingRecord), Is.True);
            _tradingRecord.Exit(20);
            Assert.That(_rule.IsSatisfied(20, _tradingRecord), Is.False);
            Assert.That(_rule.IsSatisfied(21, _tradingRecord), Is.False);
            Assert.That(_rule.IsSatisfied(22, _tradingRecord), Is.True);
        }
    }
}