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
namespace TA4N.Indicators.Statistics
{
    /// <summary>
	/// Covariance indicator.
	/// </summary>
	public class CovarianceIndicator : CachedIndicator<Decimal>
	{
		private readonly IIndicator<Decimal> _indicator1;
		private readonly IIndicator<Decimal> _indicator2;
		private readonly int _timeFrame;
		private readonly SmaIndicator _sma1;
		private readonly SmaIndicator _sma2;

		/// <summary>
		/// Constructor. </summary>
		/// <param name="indicator1"> the first indicator </param>
		/// <param name="indicator2"> the second indicator </param>
		/// <param name="timeFrame"> the time frame </param>
		public CovarianceIndicator(IIndicator<Decimal> indicator1, IIndicator<Decimal> indicator2, int timeFrame) : base(indicator1)
		{
			_indicator1 = indicator1;
			_indicator2 = indicator2;
			_timeFrame = timeFrame;
			_sma1 = new SmaIndicator(indicator1, timeFrame);
			_sma2 = new SmaIndicator(indicator2, timeFrame);
		}

		protected override Decimal Calculate(int index)
		{
			var startIndex = Math.Max(0, index - _timeFrame + 1);
			var numberOfObservations = index - startIndex + 1;
			var covariance = Decimal.Zero;
			var average1 = _sma1.GetValue(index);
			var average2 = _sma2.GetValue(index);
			for (var i = startIndex; i <= index; i++)
			{
				var mul = _indicator1.GetValue(i).Minus(average1).MultipliedBy(_indicator2.GetValue(i).Minus(average2));
				covariance = covariance.Plus(mul);
			}
			covariance = covariance.DividedBy(Decimal.ValueOf(numberOfObservations));
			return covariance;
		}

		public override string ToString()
		{
			return GetType().Name + " timeFrame: " + _timeFrame;
		}
	}
}