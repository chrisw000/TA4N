﻿/*
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
	/// Three white soldiers indicator.
	/// </summary>
	/// <see href="http://www.investopedia.com/terms/t/three_white_soldiers.asp">Investopedia.com</see>
	public class ThreeWhiteSoldiersIndicator : CachedIndicator<bool>
	{
		private readonly TimeSeries _series;

		/// <summary>
		/// Upper shadow </summary>
		private readonly UpperShadowIndicator _upperShadowInd;
		/// <summary>
		/// Average upper shadow </summary>
		private readonly SmaIndicator _averageUpperShadowInd;
		/// <summary>
		/// Factor used when checking if a candle has a very short upper shadow </summary>
		private readonly Decimal _factor;

		private int _blackCandleIndex = -1;

		/// <summary>
		/// Constructor. </summary>
		/// <param name="series"> a time series </param>
		/// <param name="timeFrame"> the number of ticks used to calculate the average upper shadow </param>
		/// <param name="factor"> the factor used when checking if a candle has a very short upper shadow </param>
		public ThreeWhiteSoldiersIndicator(TimeSeries series, int timeFrame, Decimal factor) : base(series)
		{
			_series = series;
			_upperShadowInd = new UpperShadowIndicator(series);
			_averageUpperShadowInd = new SmaIndicator(_upperShadowInd, timeFrame);
			_factor = factor;
		}

		protected override bool Calculate(int index)
		{
			if (index < 3)
			{
				// We need 4 candles: 1 black, 3 white
				return false;
			}
			_blackCandleIndex = index - 3;
			return _series.GetTick(_blackCandleIndex).Bearish && IsWhiteSoldier(index - 2) && IsWhiteSoldier(index - 1) && IsWhiteSoldier(index);
		}

		/// <param name="index"> the tick/candle index </param>
		/// <returns> true if the tick/candle has a very short upper shadow, false otherwise </returns>
		private bool HasVeryShortUpperShadow(int index)
		{
			var currentUpperShadow = _upperShadowInd.GetValue(index);
			// We use the black candle index to remove to bias of the previous soldiers
			var averageUpperShadow = _averageUpperShadowInd.GetValue(_blackCandleIndex);

			return currentUpperShadow.IsLessThan(averageUpperShadow.MultipliedBy(_factor));
		}

		/// <param name="index"> the current tick/candle index </param>
		/// <returns> true if the current tick/candle is growing, false otherwise </returns>
		private bool IsGrowing(int index)
		{
			var prevTick = _series.GetTick(index - 1);
			var currTick = _series.GetTick(index);
			var prevOpenPrice = prevTick.OpenPrice;
			var prevClosePrice = prevTick.ClosePrice;
			var currOpenPrice = currTick.OpenPrice;
			var currClosePrice = currTick.ClosePrice;

			// Opens within the body of the previous candle
			return currOpenPrice.IsGreaterThan(prevOpenPrice) && currOpenPrice.IsLessThan(prevClosePrice) && currClosePrice.IsGreaterThan(prevClosePrice);
					// Closes above the previous close price
		}

		/// <param name="index"> the current tick/candle index </param>
		/// <returns> true if the current tick/candle is a white soldier, false otherwise </returns>
		private bool IsWhiteSoldier(int index)
		{
			var prevTick = _series.GetTick(index - 1);
			var currTick = _series.GetTick(index);
			if (currTick.Bullish)
			{
				if (prevTick.Bearish)
				{
					// First soldier case
					return HasVeryShortUpperShadow(index) && currTick.OpenPrice.IsGreaterThan(prevTick.MinPrice);
				} else
				{
					return HasVeryShortUpperShadow(index) && IsGrowing(index);
				}
			}
			return false;
		}
	}
}