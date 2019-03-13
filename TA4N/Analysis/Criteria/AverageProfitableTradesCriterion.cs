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
namespace TA4N.Analysis.Criteria
{
	/// <summary>
	/// Average profitable trades criterion.
	/// <para>
	/// The number of profitable trades.
	/// </para>
	/// </summary>
	public class AverageProfitableTradesCriterion : AbstractAnalysisCriterion
	{
		public override double Calculate(TimeSeries series, Trade trade)
		{
			var entryIndex = trade.Entry.Index;
			var exitIndex = trade.Exit.Index;

			Decimal result;
			if (trade.Entry.IsBuy)
			{
				// buy-then-sell trade
				result = series.GetTick(exitIndex).ClosePrice.DividedBy(series.GetTick(entryIndex).ClosePrice);
			}
			else
			{
				// sell-then-buy trade
				result = series.GetTick(entryIndex).ClosePrice.DividedBy(series.GetTick(exitIndex).ClosePrice);
			}

			return (result.IsGreaterThan(Decimal.One)) ? 1d : 0d;
		}

		public override double Calculate(TimeSeries series, TradingRecord tradingRecord)
		{
			var numberOfProfitable = 0;
			foreach (var trade in tradingRecord.Trades)
			{
				var entryIndex = trade.Entry.Index;
				var exitIndex = trade.Exit.Index;

				Decimal result;
				if (trade.Entry.IsBuy)
				{
					// buy-then-sell trade
					result = series.GetTick(exitIndex).ClosePrice.DividedBy(series.GetTick(entryIndex).ClosePrice);
				}
				else
				{
					// sell-then-buy trade
					result = series.GetTick(entryIndex).ClosePrice.DividedBy(series.GetTick(exitIndex).ClosePrice);
				}
				if (result.IsGreaterThan(Decimal.One))
				{
					numberOfProfitable++;
				}
			}
			return ((double) numberOfProfitable) / tradingRecord.TradeCount;
		}

		public override bool BetterThan(double criterionValue1, double criterionValue2)
		{
			return criterionValue1 > criterionValue2;
		}
	}
}