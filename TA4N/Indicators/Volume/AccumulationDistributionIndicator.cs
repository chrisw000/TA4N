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
namespace TA4N.Indicators.Volume
{
    using CloseLocationValueIndicator = Helpers.CloseLocationValueIndicator;

    public class AccumulationDistributionIndicator : RecursiveCachedIndicator<Decimal>
	{
        private readonly TimeSeries _series;
        private readonly CloseLocationValueIndicator _clvIndicator;

	    /// <summary>
	    /// <para>
	    /// Accumulation-distribution indicator.
	    /// <see cref="http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:accumulation_distribution_line"/>
	    /// </para>
	    /// <para>
	    /// The Accumulation Distribution Line is a cumulative measure of each period's volume flow, or money flow. 
	    /// A high positive multiplier combined with high volume shows strong buying pressure that pushes the indicator higher. 
	    /// Conversely, a low negative number combined with high volume reflects strong selling pressure that pushes the indicator lower. 
	    ///  Money Flow Volume [MFV is the delta thats calculated between ticks] accumulates to form a line that 
	    /// either confirms or contradicts the underlying price trend.
	    /// </para>
	    /// <para>
	    /// In this regard, the indicator is used to either reinforce the underlying trend or cast doubts on its sustainability. 
	    /// An uptrend in prices with a downtrend in the Accumulation Distribution Line suggests underlying selling pressure 
	    /// (distribution) that could foreshadow a bearish reversal on the price chart. 
	    /// </para>
	    /// <para>
	    ///  A downtrend in prices with an uptrend in the Accumulation Distribution Line indicate underlying buying 
	    /// pressure (accumulation) that could foreshadow a bullish reversal in prices.
	    /// </para>
	    /// </summary>
        public AccumulationDistributionIndicator(TimeSeries series) : base(series)
		{
			_series = series;
			_clvIndicator = new CloseLocationValueIndicator(series);
		}

		protected override Decimal Calculate(int index)
		{
			if (index == 0)
			{
				return Decimal.Zero;
			}

			// Calculating the money flow multiplier
			var moneyFlowMultiplier = _clvIndicator.GetValue(index);

			// Calculating the money flow volume
			var moneyFlowVolume = moneyFlowMultiplier.MultipliedBy(_series.GetTick(index).Volume);

			return moneyFlowVolume.Plus(GetValue(index - 1));
		}
	}
}