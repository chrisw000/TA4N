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
    using TA4N;
    using TA4N.Analysis;

    /// <summary>
	/// Maximum drawdown criterion.
	/// </summary>
	/// <see href="http://en.wikipedia.org/wiki/Drawdown_%28economics%29">Wikipedia</see>
	public class MaximumDrawdownCriterion : AbstractAnalysisCriterion
	{
        public override double Calculate(TimeSeries series, TradingRecord tradingRecord)
		{
			var cashFlow = new CashFlow(series, tradingRecord);
			var maximumDrawdown = CalculateMaximumDrawdown(series, cashFlow);
			return maximumDrawdown.ToDouble();
		}

		public override double Calculate(TimeSeries series, Trade trade)
		{
			if (trade?.Entry != null && trade.Exit != null)
			{
				var cashFlow = new CashFlow(series, trade);
				var maximumDrawdown = CalculateMaximumDrawdown(series, cashFlow);
				return maximumDrawdown.ToDouble();
			}
			return 0;
		}

		public override bool BetterThan(double criterionValue1, double criterionValue2)
		{
			return criterionValue1 < criterionValue2;
		}

		/// <summary>
		/// Calculates the maximum drawdown from a cash flow over a series. </summary>
		/// <param name="series"> the time series </param>
		/// <param name="cashFlow"> the cash flow </param>
		/// <returns> the maximum drawdown from a cash flow over a series </returns>
		private Decimal CalculateMaximumDrawdown(TimeSeries series, CashFlow cashFlow)
		{
			var maximumDrawdown = Decimal.Zero;
			var maxPeak = Decimal.Zero;
			for (var i = series.Begin; i <= series.End; i++)
			{
				var value = cashFlow.GetValue(i);
				if (value.IsGreaterThan(maxPeak))
				{
					maxPeak = value;
				}

				var drawdown = maxPeak.Minus(value).DividedBy(maxPeak);
				if (drawdown.IsGreaterThan(maximumDrawdown))
				{
					maximumDrawdown = drawdown;
				}
			}
			return maximumDrawdown;
		}
	}
}