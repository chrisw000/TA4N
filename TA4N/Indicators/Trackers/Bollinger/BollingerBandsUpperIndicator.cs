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
namespace TA4N.Indicators.Trackers.Bollinger
{
    /// <summary>
	/// Buy - Occurs when the price line cross from down to up de Bollinger Band Low.
	/// Sell - Occurs when the price line cross from up to down de Bollinger Band High.
	/// </summary>
	public class BollingerBandsUpperIndicator : CachedIndicator<Decimal>
	{
        private readonly IIndicator<Decimal> _indicator;
        private readonly BollingerBandsMiddleIndicator _bbm;
        private readonly Decimal _k;

		public BollingerBandsUpperIndicator(BollingerBandsMiddleIndicator bbm, IIndicator<Decimal> indicator) : this(bbm, indicator, Decimal.Two)
		{
		}

		public BollingerBandsUpperIndicator(BollingerBandsMiddleIndicator bbm, IIndicator<Decimal> indicator, Decimal k) : base(indicator)
		{
			_bbm = bbm;
			_indicator = indicator;
			_k = k;
		}

		protected override Decimal Calculate(int index)
		{
			return _bbm.GetValue(index).Plus(_indicator.GetValue(index).MultipliedBy(_k));
		}

		/// <returns> the K multiplier </returns>
		public virtual Decimal K => _k;

	    public override string ToString()
		{
			return GetType().Name + "deviation: " + _indicator + "series" + _bbm;
		}
	}
}