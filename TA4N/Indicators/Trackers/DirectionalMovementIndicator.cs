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
	using DirectionalDownIndicator = Helpers.DirectionalDownIndicator;
	using DirectionalUpIndicator = Helpers.DirectionalUpIndicator;

	/// <summary>
	/// Directional movement indicator.
	/// </summary>
	public class DirectionalMovementIndicator : CachedIndicator<Decimal>
	{
		private readonly int _timeFrame;
		private readonly DirectionalUpIndicator _dup;
		private readonly DirectionalDownIndicator _ddown;

		public DirectionalMovementIndicator(TimeSeries series, int timeFrame) : base(series)
		{
			_timeFrame = timeFrame;
			_dup = new DirectionalUpIndicator(series, timeFrame);
			_ddown = new DirectionalDownIndicator(series, timeFrame);
		}

		protected override Decimal Calculate(int index)
		{
			var dupValue = _dup.GetValue(index);
			var ddownValue = _ddown.GetValue(index);
			var difference = dupValue.Minus(ddownValue);
			return difference.Abs().DividedBy(dupValue.Plus(ddownValue)).MultipliedBy(Decimal.Hundred);
		}

		public override string ToString()
		{
			return GetType().Name + " timeFrame: " + _timeFrame;
		}
	}
}