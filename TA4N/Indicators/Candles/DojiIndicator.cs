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
	using AbsoluteIndicator = Simple.AbsoluteIndicator;

    /// <summary>
	/// Doji indicator.
	/// <para>
	/// A candle/tick is considered Doji if its body height is lower than the average multiplied by a factor.
	/// </para>
	/// </summary>
	/// <see href="http://stockcharts.com/school/doku.php?id=chart_school:chart_analysis:introduction_to_candlesticks#doji">StockCharts.com</see>
	public class DojiIndicator : CachedIndicator<bool>
	{
		/// <summary>
		/// Body height </summary>
		private readonly IIndicator<Decimal> _bodyHeightInd;
		/// <summary>
		/// Average body height </summary>
		private readonly SmaIndicator _averageBodyHeightInd;

		private readonly Decimal _factor;

		/// <summary>
		/// Constructor. </summary>
		/// <param name="series"> a time series </param>
		/// <param name="timeFrame"> the number of ticks used to calculate the average body height </param>
		/// <param name="bodyFactor"> the factor used when checking if a candle is Doji </param>
		public DojiIndicator(TimeSeries series, int timeFrame, Decimal bodyFactor) : base(series)
		{
			_bodyHeightInd = new AbsoluteIndicator(new RealBodyIndicator(series));
			_averageBodyHeightInd = new SmaIndicator(_bodyHeightInd, timeFrame);
			_factor = bodyFactor;
		}

		protected override bool Calculate(int index)
		{
			if (index < 1)
			{
				return _bodyHeightInd.GetValue(index).IsZero;
			}

			var averageBodyHeight = _averageBodyHeightInd.GetValue(index - 1);
			var currentBodyHeight = _bodyHeightInd.GetValue(index);

			return currentBodyHeight.IsLessThan(averageBodyHeight.MultipliedBy(_factor));
		}
	}
}