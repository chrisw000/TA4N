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

using NUnit.Framework;
using TA4N.Trading.Rules;

namespace TA4N.Test.Trading.Rules
{
	using FixedDecimalIndicator = TA4N.Indicators.Simple.FixedDecimalIndicator;

	public class OverIndicatorRuleTest
	{
		private IIndicator<Decimal> _indicator;
		private OverIndicatorRule _rule;

        [SetUp]
		public virtual void SetUp()
		{
			_indicator = new FixedDecimalIndicator(20, 15, 10, 5, 0, -5, -10, 100);
			_rule = new OverIndicatorRule(_indicator, Decimal.ValueOf(5));
		}

        [Test]
		public virtual void IsSatisfied()
		{
			Assert.IsTrue(_rule.IsSatisfied(0));
			Assert.IsTrue(_rule.IsSatisfied(1));
			Assert.IsTrue(_rule.IsSatisfied(2));
			Assert.IsFalse(_rule.IsSatisfied(3));
			Assert.IsFalse(_rule.IsSatisfied(4));
			Assert.IsFalse(_rule.IsSatisfied(5));
			Assert.IsFalse(_rule.IsSatisfied(6));
			Assert.IsTrue(_rule.IsSatisfied(7));
		}
	}
}