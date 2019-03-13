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
	/// Bearish Harami pattern indicator.
	/// </summary>
	/// <see href="http://www.investopedia.com/terms/b/bearishharami.asp">Investopedia.com</see>
	public class BearishHaramiIndicator : CachedIndicator<bool>
	{
		private readonly TimeSeries _series;

		/// <summary>
		/// Constructor. </summary>
		/// <param name="series"> a time series </param>
		public BearishHaramiIndicator(TimeSeries series) : base(series)
		{
			_series = series;
		}

		protected override bool Calculate(int index)
		{
			if (index < 1)
			{
				// Harami is a 2-candle pattern
				return false;
			}
			var prevTick = _series.GetTick(index - 1);
			var currTick = _series.GetTick(index);
			if (prevTick.Bullish && currTick.Bearish)
			{
				var prevOpenPrice = prevTick.OpenPrice;
				var prevClosePrice = prevTick.ClosePrice;
				var currOpenPrice = currTick.OpenPrice;
				var currClosePrice = currTick.ClosePrice;
				return currOpenPrice.IsGreaterThan(prevOpenPrice) && currOpenPrice.IsLessThan(prevClosePrice) && currClosePrice.IsGreaterThan(prevOpenPrice) && currClosePrice.IsLessThan(prevClosePrice);
			}
			return false;
		}
	}
}