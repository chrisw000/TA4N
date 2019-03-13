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
    using HighestValueIndicator = Helpers.HighestValueIndicator;
	using LowestValueIndicator = Helpers.LowestValueIndicator;
	using MaxPriceIndicator = Simple.MaxPriceIndicator;
	using MedianPriceIndicator = Simple.MedianPriceIndicator;
	using MinPriceIndicator = Simple.MinPriceIndicator;

    /// <summary>
    /// The Fisher Indicator. </summary>
    /// <see href="http://www.tradingsystemlab.com/files/The%20Fisher%20Transform.pdf">tradingsystemlab.com</see >
    public class FisherIndicator : RecursiveCachedIndicator<Decimal>
	{
        private static readonly double ZeroDotFive = 0.5;
		private static readonly Decimal ValueMax = 0.999;
		private static readonly Decimal ValueMin = -0.999;

		private readonly IIndicator<Decimal> _price;
        private readonly IIndicator<Decimal> _intermediateValue;
        private readonly Decimal _densityFactor;
        private readonly Decimal _gamma;
        private readonly Decimal _delta;
        
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="price"> the price indicator (usually <seealso cref="MedianPriceIndicator"/>) </param>
		/// <param name="timeFrame"> the time frame (usually 10) </param>
		public FisherIndicator(IIndicator<Decimal> price, int timeFrame=10) : this(price, timeFrame, 0.33, 0.67)
		{
		}

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="price"> the price indicator (usually <seealso cref="MedianPriceIndicator"/>) </param>
        /// <param name="timeFrame"> the time frame (usually 10) </param>
        /// <param name="alphaD"> the alpha (usually 0.33) </param>
        /// <param name="betaD"> the beta (usually 0.67) </param>
        internal FisherIndicator(IIndicator<Decimal> price, int timeFrame, double alphaD, double betaD)  : this(price, timeFrame, alphaD, betaD, ZeroDotFive, ZeroDotFive, 1.0, true)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="price"> the price indicator (usually <seealso cref="MedianPriceIndicator"/>) </param>
        /// <param name="timeFrame"> the time frame (usually 10) </param>
        /// <param name="alphaD"> the alpha (usually 0.33) </param>
        /// <param name="betaD"> the beta (usually 0.67) </param>
        /// <param name="gammaD">gammaD the gamma (usually 0.25 or 0.5)</param>
        /// <param name="deltaD">deltaD the delta (usually 0.5)</param>
        /// <param name="densityFactorD">densityFactorD the density factor (usually 1.0)</param>
        /// <param name="isPriceIndicator">isPriceIndicator use true, if "price" is a price indicator</param>
        internal FisherIndicator(IIndicator<Decimal> price, int timeFrame, double alphaD, double betaD, double gammaD, double deltaD, double densityFactorD, bool isPriceIndicator) : base(price)
        {
            _price = price;
            _gamma = Decimal.ValueOf(gammaD);
            _delta = Decimal.ValueOf(deltaD);
            _densityFactor = Decimal.ValueOf(densityFactorD);
            var alpha = Decimal.ValueOf(alphaD);
            var beta = Decimal.ValueOf(betaD);
            IIndicator<Decimal> periodHigh = new HighestValueIndicator(isPriceIndicator ? new MaxPriceIndicator(price.TimeSeries) : price, timeFrame);
            IIndicator<Decimal> periodLow = new LowestValueIndicator(isPriceIndicator ? new MinPriceIndicator(price.TimeSeries) : price, timeFrame);

			_intermediateValue = new RecursiveCachedIndicatorAnonymousInnerClass(this, alpha, beta, periodHigh, periodLow);
		}

		private class RecursiveCachedIndicatorAnonymousInnerClass : RecursiveCachedIndicator<Decimal>
		{
			private readonly FisherIndicator _outerInstance;

			private readonly Decimal _alpha;
			private readonly Decimal _beta;
			private readonly IIndicator<Decimal> _periodHigh;
			private readonly IIndicator<Decimal> _periodLow;

			public RecursiveCachedIndicatorAnonymousInnerClass(FisherIndicator outerInstance, Decimal alpha, Decimal beta, IIndicator<Decimal> periodHigh, IIndicator<Decimal> periodLow) : base(outerInstance._price)
			{
				_outerInstance = outerInstance;
				_alpha = alpha;
				_beta = beta;
				_periodHigh = periodHigh;
				_periodLow = periodLow;
			}
            
			protected override Decimal Calculate(int index)
			{
				if (index <= 0)
				{
					return Decimal.Zero;
				}
				// alpha * 2 * ((price - MinL) / (MaxH - MinL) - 0.5) + beta * prior value
				var currentPrice = _outerInstance._price.GetValue(index);
				var minL = _periodLow.GetValue(index);
				var maxH = _periodHigh.GetValue(index);
				var term1 = currentPrice.Minus(minL).DividedBy(maxH.Minus(minL)).Minus(ZeroDotFive);
				var term2 = _alpha.MultipliedBy(Decimal.Two).MultipliedBy(term1);
				var term3 = term2.Plus(_beta.MultipliedBy(GetValue(index - 1)));

                return term3.DividedBy(_outerInstance._densityFactor);
			}
		}

		protected override Decimal Calculate(int index)
		{
			if (index <= 0)
			{
				return Decimal.Zero;
			}

			var value = _intermediateValue.GetValue(index);

            if (value.IsGreaterThan(ValueMax))
            {
                value = ValueMax;
            } else if (value.IsLessThan(ValueMin))
            {
                value = ValueMin;
            }

            //Fish = gamma * MathLog((1 + Value) / (1 - Value)) + delta * priorFisher
            var term1 = Decimal.One.Plus(value).DividedBy(Decimal.One.Minus(value)).Log();
            var term2 = GetValue(index - 1);

            return _gamma.MultipliedBy(term1).Plus(_delta.MultipliedBy(term2));
		}
	}
}