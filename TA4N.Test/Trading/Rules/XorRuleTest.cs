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

using NUnit.Framework;

namespace TA4N.Trading.Rules
{
	public sealed class XorRuleTest
	{
		private IRule _satisfiedRule;
		private IRule _unsatisfiedRule;

        [SetUp]
		public void SetUp()
		{
			_satisfiedRule = new BooleanRule(true);
			_unsatisfiedRule = new BooleanRule(false);
		}

        [Test]
		public void IsSatisfied()
		{
			Assert.IsTrue(_satisfiedRule.Xor(BooleanRule.False).IsSatisfied(0));
			Assert.IsTrue(BooleanRule.False.Xor(_satisfiedRule).IsSatisfied(0));
			Assert.IsFalse(_unsatisfiedRule.Xor(BooleanRule.False).IsSatisfied(0));
			Assert.IsFalse(BooleanRule.False.Xor(_unsatisfiedRule).IsSatisfied(0));

			Assert.IsFalse(_satisfiedRule.Xor(BooleanRule.True).IsSatisfied(10));
			Assert.IsFalse(BooleanRule.True.Xor(_satisfiedRule).IsSatisfied(10));
			Assert.IsTrue(_unsatisfiedRule.Xor(BooleanRule.True).IsSatisfied(10));
			Assert.IsTrue(BooleanRule.True.Xor(_unsatisfiedRule).IsSatisfied(10));
		}
	}

}