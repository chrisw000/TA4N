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
	/// <summary>
	/// A boolean-indicator-based rule.
	/// <para>
	/// Satisfied when the value of the <seealso cref="IIndicator{T}"/> is true.
	/// </para>
	/// </summary>
	public class BooleanIndicatorRule : AbstractRule
	{
		private readonly IIndicator<bool> _indicator;

		/// <summary>
		/// Constructor. </summary>
		/// <param name="indicator"> a boolean indicator </param>
		public BooleanIndicatorRule(IIndicator<bool> indicator)
		{
			_indicator = indicator;
		}

		public override bool IsSatisfied(int index, TradingRecord tradingRecord)
		{
			var satisfied = _indicator.GetValue(index);
			TraceIsSatisfied(index, satisfied);
			return satisfied;
		}
	}
}