using System;
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
namespace TA4N.Examples.analysis
{
    using TA4N;
	using TA4N.Analysis.Criteria;
	using TA4N.Examples.Loaders;
	using Strategies;

	/// <summary>
	/// This class diplays analysis criterion values after running a trading strategy over a time series.
	/// </summary>
	public class StrategyAnalysis
	{
        // Getting the time series
        private TimeSeries _series = CsvTradesLoader.LoadBitstampSeries();

	    [Test]
	    public void RunMovingMomentum()
	    {
            var strategy = MovingMomentumStrategy.BuildStrategy(_series);
            PrintAllResults(strategy);
        }

        [Test]
	    public void RunRSI2()
	    {
            var strategy = MovingMomentumStrategy.BuildStrategy(_series);
            PrintAllResults(strategy);
	    }

	    [Test]
	    public void RunGlobalExtrema()
	    {
	        var strategy = GlobalExtremaStrategy.BuildStrategy(_series);
            PrintAllResults(strategy);
	    }

	    [Test]
	    public void RunCciCorrection()
	    {
	        var strategy = CCICorrectionStrategy.BuildStrategy(_series);
            PrintAllResults(strategy);
	    }

        public void PrintAllResults(Strategy strategy, bool showTrades = true)
        {
            OutputReport(strategy, OrderType.Buy, showTrades);
            Console.WriteLine("");
            OutputReport(strategy, OrderType.Sell, showTrades);
        }

        private void OutputReport(Strategy strategy, OrderType type, bool showTrades)
        {
            if (type.Equals(OrderType.Sell))
                Console.WriteLine("RUN STRATEGY SHORT:");
            else
                Console.WriteLine("RUN STRATEGY LONG:");

            // Running the strategy
            var tradingRecord = _series.Run(strategy, type, Decimal.Thousand);

            // Total profit
            var totalProfit = new TotalProfitCriterion();
			Console.WriteLine("Total profit: " + totalProfit.Calculate(_series, tradingRecord));
			// Number of ticks
			Console.WriteLine("Number of ticks: " + new NumberOfTicksCriterion().Calculate(_series, tradingRecord));
			// Average profit (per tick)
			Console.WriteLine("Average profit (per tick): " + new AverageProfitCriterion().Calculate(_series, tradingRecord));
			// Number of trades
			Console.WriteLine("Number of trades: " + new NumberOfTradesCriterion().Calculate(_series, tradingRecord));
			// Profitable trades ratio
			Console.WriteLine("Profitable trades ratio: " + new AverageProfitableTradesCriterion().Calculate(_series, tradingRecord));
			// Maximum drawdown
			Console.WriteLine("Maximum drawdown: " + new MaximumDrawdownCriterion().Calculate(_series, tradingRecord));
			// Reward-risk ratio
			Console.WriteLine("Reward-risk ratio: " + new RewardRiskRatioCriterion().Calculate(_series, tradingRecord));
			// Total transaction cost
			Console.WriteLine("Total transaction cost (from $1000): " + new LinearTransactionCostCriterion(1000, 0.005).Calculate(_series, tradingRecord));
			// Buy-and-hold
			Console.WriteLine("Buy-and-hold: " + new BuyAndHoldCriterion().Calculate(_series, tradingRecord));
			// Total profit vs buy-and-hold
			Console.WriteLine("Custom strategy profit vs buy-and-hold strategy profit: " + new VersusBuyAndHoldCriterion(totalProfit).Calculate(_series, tradingRecord));
		}
	}
}
