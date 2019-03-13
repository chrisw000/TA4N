using System.Collections.Generic;

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
namespace TA4N
{
	/// <summary>
	/// An analysis criterion.
	/// <para>
	/// Can be used to:
	/// <ul>
	/// <li>Analyze the performance of a <seealso cref="Strategy"/></li>
	/// <li>Compare several <seealso cref="Strategy"/> together</li>
	/// </ul>
	/// </para>
	/// </summary>
	public interface IAnalysisCriterion
	{
 		/// <param name="series"> a time series </param>
        /// <param name="trade"> a trade </param>
        /// <returns> the criterion value for the trade </returns>
        double Calculate(TimeSeries series, Trade trade);

		/// <param name="series"> a time series </param>
		/// <param name="tradingRecord"> a trading record </param>
		/// <returns> the criterion value for the trades </returns>
		double Calculate(TimeSeries series, TradingRecord tradingRecord);

		/// <param name="series"> the time series </param>
		/// <param name="strategies"> a list of strategies </param>
		/// <returns> the best strategy (among the provided ones) according to the criterion </returns>
		Strategy ChooseBest(TimeSeries series, IList<Strategy> strategies);

		/// <param name="criterionValue1"> the first value </param>
		/// <param name="criterionValue2"> the second value </param>
		/// <returns> true if the first value is better than (according to the criterion) the second one, false otherwise </returns>
		bool BetterThan(double criterionValue1, double criterionValue2);
	}
}