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
namespace TA4N.Trading.Rules
{
    using Indicators.Simple;

	/// <summary>
	/// Indicator-over-indicator rule.
	/// <para>
	/// Satisfied when the value of the first <seealso cref="IIndicator{T}"/> is strictly greater than the value of the second one.
	/// </para>
	/// </summary>
	public class OverIndicatorRule : AbstractRule
	{
        /// <summary>
		/// The first indicator </summary>
		private readonly IIndicator<Decimal> _first;
		/// <summary>
		/// The second indicator </summary>
		private readonly IIndicator<Decimal> _second;

		/// <summary>
		/// Constructor. </summary>
		/// <param name="indicator"> the indicator </param>
		/// <param name="threshold"> a threshold </param>
		public OverIndicatorRule(IIndicator<Decimal> indicator, Decimal threshold) : this(indicator, new ConstantIndicator<Decimal>(threshold))
		{
		}

		/// <summary>
		/// Constructor. </summary>
		/// <param name="first"> the first indicator </param>
		/// <param name="second"> the second indicator </param>
		public OverIndicatorRule(IIndicator<Decimal> first, IIndicator<Decimal> second)
		{
			_first = first;
			_second = second;
		}

		public override bool IsSatisfied(int index, TradingRecord tradingRecord)
		{
			var satisfied = _first.GetValue(index).IsGreaterThan(_second.GetValue(index));
			TraceIsSatisfied(index, satisfied);
			return satisfied;
		}
	}
}