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
	/// Directional movement up indicator.
	/// </summary>
	public class DirectionalMovementUpIndicator : CachedIndicator<Decimal>
	{
		private readonly TimeSeries _series;

		public DirectionalMovementUpIndicator(TimeSeries series) : base(series)
		{
			_series = series;
		}

		protected override Decimal Calculate(int index)
		{
			if (index == 0)
			{
				return Decimal.Zero;
			}
			var prevMaxPrice = _series.GetTick(index - 1).MaxPrice;
			var maxPrice = _series.GetTick(index).MaxPrice;
			var prevMinPrice = _series.GetTick(index - 1).MinPrice;
			var minPrice = _series.GetTick(index).MinPrice;

			if ((maxPrice.IsLessThan(prevMaxPrice) && minPrice.IsGreaterThan(prevMinPrice)) || prevMinPrice.Minus(minPrice).IsEqual(maxPrice.Minus(prevMaxPrice)))
			{
				return Decimal.Zero;
			}
			if (maxPrice.Minus(prevMaxPrice).IsGreaterThan(prevMinPrice.Minus(minPrice)))
			{
				return maxPrice.Minus(prevMaxPrice);
			}

			return Decimal.Zero;
		}
	}
}