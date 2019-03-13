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
namespace TA4N.Indicators.Statistics
{
    /// <summary>
	/// Simple linear regression indicator.
	/// <para>
	/// A moving (i.e. over the time frame) simple linear regression (least squares).
	/// y = slope * x + intercept
	/// See also: http://introcs.cs.princeton.edu/java/97data/LinearRegression.java.html
	/// </para>
	/// </summary>
	public class SimpleLinearRegressionIndicator : CachedIndicator<Decimal>
	{
        private readonly IIndicator<Decimal> _indicator;
        private readonly int _timeFrame;
        private Decimal _slope;
        private Decimal _intercept;

		public SimpleLinearRegressionIndicator(IIndicator<Decimal> indicator, int timeFrame) : base(indicator)
		{
			_indicator = indicator;
			_timeFrame = timeFrame;
		}

		protected override Decimal Calculate(int index)
		{
			var startIndex = Math.Max(0, index - _timeFrame + 1);
			var endIndex = index;
			if (endIndex - startIndex + 1 < 2)
			{
				// Not enough observations to compute a regression line
				return Decimal.NaNRenamed;
			}
			CalculateRegressionLine(startIndex, endIndex);
			return _slope.MultipliedBy(Decimal.ValueOf(index)).Plus(_intercept);
		}

		/// <summary>
		/// Calculates the regression line. </summary>
		/// <param name="startIndex"> the start index (inclusive) in the time series </param>
		/// <param name="endIndex"> the end index (inclusive) in the time series </param>
		private void CalculateRegressionLine(int startIndex, int endIndex)
		{
			// First pass: compute xBar and yBar
			var sumX = Decimal.Zero;
			var sumY = Decimal.Zero;
			for (var i = startIndex; i <= endIndex; i++)
			{
				sumX = sumX.Plus(Decimal.ValueOf(i));
				sumY = sumY.Plus(_indicator.GetValue(i));
			}
			var nbObservations = Decimal.ValueOf(endIndex - startIndex + 1);
			var xBar = sumX.DividedBy(nbObservations);
			var yBar = sumY.DividedBy(nbObservations);

			// Second pass: compute slope and intercept
			var xxBar = Decimal.Zero;
			var xyBar = Decimal.Zero;
			for (var i = startIndex; i <= endIndex; i++)
			{
				var dX = Decimal.ValueOf(i).Minus(xBar);
				var dY = _indicator.GetValue(i).Minus(yBar);
				xxBar = xxBar.Plus(dX.MultipliedBy(dX));
				xyBar = xyBar.Plus(dX.MultipliedBy(dY));
			}

			_slope = xyBar.DividedBy(xxBar);
			_intercept = yBar.Minus(_slope.MultipliedBy(xBar));
		}
	}
}