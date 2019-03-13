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
namespace TA4N.Indicators.Trackers.Ichimoku
{
	/// <summary>
	/// Ichimoku clouds: Senkou Span A (Leading Span A) indicator
	/// <para>
	/// </para>
	/// </summary>
	/// <see href="http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:Ichimoku_cloud">StockCharts.com</see>
	public class IchimokuSenkouSpanAIndicator : CachedIndicator<Decimal>
	{
		/// <summary>
		/// The Tenkan-sen indicator </summary>
		private readonly IchimokuTenkanSenIndicator _conversionLine;

		/// <summary>
		/// The Kijun-sen indicator </summary>
		private readonly IchimokuKijunSenIndicator _baseLine;

		/// <summary>
		/// Constructor. </summary>
		/// <param name="series"> the series </param>
		public IchimokuSenkouSpanAIndicator(TimeSeries series) : this(series, new IchimokuTenkanSenIndicator(series), new IchimokuKijunSenIndicator(series))
		{
		}

		/// <summary>
		/// Constructor. </summary>
		/// <param name="series"> the series </param>
		/// <param name="timeFrameConversionLine"> the time frame for the conversion line (usually 9) </param>
		/// <param name="timeFrameBaseLine"> the time frame for the base line (usually 26) </param>
		public IchimokuSenkouSpanAIndicator(TimeSeries series, int timeFrameConversionLine, int timeFrameBaseLine) : this(series, new IchimokuTenkanSenIndicator(series, timeFrameConversionLine), new IchimokuKijunSenIndicator(series, timeFrameBaseLine))
		{
		}

		/// <summary>
		/// Constructor. </summary>
		/// <param name="series"> the series </param>
		/// <param name="conversionLine"> the conversion line </param>
		/// <param name="baseLine"> the base line </param>
		public IchimokuSenkouSpanAIndicator(TimeSeries series, IchimokuTenkanSenIndicator conversionLine, IchimokuKijunSenIndicator baseLine) : base(series)
		{
			_conversionLine = conversionLine;
			_baseLine = baseLine;
		}

		protected override Decimal Calculate(int index)
		{
			return _conversionLine.GetValue(index).Plus(_baseLine.GetValue(index)).DividedBy(Decimal.Two);
		}
	}
}