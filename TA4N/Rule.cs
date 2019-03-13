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
	/// A rule for strategy building.
	/// <para>
	/// A trading rule may be composed of a combination of other rules.
	/// 
	/// A <seealso cref="Strategy"/> is a pair of complementary (entry and exit) rules.
	/// </para>
	/// </summary>
	public interface IRule
	{
		/// <param name="rule"> another trading rule </param>
		/// <returns> a rule which is the AND combination of this rule with the provided one </returns>
		IRule And(IRule rule);

		/// <param name="rule"> another trading rule </param>
		/// <returns> a rule which is the OR combination of this rule with the provided one </returns>
		IRule Or(IRule rule);

		/// <param name="rule"> another trading rule </param>
		/// <returns> a rule which is the XOR combination of this rule with the provided one </returns>
		IRule Xor(IRule rule);

		/// <returns> a rule which is the logical negation of this rule </returns>
		IRule Negation();

		/// <param name="index"> the tick index </param>
		/// <returns> true if this rule is satisfied for the provided index, false otherwise </returns>
		bool IsSatisfied(int index);

		/// <param name="index"> the tick index </param>
		/// <param name="tradingRecord"> the potentially needed trading history </param>
		/// <returns> true if this rule is satisfied for the provided index, false otherwise </returns>
		bool IsSatisfied(int index, TradingRecord tradingRecord);
	}

}