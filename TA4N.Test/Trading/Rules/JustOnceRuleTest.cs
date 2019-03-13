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

namespace TA4N.Trading.Rules
{
    public sealed class JustOnceRuleTest
	{
		private JustOnceRule _rule;

        [SetUp]
		public void SetUp()
		{
			_rule = new JustOnceRule();
		}

        [Test]
		public void IsSatisfied()
		{
			Assert.IsTrue(_rule.IsSatisfied(10));
			Assert.IsFalse(_rule.IsSatisfied(11));
			Assert.IsFalse(_rule.IsSatisfied(12));
			Assert.IsFalse(_rule.IsSatisfied(13));
			Assert.IsFalse(_rule.IsSatisfied(14));
		}

        [Test]
		public void IsSatisfiedInReverseOrder()
		{
			Assert.IsTrue(_rule.IsSatisfied(5));
			Assert.IsFalse(_rule.IsSatisfied(2));
			Assert.IsFalse(_rule.IsSatisfied(1));
			Assert.IsFalse(_rule.IsSatisfied(0));
		}
	}
}