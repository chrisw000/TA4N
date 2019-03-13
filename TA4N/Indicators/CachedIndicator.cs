using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

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
	/// Cached <seealso cref="IIndicator{T}"/>.
	/// <para>
	/// Caches the constructor of the indicator. Avoid to calculate the same index of the indicator twice.
	/// </para>
	/// </summary>
	public abstract class CachedIndicator<T> : AbstractIndicator<T>
	{
		/// <summary>
		/// List of cached results </summary>
		private readonly List<T> _results = new List<T>();

		/// <summary>
		/// Should always be the index of the last result in the results list.
		/// I.E. the last calculated result.
		/// </summary>
		protected internal int HighestResultIndex = -1;

		/// <summary>
		/// Constructor. </summary>
		/// <param name="series"> the related time series </param>
		public CachedIndicator(TimeSeries series) : base(series)
		{
		}

		/// <summary>
		/// Constructor. </summary>
		/// <param name="indicator"> a related indicator (with a time series) </param>
		public CachedIndicator(IIndicator<T> indicator) : this(indicator.TimeSeries)
		{
		}

		public override T GetValue(int index)
		{
			var series = TimeSeries;
			if (series == null)
			{
				// Series is null; the indicator doesn't need cache.
				// (e.g. simple computation of the value)
				// --> Calculating the value
				return Calculate(index);
			}

			// Series is not null

			var removedTicksCount = series.RemovedTicksCount;

			var maximumResultCount = TimeSeries.MaximumTickCount;

			T result;
			if (index < removedTicksCount)
			{
				// Result already removed from cache
				Log.Trace("{0}: result from tick {1} already removed from cache, use {2}-th instead", GetType().Name, index, removedTicksCount);
				IncreaseLengthTo(removedTicksCount, maximumResultCount);
				HighestResultIndex = removedTicksCount;
				result = _results[0];
				if (result == null)
				{
					result = Calculate(removedTicksCount);
					_results[0] = result;
				}
			}
			else
			{
				IncreaseLengthTo(index, maximumResultCount);
				if (index > HighestResultIndex)
				{
					// Result not calculated yet
					HighestResultIndex = index;
					result = Calculate(index);
					_results[_results.Count - 1] = result;
				}
				else
				{
					// Result covered by current cache
					var resultInnerIndex = _results.Count - 1 - (HighestResultIndex - index);
					result = _results[resultInnerIndex];
					if (result == null)
					{
						result = Calculate(index);
					}
					_results[resultInnerIndex] = result;
				}
			}
			return result;
		}

		/// <param name="index"> the tick index </param>
		/// <returns> the value of the indicator </returns>
		protected abstract T Calculate(int index);

		/// <summary>
		/// Increases the size of cached results buffer. </summary>
		/// <param name="index"> the index to increase length to </param>
		/// <param name="maxLength"> the maximum length of the results buffer </param>
		private void IncreaseLengthTo(int index, int maxLength)
		{
			if (HighestResultIndex > -1)
			{
				var newResultsCount = Math.Min(index - HighestResultIndex, maxLength);
				if (newResultsCount == maxLength)
				{
					_results.Clear();
				    _results.AddRange(Enumerable.Repeat<T>(default(T), maxLength).ToList<T>());
				}
				else if (newResultsCount > 0)
				{
                    _results.AddRange(Enumerable.Repeat<T>(default(T), newResultsCount).ToList<T>());
					RemoveExceedingResults(maxLength);
				}
			}
			else
			{
				// First use of cache
				Debug.Assert(_results.Count == 0, "Cache results list should be empty");
                _results.AddRange(Enumerable.Repeat<T>(default(T), Math.Min(index + 1, maxLength)).ToList<T>());
			}
		}

		/// <summary>
		/// Removes the N first results which exceed the maximum tick count.
		/// (i.e. keeps only the last maximumResultCount results) </summary>
		/// <param name="maximumResultCount"> the number of results to keep </param>
		private void RemoveExceedingResults(int maximumResultCount)
		{
			var resultCount = _results.Count;
			if (resultCount > maximumResultCount)
			{
				// Removing old results
				var nbResultsToRemove = resultCount - maximumResultCount;
				for (var i = 0; i < nbResultsToRemove; i++)
				{
					_results.RemoveAt(0);
				}
			}
		}
	}
}