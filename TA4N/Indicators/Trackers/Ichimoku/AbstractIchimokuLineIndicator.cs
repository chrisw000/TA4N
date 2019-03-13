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
	using HighestValueIndicator = Helpers.HighestValueIndicator;
	using LowestValueIndicator = Helpers.LowestValueIndicator;
	using MaxPriceIndicator = Simple.MaxPriceIndicator;
	using MinPriceIndicator = Simple.MinPriceIndicator;

	/// <summary>
	/// An abstract class for Ichimoku clouds Indicators.
	/// <para>
	/// </para>
	/// </summary>
	/// <see href="http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:Ichimoku_cloud">StockCharts.com</see>
	public abstract class AbstractIchimokuLineIndicator : CachedIndicator<Decimal>
	{
		/// <summary>
		/// The period high </summary>
		private readonly IIndicator<Decimal> _periodHigh;

		/// <summary>
		/// The period low </summary>
		private readonly IIndicator<Decimal> _periodLow;

		/// <summary>
		/// Contructor. </summary>
		/// <param name="series"> the series </param>
		/// <param name="timeFrame"> the time frame </param>
		public AbstractIchimokuLineIndicator(TimeSeries series, int timeFrame) : base(series)
		{
			_periodHigh = new HighestValueIndicator(new MaxPriceIndicator(series), timeFrame);
			_periodLow = new LowestValueIndicator(new MinPriceIndicator(series), timeFrame);
		}

		protected override Decimal Calculate(int index)
		{
			return _periodHigh.GetValue(index).Plus(_periodLow.GetValue(index)).DividedBy(Decimal.Two);
		}
	}
}