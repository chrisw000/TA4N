﻿using TA4N.Indicators.Trackers;

/// <summary>
/// The MIT License (MIT)
/// 
/// Copyright (c) 2014-2016 Marc de Verdelhan & respective authors (see AUTHORS)
/// 
/// Permission is hereby granted, free of charge, to any person obtaining a copy of
/// this software and associated documentation files (the "Software"), to deal in
/// the Software without restriction, including without limitation the rights to
/// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
/// the Software, and to permit persons to whom the Software is furnished to do so,
/// subject to the following conditions:
/// 
/// The above copyright notice and this permission notice shall be included in all
/// copies or substantial portions of the Software.
/// 
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
/// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
/// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
/// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
/// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
/// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
/// </summary>
namespace TA4N.Indicators.Volume
{
    /// <summary>
	/// The Moving volume weighted average price (MVWAP) Indicator. </summary>
	/// <see href="http://www.investopedia.com/articles/trading/11/trading-with-vwap-mvwap.asp">Investopedia.com</see>
	public class MvwapIndicator : CachedIndicator<Decimal>
	{
		private readonly IIndicator<Decimal> _sma;

		/// <summary>
		/// Constructor. </summary>
		/// <param name="vwap"> the vwap </param>
		/// <param name="timeFrame"> the time frame </param>
		public MvwapIndicator(VwapIndicator vwap, int timeFrame) : base(vwap)
		{
			_sma = new SmaIndicator(vwap, timeFrame);
		}

		protected override Decimal Calculate(int index)
		{
			return _sma.GetValue(index);
		}

	}

}