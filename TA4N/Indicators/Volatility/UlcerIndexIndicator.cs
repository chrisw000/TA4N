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
namespace TA4N.Indicators.Volatility
{
    using Helpers;

    /// <summary>
	/// Ulcer index indicator.
	/// <para>
	/// </para>
	/// </summary>
	/// <see href="http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:ulcer_index">StockCharts.com</see>
	/// <seealso cref= https://en.wikipedia.org/wiki/Ulcer_index </seealso>
	public class UlcerIndexIndicator : CachedIndicator<Decimal>
	{
        private readonly IIndicator<Decimal> _indicator;
        private readonly HighestValueIndicator _highestValueInd;
        private readonly int _timeFrame;

		/// <summary>
		/// Constructor. </summary>
		/// <param name="indicator"> the indicator </param>
		/// <param name="timeFrame"> the time frame </param>
		public UlcerIndexIndicator(IIndicator<Decimal> indicator, int timeFrame) : base(indicator)
		{
			_indicator = indicator;
			_timeFrame = timeFrame;
			_highestValueInd = new HighestValueIndicator(indicator, timeFrame);
		}

		protected override Decimal Calculate(int index)
		{
			var startIndex = Math.Max(0, index - _timeFrame + 1);
			var numberOfObservations = index - startIndex + 1;
			var squaredAverage = Decimal.Zero;
			for (var i = startIndex; i <= index; i++)
			{
				var currentValue = _indicator.GetValue(i);
				var highestValue = _highestValueInd.GetValue(i);
				var percentageDrawdown = currentValue.Minus(highestValue).DividedBy(highestValue).MultipliedBy(Decimal.Hundred);
				squaredAverage = squaredAverage.Plus(percentageDrawdown.Pow(2));
			}
			squaredAverage = squaredAverage.DividedBy(Decimal.ValueOf(numberOfObservations));
			return squaredAverage.Sqrt();
		}

		public override string ToString()
		{
			return GetType().Name + " timeFrame: " + _timeFrame;
		}
	}
}