using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CsvHelper;
using NUnit.Framework;
using NUnit.Framework.Internal;

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
namespace TA4N.Examples.Loaders
{
    using TA4N;
    using NodaTime;

	/// <summary>
	/// This class build a Ta4j time series from a CSV file containing trades.
	/// </summary>
    public class CsvTradesLoader
    {
        private static TimeSeries bitstampSeries;

        [Test]
        public static void Main()
        {
            var series = LoadBitstampSeries();

            Console.WriteLine("Series: " + series.Name + " (" + series.SeriesPeriodDescription + ")");
            Console.WriteLine("Number of ticks: " + series.TickCount);
            Console.WriteLine("First tick: \n" + "\tVolume: " + series.GetTick(0).Volume + "\n" + "\tNumber of trades: " + series.GetTick(0).Trades + "\n" + "\tClose price: " + series.GetTick(0).ClosePrice);
        }

        /// <returns> a time series from Bitstamp (bitcoin exchange) trades </returns>
		public static TimeSeries LoadBitstampSeries()
        {
            if (bitstampSeries != null) return bitstampSeries;

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = @"TA4N.Example.Resources.bitstamp_trades_from_20131125_usd.csv";

            List<dynamic> lines = null;

            using (var stream = assembly.GetManifestResourceStream(resourceName))
		    {
		        if (stream != null)
		            using (var reader = new StreamReader(stream))
					{
						// Reading all lines of the CSV file
						var config = new CsvHelper.Configuration.CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
						{
							HasHeaderRecord = true
						};
						using (var csvReader = new CsvReader(reader, config))
						{
							lines = csvReader.GetRecords<dynamic>().ToList();
						}
					}
		    }

            // NOTE: csvReader takes care of the header, so don't need to remove it like Java version

            IList<Tick> ticks = null;
			if (lines?.Count > 0)
			{
			    LocalDateTime beginTime;
			    LocalDateTime endTime;
                // Getting the first and last trades timestamps
                long ts1 = long.Parse(lines[0].timestamp);
			    long ts2 = long.Parse(lines[lines.Count - 1].timestamp);
			    if (ts1 > ts2)
			    {
                    beginTime = LocalDateTime.FromDateTime(Instant.FromUnixTimeSeconds(ts2).ToDateTimeUtc());
                    endTime = LocalDateTime.FromDateTime(Instant.FromUnixTimeSeconds(ts1).ToDateTimeUtc());
                    
                    // Since the CSV file has the most recent trades at the top of the file, we'll reverse the list to feed the List<Tick> correctly.
                    lines.Reverse();
                }
                else
			    {
                    beginTime = LocalDateTime.FromDateTime(Instant.FromUnixTimeSeconds(ts1).ToDateTimeUtc());
                    endTime = LocalDateTime.FromDateTime(Instant.FromUnixTimeSeconds(ts2).ToDateTimeUtc());
                }
                
				// Building the empty ticks (every 300 seconds, yeah welcome in Bitcoin world)
				ticks = BuildEmptyTicks(beginTime, endTime, 300);

				// Filling the ticks with trades
				foreach (var tradeLine in lines)
				{
                    var tradeTimestamp = LocalDateTime.FromDateTime(Instant.FromUnixTimeSeconds(long.Parse(tradeLine.timestamp)).ToDateTimeUtc());
      				foreach (var tick in ticks)
					{
						if (tick.InPeriod(tradeTimestamp))
						{
							var tradePrice = double.Parse(tradeLine.price);
							var tradeAmount = double.Parse(tradeLine.amount);
							tick.AddTrade(tradeAmount, tradePrice);
						    break;
						}
					}
				}
				// Removing still empty ticks
				RemoveEmptyTicks(ticks);
			}

            bitstampSeries = new TimeSeries("bitstamp_trades", ticks);

            return bitstampSeries;
        }

		/// <summary>
		/// Builds a list of empty ticks. </summary>
		/// <param name="beginTime"> the begin time of the whole period </param>
		/// <param name="endTime"> the end time of the whole period </param>
		/// <param name="duration"> the tick duration (in seconds) </param>
		/// <returns> the list of empty ticks </returns>
		private static IList<Tick> BuildEmptyTicks(LocalDateTime beginTime, LocalDateTime endTime, int duration)
		{
			IList<Tick> emptyTicks = [];

			var tickTimePeriod = Period.FromSeconds(duration);
			var tickEndTime = beginTime;
			do
			{
				tickEndTime = tickEndTime.Plus(tickTimePeriod);
				emptyTicks.Add(new Tick(tickTimePeriod, tickEndTime));
			} while (tickEndTime < endTime);

			return emptyTicks;
		}

		/// <summary>
		/// Removes all empty (i.e. with no trade) ticks of the list. </summary>
		/// <param name="ticks"> a list of ticks </param>
		private static void RemoveEmptyTicks(IList<Tick> ticks)
		{
			for (var i = ticks.Count - 1; i >= 0; i--)
			{
				if (ticks[i].Trades == 0)
				{
					ticks.RemoveAt(i);
				}
			}
		}
	}

}