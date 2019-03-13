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
    using AwesomeOscillatorIndicator = Oscillators.AwesomeOscillatorIndicator;
	using MedianPriceIndicator = Simple.MedianPriceIndicator;

	/// <summary>
	/// Acceleration-deceleration indicator.
	/// <para>
	/// </para>
	/// </summary>
	public class AccelerationDecelerationIndicator : CachedIndicator<Decimal>
	{
        private readonly AwesomeOscillatorIndicator _awesome;
        private readonly SmaIndicator _sma5;

		public AccelerationDecelerationIndicator(TimeSeries series, int timeFrameSma1 = 5, int timeFrameSma2 = 34) : base(series)
		{
			_awesome = new AwesomeOscillatorIndicator(new MedianPriceIndicator(series), timeFrameSma1, timeFrameSma2);
			_sma5 = new SmaIndicator(_awesome, timeFrameSma1);
		}

        protected override Decimal Calculate(int index)
		{
			return _awesome.GetValue(index).Minus(_sma5.GetValue(index));
		}
	}
}