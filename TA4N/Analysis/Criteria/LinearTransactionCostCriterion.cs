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
	/// A linear transaction cost criterion.
	/// <para>
	/// That criterion calculate the transaction cost according to an initial traded amount
	/// and a linear function defined by a and b (a * x + b).
	/// </para>
	/// </summary>
	public class LinearTransactionCostCriterion : AbstractAnalysisCriterion
	{
		private readonly double _initialAmount;

		private readonly double _a;
		private readonly double _b;

		private readonly TotalProfitCriterion _profit;

		/// <summary>
		/// Constructor.
		/// (a * x) </summary>
		/// <param name="initialAmount"> the initially traded amount </param>
		/// <param name="a"> the a coefficient (e.g. 0.005 for 0.5% per <seealso cref="Order"/>) </param>
		public LinearTransactionCostCriterion(double initialAmount, double a) : this(initialAmount, a, 0)
		{
		}

		/// <summary>
		/// Constructor.
		/// (a * x + b) </summary>
		/// <param name="initialAmount"> the initially traded amount </param>
		/// <param name="a"> the a coefficient (e.g. 0.005 for 0.5% per <seealso cref="Order"/>) </param>
		/// <param name="b"> the b constant (e.g. 0.2 for $0.2 per <seealso cref="Order"/>) </param>
		public LinearTransactionCostCriterion(double initialAmount, double a, double b)
		{
			_initialAmount = initialAmount;
			_a = a;
			_b = b;
			_profit = new TotalProfitCriterion();
		}

		public override double Calculate(TimeSeries series, Trade trade)
		{
			return GetTradeCost(series, trade, _initialAmount);
		}

		public override double Calculate(TimeSeries series, TradingRecord tradingRecord)
		{
			var totalCosts = 0d;
			var tradedAmount = _initialAmount;

			foreach (var trade in tradingRecord.Trades)
			{
				var tradeCost = GetTradeCost(series, trade, tradedAmount);
				totalCosts += tradeCost;
				// To calculate the new traded amount:
				//    - Remove the cost of the first order
				//    - Multiply by the profit ratio
				tradedAmount = (tradedAmount - tradeCost) * _profit.Calculate(series, trade);
			}

			// Special case: if the current trade is open
			var currentTrade = tradingRecord.CurrentTrade;
			if (currentTrade.Opened)
			{
				totalCosts += GetOrderCost(currentTrade.Entry, tradedAmount);
            }

			return totalCosts;
		}

		public override bool BetterThan(double criterionValue1, double criterionValue2)
		{
			return criterionValue1 < criterionValue2;
		}

		/// <param name="order"> a trade order </param>
		/// <param name="tradedAmount"> the traded amount for the order </param>
		/// <returns> the absolute order cost </returns>
		private double GetOrderCost(Order order, double tradedAmount)
		{
			var orderCost = 0d;
			if (order != null)
			{
				return _a * tradedAmount + _b;
			}
			return orderCost;
		}

		/// <param name="series"> the time series </param>
		/// <param name="trade"> a trade </param>
		/// <param name="initialAmount"> the initially traded amount for the trade </param>
		/// <returns> the absolute total cost of all orders in the trade </returns>
		private double GetTradeCost(TimeSeries series, Trade trade, double initialAmount)
		{
			var totalTradeCost = 0d;
            if (trade?.Entry != null)
            {
                totalTradeCost = GetOrderCost(trade.Entry, initialAmount);
                if (trade.Exit != null)
                {
                    // To calculate the new traded amount:
                    //    - Remove the cost of the first order
                    //    - Multiply by the profit ratio
                    var newTradedAmount = (initialAmount - totalTradeCost) * _profit.Calculate(series, trade);
                    totalTradeCost += GetOrderCost(trade.Exit, newTradedAmount);
                }
            }
            return totalTradeCost;
		}
	}
}