using System.Text;

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
namespace TA4N.Examples.indicators
{

	using TimeSeries = TA4N.TimeSeries;
	using AverageTrueRangeIndicator = TA4N.indicators.helpers.AverageTrueRangeIndicator;
	using StandardDeviationIndicator = TA4N.indicators.statistics.StandardDeviationIndicator;
	using PPOIndicator = TA4N.indicators.oscillators.PPOIndicator;
	using ClosePriceIndicator = TA4N.indicators.simple.ClosePriceIndicator;
	using PriceVariationIndicator = TA4N.indicators.simple.PriceVariationIndicator;
	using TypicalPriceIndicator = TA4N.indicators.simple.TypicalPriceIndicator;
	using EMAIndicator = TA4N.indicators.trackers.EMAIndicator;
	using ROCIndicator = TA4N.indicators.trackers.ROCIndicator;
	using RSIIndicator = TA4N.indicators.trackers.RSIIndicator;
	using SMAIndicator = TA4N.indicators.trackers.SMAIndicator;
	using WilliamsRIndicator = TA4N.indicators.trackers.WilliamsRIndicator;
	using CsvTradesLoader = TA4N.Examples.loaders.CsvTradesLoader;

	/// <summary>
	/// This class builds a CSV file containing values from indicators.
	/// </summary>
	public class IndicatorsToCsv
	{

		public static void Main(string[] args)
		{

			/// <summary>
			/// Getting time series
			/// </summary>
			TimeSeries series = CsvTradesLoader.loadBitstampSeries();

			/// <summary>
			/// Creating indicators
			/// </summary>
			// Close price
			ClosePriceIndicator closePrice = new ClosePriceIndicator(series);
			// Typical price
			TypicalPriceIndicator typicalPrice = new TypicalPriceIndicator(series);
			// Price variation
			PriceVariationIndicator priceVariation = new PriceVariationIndicator(series);
			// Simple moving averages
			SMAIndicator shortSma = new SMAIndicator(closePrice, 8);
			SMAIndicator longSma = new SMAIndicator(closePrice, 20);
			// Exponential moving averages
			EMAIndicator shortEma = new EMAIndicator(closePrice, 8);
			EMAIndicator longEma = new EMAIndicator(closePrice, 20);
			// Percentage price oscillator
			PPOIndicator ppo = new PPOIndicator(closePrice, 12, 26);
			// Rate of change
			ROCIndicator roc = new ROCIndicator(closePrice, 100);
			// Relative strength index
			RSIIndicator rsi = new RSIIndicator(closePrice, 14);
			// Williams %R
			WilliamsRIndicator williamsR = new WilliamsRIndicator(series, 20);
			// Average true range
			AverageTrueRangeIndicator atr = new AverageTrueRangeIndicator(series, 20);
			// Standard deviation
			StandardDeviationIndicator sd = new StandardDeviationIndicator(closePrice, 14);

			/// <summary>
			/// Building header
			/// </summary>
			StringBuilder sb = new StringBuilder("timestamp,close,typical,variation,sma8,sma20,ema8,ema20,ppo,roc,rsi,williamsr,atr,sd\n");

			/// <summary>
			/// Adding indicators values
			/// </summary>
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int nbTicks = series.getTickCount();
			int nbTicks = series.TickCount;
			for (int i = 0; i < nbTicks; i++)
			{
				sb.Append(series.getTick(i).EndTime.Millis / 1000d).Append(',').Append(closePrice.getValue(i)).Append(',').Append(typicalPrice.getValue(i)).Append(',').Append(priceVariation.getValue(i)).Append(',').Append(shortSma.getValue(i)).Append(',').Append(longSma.getValue(i)).Append(',').Append(shortEma.getValue(i)).Append(',').Append(longEma.getValue(i)).Append(',').Append(ppo.getValue(i)).Append(',').Append(roc.getValue(i)).Append(',').Append(rsi.getValue(i)).Append(',').Append(williamsR.getValue(i)).Append(',').Append(atr.getValue(i)).Append(',').Append(sd.getValue(i)).Append('\n');
			}

			/// <summary>
			/// Writing CSV file
			/// </summary>
			System.IO.StreamWriter writer = null;
			try
			{
				writer = new System.IO.StreamWriter("indicators.csv");
				writer.Write(sb.ToString());
			} catch (IOException ioe)
			{
//JAVA TO C# CONVERTER WARNING: The .NET Type.FullName property will not always yield results identical to the Java Class.getName method:
				Logger.getLogger(typeof(IndicatorsToCsv).FullName).log(Level.SEVERE, "Unable to write CSV file", ioe);
			} finally
			{
				try
				{
					if (writer != null)
					{
						writer.Close();
					}
				} catch (IOException)
				{
				}
			}

		}
	}

}