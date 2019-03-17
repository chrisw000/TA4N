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
	/// An OR combination of two <seealso cref="IRule"/>.
	/// <para>
	/// Satisfied when one of the two provided rules is satisfied.<br/>
	/// Warning: the second rule is not tested if the first rule is satisfied.
	/// </para>
	/// </summary>
	public class OrRule : AbstractRule
	{
		private readonly IRule _rule1;
		private readonly IRule _rule2;

		/// <summary>
		/// Constructor. </summary>
		/// <param name="rule1"> a trading rule </param>
		/// <param name="rule2"> another trading rule </param>
		public OrRule(IRule rule1, IRule rule2)
		{
			_rule1 = rule1;
			_rule2 = rule2;
            logger = LogWrapper.Factory?.CreateLogger<OrRule>();
		}

		public override bool IsSatisfied(int index, TradingRecord tradingRecord)
		{
			var satisfied = _rule1.IsSatisfied(index, tradingRecord) || _rule2.IsSatisfied(index, tradingRecord);
			TraceIsSatisfied(index, satisfied);
			return satisfied;
		}
	}
}