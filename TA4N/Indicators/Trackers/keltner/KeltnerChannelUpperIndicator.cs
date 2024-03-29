﻿/*
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
namespace TA4N.Indicators.Trackers.Keltner
{
    using AverageTrueRangeIndicator = Helpers.AverageTrueRangeIndicator;

    /// <summary>
    /// Keltner Channel (upper line) indicator </summary>
    /// <see href="http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:keltner_channels">StockCharts.com</see>
    public class KeltnerChannelUpperIndicator : CachedIndicator<Decimal>
	{
		private readonly AverageTrueRangeIndicator _averageTrueRangeIndicator;
		private readonly KeltnerChannelMiddleIndicator _keltnerMiddleIndicator;
		private readonly Decimal _ratio;

		public KeltnerChannelUpperIndicator(KeltnerChannelMiddleIndicator keltnerMiddleIndicator, Decimal ratio, int timeFrameAtr) : base(keltnerMiddleIndicator)
		{
			_ratio = ratio;
			_keltnerMiddleIndicator = keltnerMiddleIndicator;
			_averageTrueRangeIndicator = new AverageTrueRangeIndicator(keltnerMiddleIndicator.TimeSeries, timeFrameAtr);
		}

		protected override Decimal Calculate(int index)
		{
			return _keltnerMiddleIndicator.GetValue(index).Plus(_ratio.MultipliedBy(_averageTrueRangeIndicator.GetValue(index)));
		}
	}
}