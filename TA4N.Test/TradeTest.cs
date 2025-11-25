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
using NUnit.Framework;

namespace TA4N.Test
{
    public sealed class TradeTest
    {
        private Trade _newTrade, _uncoveredTrade, _trEquals1, _trEquals2, _trNotEquals1, _trNotEquals2;

        [SetUp]
        public void SetUp()
        {
            _newTrade = new Trade();
            _uncoveredTrade = new Trade(OrderType.Sell);
            _trEquals1 = new Trade();
            _trEquals1.Operate(1);
            _trEquals1.Operate(2);
            _trEquals2 = new Trade();
            _trEquals2.Operate(1);
            _trEquals2.Operate(2);
            _trNotEquals1 = new Trade(OrderType.Sell);
            _trNotEquals1.Operate(1);
            _trNotEquals1.Operate(2);
            _trNotEquals2 = new Trade(OrderType.Sell);
            _trNotEquals2.Operate(1);
            _trNotEquals2.Operate(2);
        }

        [Test]
        public void WhenNewShouldCreateBuyOrderWhenEntering()
        {
            _newTrade.Operate(0);
            Assert.That(_newTrade.Entry, Is.EqualTo(Order.BuyAt(0)));
        }

        [Test]
        public void WhenNewShouldNotExit()
        {
            Assert.That(_newTrade.Opened, Is.False);
        }

        [Test]
        public void WhenOpenedShouldCreateSellOrderWhenExiting()
        {
            _newTrade.Operate(0);
            _newTrade.Operate(1);
            Assert.That(_newTrade.Exit, Is.EqualTo(Order.SellAt(1)));
        }

        [Test]
        public void WhenClosedShouldNotEnter()
        {
            _newTrade.Operate(0);
            _newTrade.Operate(1);
            Assert.That(_newTrade.Closed, Is.True);
            _newTrade.Operate(2);
            Assert.That(_newTrade.Closed, Is.True);
        }

        [Test]
        public void WhenExitIndexIsLessThanEntryIndexShouldThrowException()
        {
            _newTrade.Operate(3);
            Assert.Throws<InvalidOperationException>(() => _newTrade.Operate(1));
        }

        [Test]
        public void ShouldCloseTradeOnSameIndex()
        {
            _newTrade.Operate(3);
            _newTrade.Operate(3);
            Assert.That(_newTrade.Closed, Is.True);
        }

        [Test]
        public void ShouldThrowArgumentExceptionWhenOrdersHaveSameType()
        {
            Assert.Throws<ArgumentException>(() => new Trade(Order.BuyAt(0), Order.BuyAt(1)));
        }

        [Test]
        public void WhenNewShouldCreateSellOrderWhenEnteringUncovered()
        {
            _uncoveredTrade.Operate(0);
            Assert.That(_uncoveredTrade.Entry, Is.EqualTo(Order.SellAt(0)));
        }

        [Test]
        public void WhenOpenedShouldCreateBuyOrderWhenExitingUncovered()
        {
            _uncoveredTrade.Operate(0);
            _uncoveredTrade.Operate(1);
            Assert.That(_uncoveredTrade.Exit, Is.EqualTo(Order.BuyAt(1)));
        }

        [Test]
        public void OverrideToString()
        {
            Assert.That(_trEquals2.ToString(), Is.EqualTo(_trEquals1.ToString()));
            Assert.That(_trNotEquals1.ToString(), Is.Not.EqualTo(_trEquals1.ToString()));
            Assert.That(_trNotEquals2.ToString(), Is.Not.EqualTo(_trEquals1.ToString()));
        }
    }
}