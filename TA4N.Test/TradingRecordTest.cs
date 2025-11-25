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

namespace TA4N.Test
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
            Assert.That(_emptyRecord.CurrentTrade.New, Is.True);
            Assert.That(_openedRecord.CurrentTrade.Opened, Is.True);
            Assert.That(_closedRecord.CurrentTrade.New, Is.True);
        }

        [Test]
        public void Operate()
        {
            var record = new TradingRecord();
            record.Operate(1);
            Assert.That(record.CurrentTrade.Opened, Is.True);
            Assert.That(record.TradeCount, Is.EqualTo(0));
            Assert.That(record.LastTrade, Is.Null);
            Assert.That(record.LastOrder, Is.EqualTo(Order.BuyAt(1)));
            Assert.That(record.GetLastOrder(OrderType.Buy), Is.EqualTo(Order.BuyAt(1)));
            Assert.That(record.GetLastOrder(OrderType.Sell), Is.Null);
            Assert.That(record.LastEntry, Is.EqualTo(Order.BuyAt(1)));
            Assert.That(record.LastExit, Is.Null);
            record.Operate(3);
            Assert.That(record.CurrentTrade.New, Is.True);
            Assert.That(record.TradeCount, Is.EqualTo(1));
            Assert.That(record.LastTrade, Is.EqualTo(new Trade(Order.BuyAt(1), Order.SellAt(3))));
            Assert.That(record.LastOrder, Is.EqualTo(Order.SellAt(3)));
            Assert.That(record.GetLastOrder(OrderType.Buy), Is.EqualTo(Order.BuyAt(1)));
            Assert.That(record.GetLastOrder(OrderType.Sell), Is.EqualTo(Order.SellAt(3)));
            Assert.That(record.LastEntry, Is.EqualTo(Order.BuyAt(1)));
            Assert.That(record.LastExit, Is.EqualTo(Order.SellAt(3)));
            record.Operate(5);
            Assert.That(record.CurrentTrade.Opened, Is.True);
            Assert.That(record.TradeCount, Is.EqualTo(1));
            Assert.That(record.LastTrade, Is.EqualTo(new Trade(Order.BuyAt(1), Order.SellAt(3))));
            Assert.That(record.LastOrder, Is.EqualTo(Order.BuyAt(5)));
            Assert.That(record.GetLastOrder(OrderType.Buy), Is.EqualTo(Order.BuyAt(5)));
            Assert.That(record.GetLastOrder(OrderType.Sell), Is.EqualTo(Order.SellAt(3)));
            Assert.That(record.LastEntry, Is.EqualTo(Order.BuyAt(5)));
            Assert.That(record.LastExit, Is.EqualTo(Order.SellAt(3)));
        }

        [Test]
        public void IsClosed()
        {
            Assert.That(_emptyRecord.Closed, Is.True);
            Assert.That(_openedRecord.Closed, Is.False);
            Assert.That(_closedRecord.Closed, Is.True);
        }

        [Test]
        public void GetTradeCount()
        {
            Assert.That(_emptyRecord.TradeCount, Is.EqualTo(0));
            Assert.That(_openedRecord.TradeCount, Is.EqualTo(1));
            Assert.That(_closedRecord.TradeCount, Is.EqualTo(2));
        }

        [Test]
        public void GetLastTrade()
        {
            Assert.That(_emptyRecord.LastTrade, Is.Null);
            Assert.That(_openedRecord.LastTrade, Is.EqualTo(new Trade(Order.BuyAt(0), Order.SellAt(3))));
            Assert.That(_closedRecord.LastTrade, Is.EqualTo(new Trade(Order.BuyAt(7), Order.SellAt(8))));
        }

        [Test]
        public void GetLastOrder()
        {
            // Last order
            Assert.That(_emptyRecord.LastOrder, Is.Null);
            Assert.That(_openedRecord.LastOrder, Is.EqualTo(Order.BuyAt(7)));
            Assert.That(_closedRecord.LastOrder, Is.EqualTo(Order.SellAt(8)));
            // Last BUY order
            Assert.That(_emptyRecord.GetLastOrder(OrderType.Buy), Is.Null);
            Assert.That(_openedRecord.GetLastOrder(OrderType.Buy), Is.EqualTo(Order.BuyAt(7)));
            Assert.That(_closedRecord.GetLastOrder(OrderType.Buy), Is.EqualTo(Order.BuyAt(7)));
            // Last SELL order
            Assert.That(_emptyRecord.GetLastOrder(OrderType.Sell), Is.Null);
            Assert.That(_openedRecord.GetLastOrder(OrderType.Sell), Is.EqualTo(Order.SellAt(3)));
            Assert.That(_closedRecord.GetLastOrder(OrderType.Sell), Is.EqualTo(Order.SellAt(8)));
        }

        [Test]
        public void GetLastEntryExit()
        {
            // Last entry
            Assert.That(_emptyRecord.LastEntry, Is.Null);
            Assert.That(_openedRecord.LastEntry, Is.EqualTo(Order.BuyAt(7)));
            Assert.That(_closedRecord.LastEntry, Is.EqualTo(Order.BuyAt(7)));
            // Last exit
            Assert.That(_emptyRecord.LastExit, Is.Null);
            Assert.That(_openedRecord.LastExit, Is.EqualTo(Order.SellAt(3)));
            Assert.That(_closedRecord.LastExit, Is.EqualTo(Order.SellAt(8)));
        }
    }
}