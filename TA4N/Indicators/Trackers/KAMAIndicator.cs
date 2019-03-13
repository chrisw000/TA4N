using System;

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
namespace TA4N.Indicators.Trackers
{
    /// <summary>
	/// The Kaufman's Adaptive Moving Average (KAMA)  Indicator.
	/// </summary>
	/// <see href="http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:kaufman_s_adaptive_moving_average">StockCharts.com</see>
	public class KamaIndicator : CachedIndicator<Decimal>
	{
		private readonly IIndicator<Decimal> _price;
		private readonly int _timeFrameEffectiveRatio;
		private readonly Decimal _fastest;
		private readonly Decimal _slowest;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="price"> the price </param>
		/// <param name="timeFrameEffectiveRatio"> the time frame of the effective ratio (usually 10) </param>
		/// <param name="timeFrameFast"> the time frame fast (usually 2) </param>
		/// <param name="timeFrameSlow"> the time frame slow (usually 30) </param>
		public KamaIndicator(IIndicator<Decimal> price, int timeFrameEffectiveRatio, int timeFrameFast, int timeFrameSlow) : base(price)
		{
			_price = price;
			_timeFrameEffectiveRatio = timeFrameEffectiveRatio;
			_fastest = Decimal.Two.DividedBy(Decimal.ValueOf(timeFrameFast + 1));
			_slowest = Decimal.Two.DividedBy(Decimal.ValueOf(timeFrameSlow + 1));
		}

		protected override Decimal Calculate(int index)
		{
			var currentPrice = _price.GetValue(index);
			if (index < _timeFrameEffectiveRatio)
			{
				return currentPrice;
			}
			/*
			 * Efficiency Ratio (ER)
			 * ER = Change/Volatility
			 * Change = ABS(Close - Close (10 periods ago))
			 * Volatility = Sum10(ABS(Close - Prior Close))
			 * Volatility is the sum of the absolute value of the last ten price changes (Close - Prior Close).
			 */
			var startChangeIndex = Math.Max(0, index - _timeFrameEffectiveRatio);
			var change = currentPrice.Minus(_price.GetValue(startChangeIndex)).Abs();
			var volatility = Decimal.Zero;
			for (var i = startChangeIndex; i < index; i++)
			{
				volatility = volatility.Plus(_price.GetValue(i + 1).Minus(_price.GetValue(i)).Abs());
			}
			var er = change.DividedBy(volatility);
			/*
			 * Smoothing Constant (SC)
			 * SC = [ER x (fastest SC - slowest SC) + slowest SC]2
			 * SC = [ER x (2/(2+1) - 2/(30+1)) + 2/(30+1)]2
			 */
			var sc = er.MultipliedBy(_fastest.Minus(_slowest)).Plus(_slowest).Pow(2);
			/*
			 * KAMA
			 * Current KAMA = Prior KAMA + SC x (Price - Prior KAMA)
			 */
			var priorKama = GetValue(index - 1);
			return priorKama.Plus(sc.MultipliedBy(currentPrice.Minus(priorKama)));
		}
	}
}