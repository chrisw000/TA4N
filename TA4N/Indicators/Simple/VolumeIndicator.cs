﻿using System;

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

namespace TA4N.Indicators.Simple
{
	/// <summary>
	/// Volume indicator.
	/// </summary>
	public class VolumeIndicator : CachedIndicator<Decimal>
	{
		private readonly TimeSeries _series;
		private readonly int _timeFrame;

		public VolumeIndicator(TimeSeries series, int timeFrame = 1) : base(series)
		{
			_series = series;
			_timeFrame = timeFrame;
		}

		protected override Decimal Calculate(int index)
		{
			var startIndex = Math.Max(0, index - _timeFrame + 1);
			var sumOfVolume = Decimal.Zero;
			for (var i = startIndex; i <= index; i++)
			{
				sumOfVolume = sumOfVolume.Plus(_series.GetTick(i).Volume);
			}
			return sumOfVolume;
		}
	}
}