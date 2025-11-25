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
namespace TA4N.Examples.Strategies
{

	using Decimal = TA4N.Decimal;
	using Strategy = TA4N.Strategy;
	using TimeSeries = TA4N.TimeSeries;
	using TradingRecord = TA4N.TradingRecord;
	using TotalProfitCriterion = TA4N.Analysis.Criteria.TotalProfitCriterion;
	using HighestValueIndicator = TA4N.Indicators.Helpers.HighestValueIndicator;
	using LowestValueIndicator = TA4N.Indicators.Helpers.LowestValueIndicator;
	using ClosePriceIndicator = TA4N.Indicators.Simple.ClosePriceIndicator;
	using MaxPriceIndicator = TA4N.Indicators.Simple.MaxPriceIndicator;
	using MinPriceIndicator = TA4N.Indicators.Simple.MinPriceIndicator;
	using MultiplierIndicator = TA4N.Indicators.Simple.MultiplierIndicator;
	using OverIndicatorRule = TA4N.Trading.Rules.OverIndicatorRule;
	using UnderIndicatorRule = TA4N.Trading.Rules.UnderIndicatorRule;
	using CsvTradesLoader = TA4N.Examples.Loaders.CsvTradesLoader;

	/// <summary>
	/// Strategies which compares current price to global extrema over a week.
	/// </summary>
	public class GlobalExtremaStrategy
	{
		// We assume that there were at least one trade every 5 minutes during the whole week
		private const int NB_TICKS_PER_WEEK = 12 * 24 * 7;

		/// <param name="series"> a time series </param>
		/// <returns> a global extrema strategy </returns>
		public static Strategy BuildStrategy(TimeSeries series)
		{
			if (series == null)
			{
				throw new ArgumentException("Series cannot be null");
			}

			var closePrices = new ClosePriceIndicator(series);

			// Getting the max price over the past week
			var maxPrices = new MaxPriceIndicator(series);
			var weekMaxPrice = new HighestValueIndicator(maxPrices, NB_TICKS_PER_WEEK);
			// Getting the min price over the past week
			var minPrices = new MinPriceIndicator(series);
			var weekMinPrice = new LowestValueIndicator(minPrices, NB_TICKS_PER_WEEK);

			// Going long if the close price goes below the min price
			var downWeek = new MultiplierIndicator(weekMinPrice, Decimal.ValueOf("1.004"));
			var buyingRule = new UnderIndicatorRule(closePrices, downWeek);

			// Going short if the close price goes above the max price
			var upWeek = new MultiplierIndicator(weekMaxPrice, Decimal.ValueOf("0.996"));
			var sellingRule = new OverIndicatorRule(closePrices, upWeek);

			return new Strategy(buyingRule, sellingRule);
		}

        [Test]
		public static void Main()
		{
			// Getting the time series
			var series = CsvTradesLoader.LoadBitstampSeries();

			// Building the trading strategy
			var strategy = BuildStrategy(series);

			// Running the strategy
			var tradingRecord = series.Run(strategy);
			Console.WriteLine("Number of trades for the strategy: " + tradingRecord.TradeCount);

			// Analysis
			Console.WriteLine("Total profit for the strategy: " + new TotalProfitCriterion().Calculate(series, tradingRecord));
		}
	}

}