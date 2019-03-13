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
namespace TA4N.Indicators.Helpers
{
    /// <summary>
	/// Average true range indicator.
	/// </summary>
	public class AverageTrueRangeIndicator : RecursiveCachedIndicator<Decimal>
	{
        private readonly int _timeFrame;
		private readonly TrueRangeIndicator _tr;

		public AverageTrueRangeIndicator(TimeSeries series, int timeFrame) : base(series)
		{
			_timeFrame = timeFrame;
			_tr = new TrueRangeIndicator(series);
		}

		protected override Decimal Calculate(int index)
		{
			if (index == 0)
			{
				return Decimal.One;
			}
			var nbPeriods = Decimal.ValueOf(_timeFrame);
			var nbPeriodsMinusOne = Decimal.ValueOf(_timeFrame - 1);
			return GetValue(index - 1).MultipliedBy(nbPeriodsMinusOne).Plus(_tr.GetValue(index)).DividedBy(nbPeriods);
		}
	}
}