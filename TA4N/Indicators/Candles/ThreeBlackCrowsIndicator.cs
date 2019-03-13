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
using TA4N.Indicators.Trackers;

namespace TA4N.Indicators.Candles
{
    /// <summary>
	/// Three black crows indicator.
	/// </summary>
	/// <see href="http://www.investopedia.com/terms/t/three_black_crows.asp">Investopedia.com</see>
	public class ThreeBlackCrowsIndicator : CachedIndicator<bool>
	{
		private readonly TimeSeries _series;

		/// <summary>
		/// Lower shadow </summary>
		private readonly LowerShadowIndicator _lowerShadowInd;
		/// <summary>
		/// Average lower shadow </summary>
		private readonly SmaIndicator _averageLowerShadowInd;
		/// <summary>
		/// Factor used when checking if a candle has a very short lower shadow </summary>
		private readonly Decimal _factor;

		private int _whiteCandleIndex = -1;

		/// <summary>
		/// Constructor. </summary>
		/// <param name="series"> a time series </param>
		/// <param name="timeFrame"> the number of ticks used to calculate the average lower shadow </param>
		/// <param name="factor"> the factor used when checking if a candle has a very short lower shadow </param>
		public ThreeBlackCrowsIndicator(TimeSeries series, int timeFrame, Decimal factor) : base(series)
		{
			_series = series;
			_lowerShadowInd = new LowerShadowIndicator(series);
			_averageLowerShadowInd = new SmaIndicator(_lowerShadowInd, timeFrame);
			_factor = factor;
		}

		protected override bool Calculate(int index)
		{
			if (index < 3)
			{
				// We need 4 candles: 1 white, 3 black
				return false;
			}
			_whiteCandleIndex = index - 3;
			return _series.GetTick(_whiteCandleIndex).Bullish && IsBlackCrow(index - 2) && IsBlackCrow(index - 1) && IsBlackCrow(index);
		}

		/// <param name="index"> the tick/candle index </param>
		/// <returns> true if the tick/candle has a very short lower shadow, false otherwise </returns>
		private bool HasVeryShortLowerShadow(int index)
		{
			var currentLowerShadow = _lowerShadowInd.GetValue(index);
			// We use the white candle index to remove to bias of the previous crows
			var averageLowerShadow = _averageLowerShadowInd.GetValue(_whiteCandleIndex);

			return currentLowerShadow.IsLessThan(averageLowerShadow.MultipliedBy(_factor));
		}

		/// <param name="index"> the current tick/candle index </param>
		/// <returns> true if the current tick/candle is declining, false otherwise </returns>
		private bool IsDeclining(int index)
		{
			var prevTick = _series.GetTick(index - 1);
			var currTick = _series.GetTick(index);
			var prevOpenPrice = prevTick.OpenPrice;
			var prevClosePrice = prevTick.ClosePrice;
			var currOpenPrice = currTick.OpenPrice;
			var currClosePrice = currTick.ClosePrice;

			// Opens within the body of the previous candle
			return currOpenPrice.IsLessThan(prevOpenPrice) && currOpenPrice.IsGreaterThan(prevClosePrice) && currClosePrice.IsLessThan(prevClosePrice);
					// Closes below the previous close price
		}

		/// <param name="index"> the current tick/candle index </param>
		/// <returns> true if the current tick/candle is a black crow, false otherwise </returns>
		private bool IsBlackCrow(int index)
		{
			var prevTick = _series.GetTick(index - 1);
			var currTick = _series.GetTick(index);
			if (currTick.Bearish)
			{
				if (prevTick.Bullish)
				{
					// First crow case
					return HasVeryShortLowerShadow(index) && currTick.OpenPrice.IsLessThan(prevTick.MaxPrice);
				} else
				{
					return HasVeryShortLowerShadow(index) && IsDeclining(index);
				}
			}
			return false;
		}
	}
}