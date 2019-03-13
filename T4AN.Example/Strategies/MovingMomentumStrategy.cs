﻿using System;
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
namespace TA4N.Examples.strategies
{
	using Decimal = TA4N.Decimal;
	using Rule = TA4N.IRule;
	using Strategy = TA4N.Strategy;
	using TimeSeries = TA4N.TimeSeries;
	using TradingRecord = TA4N.TradingRecord;
	using TotalProfitCriterion = TA4N.Analysis.Criteria.TotalProfitCriterion;
	using StochasticOscillatorKIndicator = TA4N.Indicators.Oscillators.StochasticOscillatorKIndicator;
	using ClosePriceIndicator = TA4N.Indicators.Simple.ClosePriceIndicator;
	using EMAIndicator = TA4N.Indicators.Trackers.EmaIndicator;
	using MACDIndicator = TA4N.Indicators.Trackers.MacdIndicator;
	using CrossedDownIndicatorRule = TA4N.Trading.Rules.CrossedDownIndicatorRule;
	using CrossedUpIndicatorRule = TA4N.Trading.Rules.CrossedUpIndicatorRule;
	using OverIndicatorRule = TA4N.Trading.Rules.OverIndicatorRule;
	using UnderIndicatorRule = TA4N.Trading.Rules.UnderIndicatorRule;
	using CsvTradesLoader = TA4N.Examples.loaders.CsvTradesLoader;

	/// <summary>
	/// Moving momentum strategy.
	/// <para>
	/// </para>
	/// </summary>
	/// <see href="http://stockcharts.com/help/doku.php?id=chart_school:trading_strategies:moving_momentum">Stock Charts</see>
	public static class MovingMomentumStrategy
	{
		/// <param name="series">a time series </param>
		/// <returns> a moving momentum strategy </returns>
		public static Strategy BuildStrategy(TimeSeries series)
		{
			if (series == null)
			{
				throw new System.ArgumentException("Series cannot be null");
			}

			var closePrice = new ClosePriceIndicator(series);

			// The bias is bullish when the shorter-moving average moves above the longer moving average.
			// The bias is bearish when the shorter-moving average moves below the longer moving average.
			var shortEma = new EMAIndicator(closePrice, 9);
			var longEma = new EMAIndicator(closePrice, 26);

			var stochasticOscillK = new StochasticOscillatorKIndicator(series, 14);

			var macd = new MACDIndicator(closePrice, 9, 26);
			var emaMacd = new EMAIndicator(macd, 18);

			// Entry rule
			var entryRule = (new OverIndicatorRule(shortEma, longEma)).And(new CrossedDownIndicatorRule(stochasticOscillK, Decimal.ValueOf(20))).And(new OverIndicatorRule(macd, emaMacd)); // Signal 2 -  Signal 1 -  Trend

			// Exit rule
			var exitRule = (new UnderIndicatorRule(shortEma, longEma)).And(new CrossedUpIndicatorRule(stochasticOscillK, Decimal.ValueOf(80))).And(new UnderIndicatorRule(macd, emaMacd)); // Signal 2 -  Signal 1 -  Trend

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
			Console.WriteLine("Total profit for the strategy: " + (new TotalProfitCriterion()).Calculate(series, tradingRecord));
		}
	}
}