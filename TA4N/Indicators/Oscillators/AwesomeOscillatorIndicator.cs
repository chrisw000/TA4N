using TA4N.Indicators.Trackers;

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
    /// <summary>
	/// Awesome oscillator. (AO)
	/// </summary>
	/// <see href="http://www.forexgurus.co.uk/indicators/awesome-oscillator">forexgurus.co.uk</see>
	public class AwesomeOscillatorIndicator : CachedIndicator<Decimal>
	{
        private readonly SmaIndicator _sma5;
        private readonly SmaIndicator _sma34;

        public AwesomeOscillatorIndicator(IIndicator<Decimal> indicator, int timeFrameSma1=5, int timeFrameSma2=34) : base(indicator)
		{
			_sma5 = new SmaIndicator(indicator, timeFrameSma1);
			_sma34 = new SmaIndicator(indicator, timeFrameSma2);
		}

		protected override Decimal Calculate(int index)
		{
			return _sma5.GetValue(index).Minus(_sma34.GetValue(index));
		}
	}
}