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
	/// Chande's Range Action Verification Index (RAVI) indicator.
	/// </summary>
	public class RaviIndicator : CachedIndicator<Decimal>
	{
        private readonly SmaIndicator _shortSma;
        private readonly SmaIndicator _longSma;

		/// <summary>
		/// Constructor. </summary>
		/// <param name="price"> the price </param>
		/// <param name="shortSmaTimeFrame"> the time frame for the short SMA (usually 7) </param>
		/// <param name="longSmaTimeFrame"> the time frame for the long SMA (usually 65) </param>
		public RaviIndicator(IIndicator<Decimal> price, int shortSmaTimeFrame, int longSmaTimeFrame) : base(price)
		{
			_shortSma = new SmaIndicator(price, shortSmaTimeFrame);
			_longSma = new SmaIndicator(price, longSmaTimeFrame);
		}

		protected override Decimal Calculate(int index)
		{
			var shortMa = _shortSma.GetValue(index);
			var longMa = _longSma.GetValue(index);
			return shortMa.Minus(longMa).DividedBy(longMa).MultipliedBy(Decimal.Hundred);
		}
	}
}