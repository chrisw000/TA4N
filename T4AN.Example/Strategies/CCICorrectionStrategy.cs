using System;
using System.Data;
using TA4N.Indicators.Oscillators;
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
	using CciIndicator = TA4N.Indicators.Oscillators.CciIndicator;
	using OverIndicatorRule = TA4N.Trading.Rules.OverIndicatorRule;
	using UnderIndicatorRule = TA4N.Trading.Rules.UnderIndicatorRule;
	using CsvTradesLoader = TA4N.Examples.Loaders.CsvTradesLoader;

	/// <summary>
	/// CCI Correction Strategy
	/// <para>
	/// </para>
	/// </summary>
	/// <seealso cref= http://stockcharts.com/school/doku.php?id=chart_school:trading_strategies:cci_correction </seealso>
	public class CCICorrectionStrategy
	{
        /// <param name="series"> a time series </param>
		/// <returns> a CCI correction strategy </returns>
		public static Strategy BuildStrategy(TimeSeries series)
		{
			if (series == null)
			{
				throw new ArgumentException("Series cannot be null");
			}

			var longCci = new CciIndicator(series, 200);
			var shortCci = new CciIndicator(series, 5);
			var plus100 = Decimal.Hundred;
			var minus100 = Decimal.ValueOf(-100);

			var entryRule = (new OverIndicatorRule(longCci, plus100)).And(new UnderIndicatorRule(shortCci, minus100)); // Signal -  Bull trend

			var exitRule = (new UnderIndicatorRule(longCci, minus100)).And(new OverIndicatorRule(shortCci, plus100)); // Signal -  Bear trend

		    var strategy = new Strategy(entryRule, exitRule)
		    {
		        UnstablePeriod = 5
		    };
		    return strategy;
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