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
    using TA4N.Indicators.Statistics;

    /// <summary>
    /// %B indicator. </summary>
    /// <see href="http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:Bollinger_band_perce">StockCharts.com</see>
    public class PercentBIndicator : CachedIndicator<Decimal>
	{
        private readonly IIndicator<Decimal> _indicator;
        private readonly BollingerBandsUpperIndicator _bbu;
        private readonly BollingerBandsMiddleIndicator _bbm;
        private readonly BollingerBandsLowerIndicator _bbl;

		/// <summary>
		/// Constructor. </summary>
		/// <param name="indicator"> an indicator (usually close price) </param>
		/// <param name="timeFrame"> the time frame </param>
		/// <param name="k"> the K multiplier (usually 2.0) </param>
		public PercentBIndicator(IIndicator<Decimal> indicator, int timeFrame, Decimal k) : base(indicator)
		{
			_indicator = indicator;
			_bbm = new BollingerBandsMiddleIndicator(new SmaIndicator(indicator, timeFrame));
			var sd = new StandardDeviationIndicator(indicator, timeFrame);
			_bbu = new BollingerBandsUpperIndicator(_bbm, sd, k);
			_bbl = new BollingerBandsLowerIndicator(_bbm, sd, k);
		}

		protected override Decimal Calculate(int index)
		{
			var value = _indicator.GetValue(index);
			var upValue = _bbu.GetValue(index);
			var lowValue = _bbl.GetValue(index);
			return value.Minus(lowValue).DividedBy(upValue.Minus(lowValue));
		}
	}
}