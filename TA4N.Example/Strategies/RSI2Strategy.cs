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
	using TA4N;
	using TotalProfitCriterion = TA4N.Analysis.Criteria.TotalProfitCriterion;
	using ClosePriceIndicator = TA4N.Indicators.Simple.ClosePriceIndicator;
	using RSIIndicator = TA4N.Indicators.Trackers.RsiIndicator;
	using SMAIndicator = TA4N.Indicators.Trackers.SmaIndicator;
	using CrossedDownIndicatorRule = TA4N.Trading.Rules.CrossedDownIndicatorRule;
	using CrossedUpIndicatorRule = TA4N.Trading.Rules.CrossedUpIndicatorRule;
	using OverIndicatorRule = TA4N.Trading.Rules.OverIndicatorRule;
	using UnderIndicatorRule = TA4N.Trading.Rules.UnderIndicatorRule;
	using CsvTradesLoader = TA4N.Examples.Loaders.CsvTradesLoader;

	/// <summary>
	/// 2-Period RSI Strategy
	/// <para>
	/// </para>
	/// </summary>
	/// <seealso cref= http://stockcharts.com/school/doku.php?id=chart_school:trading_strategies:rsi2 </seealso>
	public class RSI2Strategy
	{
        /// <param name="series"> a time series </param>
		/// <returns> a 2-period RSI strategy </returns>
		public static Strategy BuildStrategy(TimeSeries series)
		{
			if (series == null)
			{
				throw new ArgumentException("Series cannot be null");
			}

			var closePrice = new ClosePriceIndicator(series);
			var shortSma = new SMAIndicator(closePrice, 5);
			var longSma = new SMAIndicator(closePrice, 200);

			// We use a 2-period RSI indicator to identify buying
			// or selling opportunities within the bigger trend.
			var rsi = new RSIIndicator(closePrice, 2);

			// Entry rule
			// The long-term trend is up when a security is above its 200-period SMA.
			var entryRule = new OverIndicatorRule(shortSma, longSma).And(new CrossedDownIndicatorRule(rsi, Decimal.ValueOf(5))).And(new OverIndicatorRule(shortSma, closePrice)); // Signal 2 -  Signal 1 -  Trend

			// Exit rule
			// The long-term trend is down when a security is below its 200-period SMA.
			var exitRule = new UnderIndicatorRule(shortSma, longSma).And(new CrossedUpIndicatorRule(rsi, Decimal.ValueOf(95))).And(new UnderIndicatorRule(shortSma, closePrice)); // Signal 2 -  Signal 1 -  Trend

			return new Strategy(entryRule, exitRule);
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