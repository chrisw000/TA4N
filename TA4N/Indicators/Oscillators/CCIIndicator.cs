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
    using MeanDeviationIndicator = Helpers.MeanDeviationIndicator;
	using TypicalPriceIndicator = Simple.TypicalPriceIndicator;

    /// <summary>
	/// Commodity Channel Index (CCI) indicator.
	/// <para>
	/// </para>
	/// </summary>
	/// <see href="http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:commodity_channel_in">StockCharts.com</see>
	public class CciIndicator : CachedIndicator<Decimal>
	{
        public static readonly Decimal Factor = Decimal.ValueOf("0.015");

		private readonly TypicalPriceIndicator _typicalPriceInd;
        private readonly SmaIndicator _smaInd;
        private readonly MeanDeviationIndicator _meanDeviationInd;
        private readonly int _timeFrame;

		/// <summary>
		/// Constructor. </summary>
		/// <param name="series"> the time series </param>
		/// <param name="timeFrame"> the time frame </param>
		public CciIndicator(TimeSeries series, int timeFrame) : base(series)
		{
			_typicalPriceInd = new TypicalPriceIndicator(series);
			_smaInd = new SmaIndicator(_typicalPriceInd, timeFrame);
			_meanDeviationInd = new MeanDeviationIndicator(_typicalPriceInd, timeFrame);
			_timeFrame = timeFrame;
		}

		protected override Decimal Calculate(int index)
		{
			var typicalPrice = _typicalPriceInd.GetValue(index);
			var typicalPriceAvg = _smaInd.GetValue(index);
			var meanDeviation = _meanDeviationInd.GetValue(index);
			if (meanDeviation.IsZero)
			{
				return Decimal.Zero;
			}
			return (typicalPrice.Minus(typicalPriceAvg)).DividedBy(meanDeviation.MultipliedBy(Factor));
		}

		public override string ToString()
		{
			return GetType().Name + " timeFrame: " + _timeFrame;
		}
	}
}