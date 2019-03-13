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
namespace TA4N.Indicators
{
    /// <summary>
	/// Recursive cached <seealso cref="IIndicator{T}"/>.
	/// <para>
	/// Recursive indicators should extend this class.<br/>
	/// This class is only here to avoid (OK, to postpone) the StackOverflowError that
	/// may be thrown on the first getValue(int) call of a recursive indicator.
	/// Concretely when an index value is asked, if the last cached value is too old/far,
	/// the computation of all the values between the last cached and the asked one
	/// is executed iteratively.
	/// </para>
	/// </summary>
	public abstract class RecursiveCachedIndicator<T> : CachedIndicator<T>
	{
        /// <summary>
		/// The recursion threshold for which an iterative calculation is executed.
		/// @TODO Should be variable (depending on the sub-indicators used in this indicator)
		/// </summary>
		private const int RecursionThreshold = 100;

		/// <summary>
		/// Constructor. </summary>
		/// <param name="series"> the related time series </param>
		public RecursiveCachedIndicator(TimeSeries series) : base(series)
		{
		}

		/// <summary>
		/// Constructor. </summary>
		/// <param name="indicator"> a related indicator (with a time series) </param>
		public RecursiveCachedIndicator(IIndicator<T> indicator) : this(indicator.TimeSeries)
		{
		}

		public override T GetValue(int index)
		{
			var series = TimeSeries;
			if (series != null)
			{
				var seriesEndIndex = series.End;
				if (index <= seriesEndIndex)
				{
					// We are not after the end of the series
					var removedTicksCount = series.RemovedTicksCount;
					var startIndex = Math.Max(removedTicksCount, HighestResultIndex);
					if (index - startIndex > RecursionThreshold)
					{
						// Too many un calculated values; the risk for a StackOverflowError becomes high.
						// Calculating the previous values iteratively
						for (var prevIdx = startIndex; prevIdx < index; prevIdx++)
						{
							base.GetValue(prevIdx);
						}
					}
				}
			}

			return base.GetValue(index);
		}
	}
}