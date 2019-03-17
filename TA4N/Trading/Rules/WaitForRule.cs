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
	/// <summary>
	/// A <seealso cref="IRule"/> which waits for a number of <seealso cref="Tick ticks"/> after an order.
	/// <para>
	/// Satisfied after a fixed number of ticks since the last order.
	/// </para>
	/// </summary>
	public class WaitForRule : AbstractRule
	{
		/// <summary>
		/// The type of the order since we have to wait for </summary>
		private readonly OrderType _orderType;

		/// <summary>
		/// The number of ticks to wait for </summary>
		private readonly int _numberOfTicks;

		/// <summary>
		/// Constructor. </summary>
		/// <param name="orderType"> the type of the order since we have to wait for </param>
		/// <param name="numberOfTicks"> the number of ticks to wait for </param>
		public WaitForRule(OrderType orderType, int numberOfTicks)
		{
			_orderType = orderType;
			_numberOfTicks = numberOfTicks;
            logger = LogWrapper.Factory?.CreateLogger<WaitForRule>();
        }

		public override bool IsSatisfied(int index, TradingRecord tradingRecord)
		{
			var satisfied = false;
			// No trading history, no need to wait
            var lastOrder = tradingRecord?.GetLastOrder(_orderType);
            if (lastOrder != null)
            {
                var currentNumberOfTicks = index - lastOrder.Index;
                satisfied = currentNumberOfTicks >= _numberOfTicks;
            }
            TraceIsSatisfied(index, satisfied);
			return satisfied;
		}
	}
}