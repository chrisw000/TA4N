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
namespace TA4N.Indicators.Oscillators
{
    using CumulatedGainsIndicator = Helpers.CumulatedGainsIndicator;
	using CumulatedLossesIndicator = Helpers.CumulatedLossesIndicator;

    /// <summary>
    /// Chande Momentum Oscillator indicator.
    /// </summary>
    /// <see href="http://tradingsim.com/blog/chande-momentum-oscillator-cmo-technical-indicator">TradingSim.com</see>
    /// <see href="http://www.investopedia.com/terms/c/chandemomentumoscillator.asp">Investopedia.com</see>
    public class CmoIndicator : CachedIndicator<Decimal>
	{
        private readonly CumulatedGainsIndicator _cumulatedGains;
        private readonly CumulatedLossesIndicator _cumulatedLosses;

		/// <summary>
		/// Constructor. </summary>
		/// <param name="price"> a price indicator </param>
		/// <param name="timeFrame"> the time frame </param>
		public CmoIndicator(IIndicator<Decimal> price, int timeFrame) : base(price)
		{
			_cumulatedGains = new CumulatedGainsIndicator(price, timeFrame);
			_cumulatedLosses = new CumulatedLossesIndicator(price, timeFrame);
		}

		protected override Decimal Calculate(int index)
		{
			var sumOfGains = _cumulatedGains.GetValue(index);
			var sumOfLosses = _cumulatedLosses.GetValue(index);
			return sumOfGains.Minus(sumOfLosses).DividedBy(sumOfGains.Plus(sumOfLosses)).MultipliedBy(Decimal.Hundred);
		}
	}
}