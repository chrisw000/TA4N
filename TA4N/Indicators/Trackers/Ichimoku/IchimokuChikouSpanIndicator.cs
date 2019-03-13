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
namespace TA4N.Indicators.Trackers.Ichimoku
{
    using ClosePriceIndicator = Simple.ClosePriceIndicator;

	/// <summary>
	/// Ichimoku clouds: Chikou Span indicator
	/// <para>
	/// </para>
	/// </summary>
	/// <see href="http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:Ichimoku_cloud">StockCharts.com</see>
	public class IchimokuChikouSpanIndicator : CachedIndicator<Decimal>
	{
		/// <summary>
		/// The close price </summary>
		private readonly ClosePriceIndicator _closePriceIndicator;

		/// <summary>
		/// The time delay </summary>
		private readonly int _timeDelay;

		/// <summary>
		/// Constructor. </summary>
		/// <param name="series"> the series </param>
		/// <param name="timeDelay"> the time delay (usually 26) </param>
		public IchimokuChikouSpanIndicator(TimeSeries series, int timeDelay=26) : base(series)
		{
			_closePriceIndicator = new ClosePriceIndicator(series);
			_timeDelay = timeDelay;
		}

		protected override Decimal Calculate(int index)
		{
			return _closePriceIndicator.GetValue(Math.Max(0, index - _timeDelay));
		}
	}
}