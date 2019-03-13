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
	/// Cross indicator.
	/// <para>
	/// Boolean indicator which monitors two-indicators crossings.
	/// </para>
	/// </summary>
	public class CrossIndicator : CachedIndicator<bool>
	{
        /// <summary>
		/// Upper indicator </summary>
		private readonly IIndicator<Decimal> _up;
		/// <summary>
		/// Lower indicator </summary>
		private readonly IIndicator<Decimal> _low;

		/// <summary>
		/// Constructor. </summary>
		/// <param name="up"> the upper indicator </param>
		/// <param name="low"> the lower indicator </param>
		public CrossIndicator(IIndicator<Decimal> up, IIndicator<Decimal> low) : base(up.TimeSeries)
		{
			// TODO: check if up series is equal to low series
			_up = up;
			_low = low;
		}

		protected override bool Calculate(int index)
		{
			var i = index;
			if (i == 0 || _up.GetValue(i).IsGreaterThanOrEqual(_low.GetValue(i)))
			{
				return false;
			}

			i--;
			if (_up.GetValue(i).IsGreaterThan(_low.GetValue(i)))
			{
				return true;
			} else
			{

				while (i > 0 && _up.GetValue(i).IsEqual(_low.GetValue(i)))
				{
					i--;
				}
				return (i != 0) && (_up.GetValue(i).IsGreaterThan(_low.GetValue(i)));
			}
		}

		/// <returns> the initial lower indicator </returns>
		public virtual IIndicator<Decimal> Low => _low;

	    /// <returns> the initial upper indicator </returns>
		public virtual IIndicator<Decimal> Up => _up;

	    public override string ToString()
		{
			return $"{GetType().Name} {_low} {_up}";
		}
	}
}