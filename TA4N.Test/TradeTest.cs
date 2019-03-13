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
namespace TA4N
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
			Assert.AreEqual(Order.BuyAt(0), _newTrade.Entry);
		}

        [Test]
		public void WhenNewShouldNotExit()
		{
			Assert.IsFalse(_newTrade.Opened);
		}

        [Test]
		public void WhenOpenedShouldCreateSellOrderWhenExiting()
		{
			_newTrade.Operate(0);
			_newTrade.Operate(1);
			Assert.AreEqual(Order.SellAt(1), _newTrade.Exit);
		}

        [Test]
        public void WhenClosedShouldNotEnter()
		{
			_newTrade.Operate(0);
			_newTrade.Operate(1);
			Assert.IsTrue(_newTrade.Closed);
			_newTrade.Operate(2);
			Assert.IsTrue(_newTrade.Closed);
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
			Assert.IsTrue(_newTrade.Closed);
		}

        [Test]
        public void ShouldThrowArgumentNullExceptionWhenOrderTypeIsNull()
		{
            Assert.Throws<ArgumentNullException>(() => new Trade(null));
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
			Assert.AreEqual(Order.SellAt(0), _uncoveredTrade.Entry);
		}

        [Test]
		public void WhenOpenedShouldCreateBuyOrderWhenExitingUncovered()
		{
			_uncoveredTrade.Operate(0);
			_uncoveredTrade.Operate(1);
			Assert.AreEqual(Order.BuyAt(1), _uncoveredTrade.Exit);
		}

        [Test]
		public void OverrideToString()
		{
			Assert.AreEqual(_trEquals1.ToString(), _trEquals2.ToString());
			Assert.AreNotEqual(_trEquals1.ToString(), _trNotEquals1.ToString());
			Assert.AreNotEqual(_trEquals1.ToString(), _trNotEquals2.ToString());
		}
	}
}