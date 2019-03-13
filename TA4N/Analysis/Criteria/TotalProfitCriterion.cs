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
	/// Total profit criterion.
	/// <para>
	/// The total profit of the provided <seealso cref="Trade trade(s)"/> over the provided <seealso cref="TimeSeries series"/>.
	/// </para>
	/// </summary>
	public class TotalProfitCriterion : AbstractAnalysisCriterion
	{
        public override double Calculate(TimeSeries series, TradingRecord tradingRecord)
		{
			var value = 1d;
			foreach (var trade in tradingRecord.Trades)
			{
				value *= CalculateProfit(series, trade);
			}
			return value;
		}

		public override double Calculate(TimeSeries series, Trade trade)
		{
			return CalculateProfit(series, trade);
		}

		public override bool BetterThan(double criterionValue1, double criterionValue2)
		{
			return criterionValue1 > criterionValue2;
		}

		/// <summary>
		/// Calculates the profit of a trade (Buy and sell). </summary>
		/// <param name="series"> a time series </param>
		/// <param name="trade"> a trade </param>
		/// <returns> the profit of the trade </returns>
		private double CalculateProfit(TimeSeries series, Trade trade)
		{
            var profit = Decimal.One;
            if (trade.Closed)
            {
                // use price of entry/exit order, if NaN use close price of underlying time series
                var exitClosePrice = trade.Exit.Price.NaN ?
                        series.GetTick(trade.Exit.Index).ClosePrice : trade.Exit.Price;
                var entryClosePrice = trade.Entry.Price.NaN ?
                        series.GetTick(trade.Entry.Index).ClosePrice : trade.Entry.Price;

                if (trade.Entry.IsBuy)
                {
                    profit = exitClosePrice.DividedBy(entryClosePrice);
                }
                else
                {
                    profit = entryClosePrice.DividedBy(exitClosePrice);
                }
            }
            return profit.ToDouble();
        }
	}
}