using System;
using System.Collections.Generic;

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
namespace TA4N.Examples.walkforward
{
	using AnalysisCriterion = TA4N.analysis.Criterion;
	using TotalProfitCriterion = TA4N.Analysis.Criteria.TotalProfitCriterion;
	using Period = org.joda.time.Period;
	using CsvTradesLoader = TA4N.Examples.loaders.CsvTradesLoader;
	using CCICorrectionStrategy = TA4N.Examples.strategies.CCICorrectionStrategy;
	using GlobalExtremaStrategy = TA4N.Examples.strategies.GlobalExtremaStrategy;
	using MovingMomentumStrategy = TA4N.Examples.strategies.MovingMomentumStrategy;
	using RSI2Strategy = TA4N.Examples.strategies.RSI2Strategy;

	/// <summary>
	/// Walk-forward optimization example.
	/// <para>
	/// </para>
	/// </summary>
	/// <seealso cref= http://en.wikipedia.org/wiki/Walk_forward_optimization </seealso>
	/// <seealso cref= http://www.futuresmag.com/2010/04/01/can-your-system-do-the-walk </seealso>
	public class WalkForward
	{
		/// <param name="series"> the time series </param>
		/// <returns> a map (key: strategy, value: name) of trading strategies </returns>
		public static IDictionary<Strategy, string> buildStrategiesMap(TimeSeries series)
		{
			Dictionary<Strategy, string> strategies = new Dictionary<Strategy, string>();
			strategies[CCICorrectionStrategy.buildStrategy(series)] = "CCI Correction";
			strategies[GlobalExtremaStrategy.buildStrategy(series)] = "Global Extrema";
			strategies[MovingMomentumStrategy.buildStrategy(series)] = "Moving Momentum";
			strategies[RSI2Strategy.buildStrategy(series)] = "RSI-2";
			return strategies;
		}

		public static void Main(string[] args)
		{
			// Splitting the series into slices
			TimeSeries series = CsvTradesLoader.loadBitstampSeries();
			IList<TimeSeries> subseries = series.split(Period.hours(6), Period.weeks(1));

			// Building the map of strategies
			IDictionary<Strategy, string> strategies = buildStrategiesMap(series);

			// The analysis criterion
			AnalysisCriterion profitCriterion = new TotalProfitCriterion();

			foreach (TimeSeries slice in subseries)
			{
				// For each sub-series...
				Console.WriteLine("Sub-series: " + slice.SeriesPeriodDescription);
				foreach (KeyValuePair<Strategy, string> entry in strategies.SetOfKeyValuePairs())
				{
					Strategy strategy = entry.Key;
					string name = entry.Value;
					// For each strategy...
					TradingRecord tradingRecord = slice.run(strategy);
					double profit = profitCriterion.calculate(slice, tradingRecord);
					Console.WriteLine("\tProfit for " + name + ": " + profit);
				}
				Strategy bestStrategy = profitCriterion.chooseBest(slice, new List<Strategy>(strategies.Keys));
				Console.WriteLine("\t\t--> Best strategy: " + strategies[bestStrategy] + "\n");
			}
		}
	}
}