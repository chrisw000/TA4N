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
	using ClosePriceIndicator = Simple.ClosePriceIndicator;
	using MaxPriceIndicator = Simple.MaxPriceIndicator;
	using MinPriceIndicator = Simple.MinPriceIndicator;
    
	/// <summary>
	/// Stochastic oscillator K.
	/// <para>
	/// Receives timeSeries and timeFrame and calculates the StochasticOscillatorKIndicator
	/// over ClosePriceIndicator, or receives an indicator, MaxPriceIndicator and
	/// MinPriceIndicator and returns StochasticOsiclatorK over this indicator.
	/// </para>
	/// </summary>
	public class StochasticOscillatorKIndicator : CachedIndicator<Decimal>
	{
		private readonly IIndicator<Decimal> _indicator;
        private readonly int _timeFrame;
        private readonly MaxPriceIndicator _maxPriceIndicator;
        private readonly MinPriceIndicator _minPriceIndicator;

		public StochasticOscillatorKIndicator(TimeSeries timeSeries, int timeFrame) : this(new ClosePriceIndicator(timeSeries), timeFrame, new MaxPriceIndicator(timeSeries), new MinPriceIndicator(timeSeries))
		{
		}

		internal StochasticOscillatorKIndicator(IIndicator<Decimal> indicator, int timeFrame, MaxPriceIndicator maxPriceIndicator, MinPriceIndicator minPriceIndicator) : base(indicator)
		{
			_indicator = indicator;
			_timeFrame = timeFrame;
			_maxPriceIndicator = maxPriceIndicator;
			_minPriceIndicator = minPriceIndicator;
		}

		protected override Decimal Calculate(int index)
		{
			var highestHigh = new HighestValueIndicator(_maxPriceIndicator, _timeFrame);
			var lowestMin = new LowestValueIndicator(_minPriceIndicator, _timeFrame);

			var highestHighPrice = highestHigh.GetValue(index);
			var lowestLowPrice = lowestMin.GetValue(index);

			return _indicator.GetValue(index).Minus(lowestLowPrice).DividedBy(highestHighPrice.Minus(lowestLowPrice)).MultipliedBy(Decimal.Hundred);
		}

		public override string ToString()
		{
			return GetType().Name + " timeFrame: " + _timeFrame;
		}
	}
}