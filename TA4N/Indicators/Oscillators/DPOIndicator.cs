using TA4N.Indicators.Trackers;

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
namespace TA4N.Indicators.Oscillators
{
    using ClosePriceIndicator = Simple.ClosePriceIndicator;

    /// <summary>
	/// The Detrended Price Oscillator (DPO) indicator.
	/// <para>
	/// The Detrended Price Oscillator (DPO) is an indicator designed to remove trend
	/// from price and make it easier to identify cycles. DPO does not extend to the
	/// last date because it is based on a displaced moving average. However,
	/// alignment with the most recent is not an issue because DPO is not a momentum
	/// oscillator. Instead, DPO is used to identify cycles highs/lows and estimate
	/// cycle length.
	/// </para> </summary>
	/// <see href="http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:detrended_price_osci">StockCharts.com</see>
	public class DpoIndicator : CachedIndicator<Decimal>
	{
	    private readonly int _timeShift;
        private readonly IIndicator<Decimal> _price;
        private readonly SmaIndicator _sma;

		/// <summary>
		/// Constructor. </summary>
		/// <param name="series"> the series </param>
		/// <param name="timeFrame"> the time frame </param>
		public DpoIndicator(TimeSeries series, int timeFrame) : this(new ClosePriceIndicator(series), timeFrame)
		{
		}

		/// <summary>
		/// Constructor. </summary>
		/// <param name="price"> the price </param>
		/// <param name="timeFrame"> the time frame </param>
		internal DpoIndicator(IIndicator<Decimal> price, int timeFrame) : base(price)
		{
		    _timeShift = timeFrame / 2 + 1;
			_price = price;
			_sma = new SmaIndicator(price, timeFrame);
		}

		protected override Decimal Calculate(int index)
		{
			return _price.GetValue(index).Minus(_sma.GetValue(index - _timeShift));
		}
	}
}