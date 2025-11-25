using System;
using TA4N.Indicators.Trackers;
using NUnit.Framework;

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

namespace TA4N.Examples
{
    using Strategy = TA4N.Strategy;
    using Decimal = TA4N.Decimal;
    using TimeSeries = TA4N.TimeSeries;
    using TradingRecord = TA4N.TradingRecord;
    using CashFlow = TA4N.Analysis.CashFlow;
    using AverageProfitableTradesCriterion = TA4N.Analysis.Criteria.AverageProfitableTradesCriterion;
    using RewardRiskRatioCriterion = TA4N.Analysis.Criteria.RewardRiskRatioCriterion;
    using TotalProfitCriterion = TA4N.Analysis.Criteria.TotalProfitCriterion;
    using VersusBuyAndHoldCriterion = TA4N.Analysis.Criteria.VersusBuyAndHoldCriterion;
    using ClosePriceIndicator = TA4N.Indicators.Simple.ClosePriceIndicator;
    using CrossedDownIndicatorRule = TA4N.Trading.Rules.CrossedDownIndicatorRule;
    using CrossedUpIndicatorRule = TA4N.Trading.Rules.CrossedUpIndicatorRule;
    using StopGainRule = TA4N.Trading.Rules.StopGainRule;
    using StopLossRule = TA4N.Trading.Rules.StopLossRule;
    using CsvTradesLoader = TA4N.Examples.Loaders.CsvTradesLoader;

    /// <summary>
    /// Quickstart for ta4j.
    /// <para>
    /// Global example.
    /// </para>
    /// </summary>
    public class Quickstart
    {
        [Test]
        public void QuickStart()
        {
            // Getting a time series (from any provider: CSV, web service, etc.)
            var series = CsvTradesLoader.LoadBitstampSeries();

            // Getting the close price of the ticks
            var firstClosePrice = series.GetTick(0).ClosePrice;
            Console.WriteLine("First close price: " + firstClosePrice.ToDouble());
            // Or within an indicator:
            var closePrice = new ClosePriceIndicator(series);
            // Here is the same close price:
            Console.WriteLine(firstClosePrice.IsEqual(closePrice.GetValue(0))); // equal to firstClosePrice

            // Getting the simple moving average (SMA) of the close price over the last 5 ticks
            var shortSma = new SmaIndicator(closePrice, 5);
            // Here is the 5-ticks-SMA value at the 42nd index
            Console.WriteLine("5-ticks-SMA value at the 42nd index: " + shortSma.GetValue(42).ToDouble());

            // Getting a longer SMA (e.g. over the 30 last ticks)
            var longSma = new SmaIndicator(closePrice, 30);


            // Ok, now let's building our trading rules!

            // Buying rules
            // We want to buy:
            //  - if the 5-ticks SMA crosses over 30-ticks SMA
            //  - or if the price goes below a defined price (e.g $800.00)
            var buyingRule = new CrossedUpIndicatorRule(shortSma, longSma)
                .Or(new CrossedDownIndicatorRule(closePrice, Decimal.ValueOf("800")));

            // Selling rules
            // We want to sell:
            //  - if the 5-ticks SMA crosses under 30-ticks SMA
            //  - or if if the price looses more than 3%
            //  - or if the price earns more than 2%
            var sellingRule = new CrossedDownIndicatorRule(shortSma, longSma)
                              //.Or(new CrossedDownIndicatorRule(new TrailingStopLossIndicator(closePrice, Decimal.ValueOf("30")), closePrice ))
                .Or(new StopLossRule(closePrice, Decimal.ValueOf("3")))
                .Or(new StopGainRule(closePrice, Decimal.ValueOf("2")));

            // Running our juicy trading strategy...
            var tradingRecord = series.Run(new Strategy(buyingRule, sellingRule));
            Console.WriteLine("Number of trades for our strategy: " + tradingRecord.TradeCount);

            // Analysis

            // Getting the cash flow of the resulting trades
            var cashFlow = new CashFlow(series, tradingRecord);

            // Getting the profitable trades ratio
            IAnalysisCriterion profitTradesRatio = new AverageProfitableTradesCriterion();
            Console.WriteLine("Profitable trades ratio: " + profitTradesRatio.Calculate(series, tradingRecord));
            // Getting the reward-risk ratio
            IAnalysisCriterion rewardRiskRatio = new RewardRiskRatioCriterion();
            Console.WriteLine("Reward-risk ratio: " + rewardRiskRatio.Calculate(series, tradingRecord));

            // Total profit of our strategy
            // vs total profit of a buy-and-hold strategy
            IAnalysisCriterion vsBuyAndHold = new VersusBuyAndHoldCriterion(new TotalProfitCriterion());
            Console.WriteLine("Our profit vs buy-and-hold profit: " + vsBuyAndHold.Calculate(series, tradingRecord));

            // Your turn!
        }
    }
}