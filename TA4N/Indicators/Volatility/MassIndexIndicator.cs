using System;
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
namespace TA4N.Indicators.Volatility
{
    using DifferenceIndicator = Simple.DifferenceIndicator;
	using MaxPriceIndicator = Simple.MaxPriceIndicator;
	using MinPriceIndicator = Simple.MinPriceIndicator;

    /// <summary>
	/// Mass index indicator.
	/// <para>
	/// </para>
	/// </summary>
	/// <see href="http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:mass_index">StockCharts.com</see>
	public class MassIndexIndicator : CachedIndicator<Decimal>
	{
        private readonly EmaIndicator _singleEma;
        private readonly EmaIndicator _doubleEma;
        private readonly int _timeFrame;

		/// <summary>
		/// Constructor. </summary>
		/// <param name="series"> the time series </param>
		/// <param name="emaTimeFrame"> the time frame for EMAs (usually 9) </param>
		/// <param name="timeFrame"> the time frame </param>
		public MassIndexIndicator(TimeSeries series, int emaTimeFrame, int timeFrame) : base(series)
		{
			IIndicator<Decimal> highLowDifferential = new DifferenceIndicator(new MaxPriceIndicator(series), new MinPriceIndicator(series)
		   );
			_singleEma = new EmaIndicator(highLowDifferential, emaTimeFrame);
			_doubleEma = new EmaIndicator(_singleEma, emaTimeFrame); // Not the same formula as DoubleEMAIndicator
			_timeFrame = timeFrame;
		}

		protected override Decimal Calculate(int index)
		{
			var startIndex = Math.Max(0, index - _timeFrame + 1);
			var massIndex = Decimal.Zero;
			for (var i = startIndex; i <= index; i++)
			{
				var emaRatio = _singleEma.GetValue(i).DividedBy(_doubleEma.GetValue(i));
				massIndex = massIndex.Plus(emaRatio);
			}
			return massIndex;
		}
	}
}