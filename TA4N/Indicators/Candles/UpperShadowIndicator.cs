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
namespace TA4N.Indicators.Candles
{
    /// <summary>
	/// Upper shadow height indicator.
	/// <para>
	/// Provides the (absolute) difference between the max price and the highest price of the candle body.
	/// I.e.: max price - max(open price, close price)
	/// </para>
	/// </summary>
	/// <see href="http://stockcharts.com/school/doku.php?id=chart_school:chart_analysis:introduction_to_candlesticks#formation">StockCharts.com</see>
	public class UpperShadowIndicator : CachedIndicator<Decimal>
	{
        private readonly TimeSeries _series;

		/// <summary>
		/// Constructor. </summary>
		/// <param name="series"> a time series </param>
		public UpperShadowIndicator(TimeSeries series) : base(series)
		{
			_series = series;
		}

		protected override Decimal Calculate(int index)
		{
			var t = _series.GetTick(index);
			var openPrice = t.OpenPrice;
			var closePrice = t.ClosePrice;
			if (closePrice.IsGreaterThan(openPrice))
			{
				// Bullish
				return t.MaxPrice.Minus(closePrice);
			} else
			{
				// Bearish
				return t.MaxPrice.Minus(openPrice);
			}
		}
	}
}