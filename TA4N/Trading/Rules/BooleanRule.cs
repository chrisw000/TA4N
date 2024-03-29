﻿/*
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
	/// A simple boolean rule.
	/// <para>
	/// Satisfied when it has been initialized with true.
	/// </para>
	/// </summary>
	public class BooleanRule : AbstractRule
	{
		/// <summary> An always-true rule </summary>
		public static readonly BooleanRule True = new BooleanRule(true);

		/// <summary> An always-false rule </summary>
		public static readonly BooleanRule False = new BooleanRule(false);

		private readonly bool _satisfied;

		/// <summary>
		/// Constructor. </summary>
		/// <param name="satisfied"> true for the rule to be always satisfied, false to be never satisfied </param>
		public BooleanRule(bool satisfied)
		{
			_satisfied = satisfied;
            logger = LogWrapper.Factory?.CreateLogger<BooleanRule>();
		}

		public override bool IsSatisfied(int index, TradingRecord tradingRecord)
		{
			TraceIsSatisfied(index, _satisfied);
			return _satisfied;
		}
	}
}