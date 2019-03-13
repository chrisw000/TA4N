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
	/// Exponential moving average indicator.
	/// </summary>
	public class EmaIndicator : RecursiveCachedIndicator<Decimal>
	{
		private readonly IIndicator<Decimal> _indicator;
		private readonly int _timeFrame;
		private readonly Decimal _multiplier;

		/// <summary>
		/// Constructor. </summary>
		/// <param name="indicator"> an indicator </param>
		/// <param name="timeFrame"> the EMA time frame </param>
		public EmaIndicator(IIndicator<Decimal> indicator, int timeFrame) : base(indicator)
		{
			_indicator = indicator;
			_timeFrame = timeFrame;
			_multiplier = Decimal.Two.DividedBy(Decimal.ValueOf(timeFrame + 1));
		}

		protected override Decimal Calculate(int index)
		{
			if (index + 1 < _timeFrame)
			{
				// Starting point of the EMA
				return (new SmaIndicator(_indicator, _timeFrame)).GetValue(index);
			}
			if (index == 0)
			{
				// If the timeframe is bigger than the indicator's value count
				return _indicator.GetValue(0);
			}
			var emaPrev = GetValue(index - 1);
			return _indicator.GetValue(index).Minus(emaPrev).MultipliedBy(_multiplier).Plus(emaPrev);
		}
	}
}