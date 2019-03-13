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
namespace TA4N
{
	public sealed class TradingRecordTest
	{
        private TradingRecord _emptyRecord, _openedRecord, _closedRecord;

        [SetUp]
        public void SetUp()
		{
			_emptyRecord = new TradingRecord();
			_openedRecord = new TradingRecord(Order.BuyAt(0), Order.SellAt(3), Order.BuyAt(7));
			_closedRecord = new TradingRecord(Order.BuyAt(0), Order.SellAt(3), Order.BuyAt(7), Order.SellAt(8));
		}

        [Test]
		public void GetCurrentTrade()
		{
			Assert.IsTrue(_emptyRecord.CurrentTrade.New);
			Assert.IsTrue(_openedRecord.CurrentTrade.Opened);
			Assert.IsTrue(_closedRecord.CurrentTrade.New);
		}

        [Test]
        public void Operate()
		{
			var record = new TradingRecord();

			record.Operate(1);
			Assert.IsTrue(record.CurrentTrade.Opened);
			Assert.AreEqual(0, record.TradeCount);
			Assert.IsNull(record.LastTrade);
			Assert.AreEqual(Order.BuyAt(1), record.LastOrder);
			Assert.AreEqual(Order.BuyAt(1), record.GetLastOrder(OrderType.Buy));
			Assert.IsNull(record.GetLastOrder(OrderType.Sell));
			Assert.AreEqual(Order.BuyAt(1), record.LastEntry);
			Assert.IsNull(record.LastExit);

			record.Operate(3);
			Assert.IsTrue(record.CurrentTrade.New);
			Assert.AreEqual(1, record.TradeCount);
			Assert.AreEqual(new Trade(Order.BuyAt(1), Order.SellAt(3)), record.LastTrade);
			Assert.AreEqual(Order.SellAt(3), record.LastOrder);
			Assert.AreEqual(Order.BuyAt(1), record.GetLastOrder(OrderType.Buy));
			Assert.AreEqual(Order.SellAt(3), record.GetLastOrder(OrderType.Sell));
			Assert.AreEqual(Order.BuyAt(1), record.LastEntry);
			Assert.AreEqual(Order.SellAt(3), record.LastExit);

			record.Operate(5);
			Assert.IsTrue(record.CurrentTrade.Opened);
			Assert.AreEqual(1, record.TradeCount);
			Assert.AreEqual(new Trade(Order.BuyAt(1), Order.SellAt(3)), record.LastTrade);
			Assert.AreEqual(Order.BuyAt(5), record.LastOrder);
			Assert.AreEqual(Order.BuyAt(5), record.GetLastOrder(OrderType.Buy));
			Assert.AreEqual(Order.SellAt(3), record.GetLastOrder(OrderType.Sell));
			Assert.AreEqual(Order.BuyAt(5), record.LastEntry);
			Assert.AreEqual(Order.SellAt(3), record.LastExit);
		}

        [Test]
        public void IsClosed()
		{
			Assert.IsTrue(_emptyRecord.Closed);
			Assert.IsFalse(_openedRecord.Closed);
			Assert.IsTrue(_closedRecord.Closed);
		}

        [Test]
        public void GetTradeCount()
		{
			Assert.AreEqual(0, _emptyRecord.TradeCount);
			Assert.AreEqual(1, _openedRecord.TradeCount);
			Assert.AreEqual(2, _closedRecord.TradeCount);
		}

        [Test]
        public void GetLastTrade()
		{
			Assert.IsNull(_emptyRecord.LastTrade);
			Assert.AreEqual(new Trade(Order.BuyAt(0), Order.SellAt(3)), _openedRecord.LastTrade);
			Assert.AreEqual(new Trade(Order.BuyAt(7), Order.SellAt(8)), _closedRecord.LastTrade);
		}

        [Test]
        public void GetLastOrder()
		{
			// Last order
			Assert.IsNull(_emptyRecord.LastOrder);
			Assert.AreEqual(Order.BuyAt(7), _openedRecord.LastOrder);
			Assert.AreEqual(Order.SellAt(8), _closedRecord.LastOrder);
			// Last BUY order
			Assert.IsNull(_emptyRecord.GetLastOrder(OrderType.Buy));
			Assert.AreEqual(Order.BuyAt(7), _openedRecord.GetLastOrder(OrderType.Buy));
			Assert.AreEqual(Order.BuyAt(7), _closedRecord.GetLastOrder(OrderType.Buy));
			// Last SELL order
			Assert.IsNull(_emptyRecord.GetLastOrder(OrderType.Sell));
			Assert.AreEqual(Order.SellAt(3), _openedRecord.GetLastOrder(OrderType.Sell));
			Assert.AreEqual(Order.SellAt(8), _closedRecord.GetLastOrder(OrderType.Sell));
		}

        [Test]
        public void GetLastEntryExit()
		{
			// Last entry
			Assert.IsNull(_emptyRecord.LastEntry);
			Assert.AreEqual(Order.BuyAt(7), _openedRecord.LastEntry);
			Assert.AreEqual(Order.BuyAt(7), _closedRecord.LastEntry);
			// Last exit
			Assert.IsNull(_emptyRecord.LastExit);
			Assert.AreEqual(Order.SellAt(3), _openedRecord.LastExit);
			Assert.AreEqual(Order.SellAt(8), _closedRecord.LastExit);
		}
	}
}