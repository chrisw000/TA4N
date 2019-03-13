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
namespace TA4N.Indicators.Volume
{
	using TypicalPriceIndicator = Simple.TypicalPriceIndicator;
	using VolumeIndicator = Simple.VolumeIndicator;

    /// <summary>
    /// The volume-weighted average price (VWAP) Indicator. </summary>
    /// <see href="http://www.investopedia.com/articles/trading/11/trading-with-vwap-mvwap.asp">Investopedia.com</see>
    /// <see href="http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:vwap_intraday">StockCharts.com</see>
    /// <see href="https://en.wikipedia.org/wiki/Volume-weighted_average_price">StockCharts.com</see>
    public class VwapIndicator : CachedIndicator<Decimal>
	{
		private readonly int _timeFrame;
        private readonly IIndicator<Decimal> _typicalPrice;
        private readonly IIndicator<Decimal> _volume;

		/// <summary>
		/// Constructor. </summary>
		/// <param name="series"> the series </param>
		/// <param name="timeFrame"> the time frame </param>
		public VwapIndicator(TimeSeries series, int timeFrame) : base(series)
		{
			_timeFrame = timeFrame;
			_typicalPrice = new TypicalPriceIndicator(series);
			_volume = new VolumeIndicator(series);
		}

		protected override Decimal Calculate(int index)
		{
			if (index <= 0)
			{
				return _typicalPrice.GetValue(index);
			}
			var startIndex = Math.Max(0, index - _timeFrame + 1);
			var cumulativeTpv = Decimal.Zero;
			var cumulativeVolume = Decimal.Zero;
			for (var i = startIndex; i <= index; i++)
			{
				var currentVolume = _volume.GetValue(i);
				cumulativeTpv = cumulativeTpv.Plus(_typicalPrice.GetValue(i).MultipliedBy(currentVolume));
				cumulativeVolume = cumulativeVolume.Plus(currentVolume);
			}
			return cumulativeTpv.DividedBy(cumulativeVolume);
		}
	}
}