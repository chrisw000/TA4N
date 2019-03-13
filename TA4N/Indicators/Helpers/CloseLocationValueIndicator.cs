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
	/// Close Location Value (CLV) indicator.
	/// </summary>
	/// <see href="http://www.investopedia.com/terms/c/close_location_value.asp">Investopedia.com</see>
	public class CloseLocationValueIndicator : CachedIndicator<Decimal>
	{
        private readonly TimeSeries _series;

		public CloseLocationValueIndicator(TimeSeries series) : base(series)
		{
			_series = series;
		}

		protected override Decimal Calculate(int index)
		{
			var tick = _series.GetTick(index);

            // I've changed this... was getting divide by zero.
			var cl = ((tick.ClosePrice.Minus(tick.MinPrice)).Minus(tick.MaxPrice.Minus(tick.ClosePrice))).DividedBy(tick.MaxPrice.Minus(tick.MinPrice));
            return cl.NaN ? Decimal.Zero : cl;
		}
	}
}