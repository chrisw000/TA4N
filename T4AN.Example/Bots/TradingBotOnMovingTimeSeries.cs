using System;
using System.Threading;

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
namespace TA4N.Examples.bots
{

	using Decimal = TA4N.Decimal;
	using Order = TA4N.Order;
	using Strategy = TA4N.Strategy;
	using Tick = TA4N.Tick;
	using TimeSeries = TA4N.TimeSeries;
	using TradingRecord = TA4N.TradingRecord;
	using ClosePriceIndicator = TA4N.Indicators.Simple.ClosePriceIndicator;
	using SMAIndicator = TA4N.Indicators.Trackers.SMAIndicator;
	using OverIndicatorRule = TA4N.Trading.Rules.OverIndicatorRule;
	using UnderIndicatorRule = TA4N.Trading.Rules.UnderIndicatorRule;
	using DateTime = org.joda.time.DateTime;
	using CsvTradesLoader = TA4N.Examples.loaders.CsvTradesLoader;

	/// <summary>
	/// This class is an example of a dummy trading bot using ta4j.
	/// <para>
	/// </para>
	/// </summary>
	public class TradingBotOnMovingTimeSeries
	{

		/// <summary>
		/// Close price of the last tick </summary>
		private static Decimal LAST_TICK_CLOSE_PRICE;

		/// <summary>
		/// Builds a moving time series (i.e. keeping only the maxTickCount last ticks) </summary>
		/// <param name="maxTickCount"> the number of ticks to keep in the time series (at maximum) </param>
		/// <returns> a moving time series </returns>
		private static TimeSeries initMovingTimeSeries(int maxTickCount)
		{
			TimeSeries series = CsvTradesLoader.loadBitstampSeries();
			Console.Write("Initial tick count: " + series.TickCount);
			// Limitating the number of ticks to maxTickCount
			series.MaximumTickCount = maxTickCount;
			LAST_TICK_CLOSE_PRICE = series.getTick(series.End).ClosePrice;
			Console.WriteLine(" (limited to " + maxTickCount + "), close price = " + LAST_TICK_CLOSE_PRICE);
			return series;
		}

		/// <param name="series"> a time series </param>
		/// <returns> a dummy strategy </returns>
		private static Strategy buildStrategy(TimeSeries series)
		{
			if (series == null)
			{
				throw new System.ArgumentException("Series cannot be null");
			}

			ClosePriceIndicator closePrice = new ClosePriceIndicator(series);
			SMAIndicator sma = new SMAIndicator(closePrice, 12);

			// Signals
			// Buy when SMA goes over close price
			// Sell when close price goes over SMA
			Strategy buySellSignals = new Strategy(new OverIndicatorRule(sma, closePrice), new UnderIndicatorRule(sma, closePrice)
		   );
			return buySellSignals;
		}

		/// <summary>
		/// Generates a random decimal number between min and max. </summary>
		/// <param name="min"> the minimum bound </param>
		/// <param name="max"> the maximum bound </param>
		/// <returns> a random decimal number between min and max </returns>
		private static Decimal randDecimal(Decimal min, Decimal max)
		{
			Decimal randomDecimal = null;
			if (min != null && max != null && min.isLessThan(max))
			{
				randomDecimal = max.minus(min).multipliedBy(Decimal.valueOf(GlobalRandom.NextDouble)).plus(min);
			}
			return randomDecimal;
		}

		/// <summary>
		/// Generates a random tick. </summary>
		/// <returns> a random tick </returns>
		private static Tick generateRandomTick()
		{
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final TA4N.Decimal maxRange = TA4N.Decimal.valueOf("0.03");
			Decimal maxRange = Decimal.valueOf("0.03"); // 3.0%
			Decimal openPrice = LAST_TICK_CLOSE_PRICE;
			Decimal minPrice = openPrice.minus(openPrice.multipliedBy(maxRange.multipliedBy(Decimal.valueOf(GlobalRandom.NextDouble))));
			Decimal maxPrice = openPrice.plus(openPrice.multipliedBy(maxRange.multipliedBy(Decimal.valueOf(GlobalRandom.NextDouble))));
			Decimal closePrice = randDecimal(minPrice, maxPrice);
			LAST_TICK_CLOSE_PRICE = closePrice;
			return new Tick(DateTime.now(), openPrice, maxPrice, minPrice, closePrice, Decimal.ONE);
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static void main(String[] args) throws InterruptedException
		public static void Main(string[] args)
		{

			Console.WriteLine("********************** Initialization **********************");
			// Getting the time series
			TimeSeries series = initMovingTimeSeries(20);

			// Building the trading strategy
			Strategy strategy = buildStrategy(series);

			// Initializing the trading history
			TradingRecord tradingRecord = new TradingRecord();
			Console.WriteLine("************************************************************");

			/// <summary>
			/// We run the strategy for the 50 next ticks.
			/// </summary>
			for (int i = 0; i < 50; i++)
			{

				// New tick
				Thread.Sleep(30); // I know...
				Tick newTick = generateRandomTick();
				Console.WriteLine("------------------------------------------------------\n" + "Tick " + i + " added, close price = " + newTick.ClosePrice.toDouble());
				series.addTick(newTick);

				int endIndex = series.End;
				if (strategy.shouldEnter(endIndex))
				{
					// Our strategy should enter
					Console.WriteLine("Strategy should ENTER on " + endIndex);
					bool entered = tradingRecord.enter(endIndex, newTick.ClosePrice, Decimal.TEN);
					if (entered)
					{
						Order entry = tradingRecord.LastEntry;
						Console.WriteLine("Entered on " + entry.Index + " (price=" + entry.Price.toDouble() + ", amount=" + entry.Amount.toDouble() + ")");
					}
				} else if (strategy.shouldExit(endIndex))
				{
					// Our strategy should exit
					Console.WriteLine("Strategy should EXIT on " + endIndex);
					bool exited = tradingRecord.exit(endIndex, newTick.ClosePrice, Decimal.TEN);
					if (exited)
					{
						Order exit = tradingRecord.LastExit;
						Console.WriteLine("Exited on " + exit.Index + " (price=" + exit.Price.toDouble() + ", amount=" + exit.Amount.toDouble() + ")");
					}
				}
			}
		}
	}

}