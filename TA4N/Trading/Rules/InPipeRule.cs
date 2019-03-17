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
	using Indicators.Simple;

	/// <summary>
	/// Indicator-between-indicators rule.
	/// <para>
	/// Satisfied when the value of the <seealso cref="IIndicator{T}"/> is between the values of the boundary (up/down) Indicators.
	/// </para>
	/// </summary>
	public class InPipeRule : AbstractRule
	{
		/// <summary>
		/// The upper indicator </summary>
		private readonly IIndicator<Decimal> _upper;
		/// <summary>
		/// The lower indicator </summary>
		private readonly IIndicator<Decimal> _lower;
		/// <summary>
		/// The evaluated indicator </summary>
		private readonly IIndicator<Decimal> _ref;

		/// <summary>
		/// Constructor. </summary>
		/// <param name="ref"> the reference indicator </param>
		/// <param name="upper"> the upper threshold </param>
		/// <param name="lower"> the lower threshold </param>
		public InPipeRule(IIndicator<Decimal> @ref, Decimal upper, Decimal lower) : this(@ref, new ConstantIndicator<Decimal>(upper), new ConstantIndicator<Decimal>(lower))
		{
		}

		/// <summary>
		/// Constructor. </summary>
		/// <param name="ref"> the reference indicator </param>
		/// <param name="upper"> the upper indicator </param>
		/// <param name="lower"> the lower indicator </param>
		public InPipeRule(IIndicator<Decimal> @ref, IIndicator<Decimal> upper, IIndicator<Decimal> lower)
		{
			_upper = upper;
			_lower = lower;
			_ref = @ref;

            logger = LogWrapper.Factory?.CreateLogger<InPipeRule>();
		}

		public override bool IsSatisfied(int index, TradingRecord tradingRecord)
		{
			var satisfied = _ref.GetValue(index).IsLessThanOrEqual(_upper.GetValue(index)) && _ref.GetValue(index).IsGreaterThanOrEqual(_lower.GetValue(index));
			TraceIsSatisfied(index, satisfied);
			return satisfied;
		}
	}
}