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

using Microsoft.Extensions.Logging;

namespace TA4N.Trading.Rules
{
    using ClosePriceIndicator = Indicators.Simple.ClosePriceIndicator;

	/// <summary>
	/// A stop-gain rule.
	/// <para>
	/// Satisfied when the close price reaches the gain threshold.
	/// </para>
	/// </summary>
	public class StopGainRule : AbstractRule
	{
		/// <summary>
		/// The close price indicator </summary>
		private readonly ClosePriceIndicator _closePrice;

		/// <summary>
		/// The gain ratio threshold (e.g. 1.03 for 3%) </summary>
		private readonly Decimal _gainRatioThreshold;

		/// <summary>
		/// Constructor. </summary>
		/// <param name="closePrice"> the close price indicator </param>
		/// <param name="gainPercentage"> the gain percentage </param>
		public StopGainRule(ClosePriceIndicator closePrice, Decimal gainPercentage)
		{
			_closePrice = closePrice;
			_gainRatioThreshold = Decimal.Hundred.Plus(gainPercentage).DividedBy(Decimal.Hundred);
            logger = LogWrapper.Factory?.CreateLogger<StopGainRule>();
		}

		public override bool IsSatisfied(int index, TradingRecord tradingRecord)
		{
            var satisfied = false;
			// No trading history or no trade opened, no gain
			if (tradingRecord != null)
			{
				var currentTrade = tradingRecord.CurrentTrade;
				if (currentTrade.Opened)
				{
					var entryPrice = currentTrade.Entry.Price;
					var currentPrice = _closePrice.GetValue(index);

                    var threshold = entryPrice.MultipliedBy(_gainRatioThreshold);
                    if (currentTrade.Entry.IsBuy)
                    {
                        satisfied = currentPrice.IsGreaterThanOrEqual(threshold);
                    }
                    else
                    {
                        satisfied = currentPrice.IsLessThanOrEqual(threshold);
                    }
				}
			}
			TraceIsSatisfied(index, satisfied);
			return satisfied;
		}
	}
}