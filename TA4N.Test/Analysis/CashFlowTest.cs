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

using System.Linq;
using TA4N.Mocks;
using NodaTime;
using NUnit.Framework;
using TA4N.Extend;

namespace TA4N.Analysis
{
    public sealed class CashFlowTest
	{
        [Test]
		public void CashFlowSize()
		{
			TimeSeries sampleTimeSeries = new MockTimeSeries(1d, 2d, 3d, 4d, 5d);
			var cashFlow = new CashFlow(sampleTimeSeries, new TradingRecord());
			Assert.AreEqual(5, cashFlow.Size);
		}

        [Test]
		public void CashFlowBuyWithOnlyOneTrade()
		{
			TimeSeries sampleTimeSeries = new MockTimeSeries(1d, 2d);
			var tradingRecord = new TradingRecord(Order.BuyAt(0), Order.SellAt(1));

			var cashFlow = new CashFlow(sampleTimeSeries, tradingRecord);

			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(0), 1);
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(1), 2);
		}

        [Test]
		public void CashFlowWithSellAndBuyOrders()
		{
			TimeSeries sampleTimeSeries = new MockTimeSeries(2, 1, 3, 5, 6, 3, 20);
			var tradingRecord = new TradingRecord(Order.BuyAt(0), Order.SellAt(1), Order.BuyAt(3), Order.SellAt(4), Order.SellAt(5), Order.BuyAt(6));

			var cashFlow = new CashFlow(sampleTimeSeries, tradingRecord);

			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(0), 1);
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(1), "0.5");
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(2), "0.5");
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(3), "0.5");
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(4), "0.6");
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(5), "0.6");
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(6), "0.09");
		}

        [Test]
		public void CashFlowSell()
		{
			TimeSeries sampleTimeSeries = new MockTimeSeries(1, 2, 4, 8, 16, 32);
			var tradingRecord = new TradingRecord(Order.SellAt(2), Order.BuyAt(3));

			var cashFlow = new CashFlow(sampleTimeSeries, tradingRecord);

			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(0), 1);
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(1), 1);
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(2), 1);
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(3), "0.5");
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(4), "0.5");
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(5), "0.5");
		}

        [Test]
		public void CashFlowShortSell()
		{
			TimeSeries sampleTimeSeries = new MockTimeSeries(1, 2, 4, 8, 16, 32);
			var tradingRecord = new TradingRecord(Order.BuyAt(0), Order.SellAt(2), Order.SellAt(2), Order.BuyAt(4), Order.BuyAt(4), Order.SellAt(5));

			var cashFlow = new CashFlow(sampleTimeSeries, tradingRecord);

			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(0), 1);
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(1), 2);
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(2), 4);
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(3), 2);
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(4), 1);
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(5), 2);
		}

        [Test]
		public void CashFlowValueWithOnlyOneTradeAndAGapBefore()
		{
			TimeSeries sampleTimeSeries = new MockTimeSeries(1d, 1d, 2d);
			var tradingRecord = new TradingRecord(Order.BuyAt(1), Order.SellAt(2));

			var cashFlow = new CashFlow(sampleTimeSeries, tradingRecord);

			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(0), 1);
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(1), 1);
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(2), 2);
		}
        
        [Test]
		public void CashFlowValueWithOnlyOneTradeAndAGapAfter()
		{
			TimeSeries sampleTimeSeries = new MockTimeSeries(1d, 2d, 2d);
			var tradingRecord = new TradingRecord(Order.BuyAt(0), Order.SellAt(1));

			var cashFlow = new CashFlow(sampleTimeSeries, tradingRecord);

			Assert.AreEqual(3, cashFlow.Size);
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(0), 1);
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(1), 2);
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(2), 2);
		}

        [Test]
		public void CashFlowValueWithTwoTradesAndLongTimeWithoutOrders()
		{
			TimeSeries sampleTimeSeries = new MockTimeSeries(1d, 2d, 4d, 8d, 16d, 32d);
			var tradingRecord = new TradingRecord(Order.BuyAt(1), Order.SellAt(2), Order.BuyAt(4), Order.SellAt(5));

			var cashFlow = new CashFlow(sampleTimeSeries, tradingRecord);

			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(0), 1);
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(1), 1);
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(2), 2);
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(3), 2);
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(4), 2);
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(5), 4);
		}

        [Test]
		public void CashFlowValue()
		{
			TimeSeries sampleTimeSeries = new MockTimeSeries(3d, 2d, 5d, 1000d, 5000d, 0.0001d, 4d, 7d, 6d, 7d, 8d, 5d, 6d);
			var tradingRecord = new TradingRecord(Order.BuyAt(0), Order.SellAt(2), Order.BuyAt(6), Order.SellAt(8), Order.BuyAt(9), Order.SellAt(11));

			var cashFlow = new CashFlow(sampleTimeSeries, tradingRecord);

			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(0), 1);
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(1), 2d / 3);
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(2), 5d / 3);
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(3), 5d / 3);
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(4), 5d / 3);
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(5), 5d / 3);
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(6), 5d / 3);
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(7), 5d / 3 * 7d / 4);
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(8), 5d / 3 * 6d / 4);
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(9), 5d / 3 * 6d / 4);
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(10), 5d / 3 * 6d / 4 * 8d / 7);
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(11), 5d / 3 * 6d / 4 * 5d / 7);
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(12), 5d / 3 * 6d / 4 * 5d / 7);
		}
        
        [Test]
		public void CashFlowValueWithNoTrades()
		{
			TimeSeries sampleTimeSeries = new MockTimeSeries(3d, 2d, 5d, 4d, 7d, 6d, 7d, 8d, 5d, 6d);
			var cashFlow = new CashFlow(sampleTimeSeries, new TradingRecord());
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(4), 1);
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(7), 1);
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(9), 1);
		}
        
        [Test]
		public void CashFlowWithConstrainedSeries()
		{
			var series = new MockTimeSeries(5d, 6d, 3d, 7d, 8d, 6d, 10d, 15d, 6d);
			var constrained = series.Subseries(4, 8);
			var tradingRecord = new TradingRecord(Order.BuyAt(4), Order.SellAt(5), Order.BuyAt(6), Order.SellAt(8));

			var flow = new CashFlow(constrained, tradingRecord);
			TaTestsUtils.AssertDecimalEquals(flow.GetValue(0), 1);
			TaTestsUtils.AssertDecimalEquals(flow.GetValue(1), 1);
			TaTestsUtils.AssertDecimalEquals(flow.GetValue(2), 1);
			TaTestsUtils.AssertDecimalEquals(flow.GetValue(3), 1);
			TaTestsUtils.AssertDecimalEquals(flow.GetValue(4), 1);
			TaTestsUtils.AssertDecimalEquals(flow.GetValue(5), "0.75");
			TaTestsUtils.AssertDecimalEquals(flow.GetValue(6), "0.75");
			TaTestsUtils.AssertDecimalEquals(flow.GetValue(7), "1.125");
			TaTestsUtils.AssertDecimalEquals(flow.GetValue(8), "0.45");
		}

        [Test]
		public void ReallyLongCashFlow()
		{
			const int size = 1000000;
			TimeSeries sampleTimeSeries = new MockTimeSeries(Enumerable.Repeat((Tick) new MockTick(10), size).ToList());
			var tradingRecord = new TradingRecord(Order.BuyAt(0), Order.SellAt(size - 1));
			var cashFlow = new CashFlow(sampleTimeSeries, tradingRecord);
			TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(size - 1), 1);
		}

	    [Test]
	    public void DynamicAddTrade()
	    {
            var data = new double[] { 2, 1, 3, 5, 6, 3, 20, 20, 3 };

            // We will rely on this being correct as it duplicates tests above...
            TimeSeries confirmedSeries = new MockTimeSeries(data);
            var confirmedTradingRecord = new TradingRecord(Order.BuyAt(0), Order.SellAt(2), Order.BuyAt(3), Order.SellAt(4), Order.BuyAt(5), Order.SellAt(6), Order.BuyAt(7), Order.SellAt(8));
            // use these results to check against the dynamically added Trades
            var confirmedCashFlow = new CashFlow(confirmedSeries, confirmedTradingRecord);
            
            TimeSeries sampleTimeSeries = new TimeSeries();
	        var tradingRecord = new TradingRecord();
            var cashFlow = new CashFlow(sampleTimeSeries, tradingRecord);
            
	        for (var i = 0; i < data.Length; i++)
	        {
	            sampleTimeSeries.AddTick(new MockTick((new LocalDateTime()).WithMillisOfSecond(i), data[i]));
                switch (i)
                {
                    case 0: // buy
                    case 3: // buy
                    case 5: // buy
                    case 7: // buy
                        tradingRecord.Enter(i);
                        break;

                    case 2: // sell
                    case 4: // sell
                    case 6: // sell
                    case 8: // sell
                        tradingRecord.Exit(i);
                        cashFlow.AddTrade(tradingRecord.LastTrade);
                        break;

                    default:
                        // don't do anything
                        break;
                }
            }

            // Check all the data off...
            Assert.Multiple(() =>
            {
                for (var i = 0; i < data.Length; i++)
	            {
	                TaTestsUtils.AssertDecimalEquals(cashFlow.GetValue(i), confirmedCashFlow.GetValue(i).ToDouble());
	            }
	        });
	    }
    }
}