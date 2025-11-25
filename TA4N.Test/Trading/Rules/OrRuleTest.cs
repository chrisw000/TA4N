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
using TA4N.Trading.Rules;

namespace TA4N.Test.Trading.Rules
{
    public sealed class OrRuleTest
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
            Assert.That(_satisfiedRule.Or(BooleanRule.False).IsSatisfied(0), Is.True);
            Assert.That(BooleanRule.False.Or(_satisfiedRule).IsSatisfied(0), Is.True);
            Assert.That(_unsatisfiedRule.Or(BooleanRule.False).IsSatisfied(0), Is.False);
            Assert.That(BooleanRule.False.Or(_unsatisfiedRule).IsSatisfied(0), Is.False);
            Assert.That(_satisfiedRule.Or(BooleanRule.True).IsSatisfied(10), Is.True);
            Assert.That(BooleanRule.True.Or(_satisfiedRule).IsSatisfied(10), Is.True);
            Assert.That(_unsatisfiedRule.Or(BooleanRule.True).IsSatisfied(10), Is.True);
            Assert.That(BooleanRule.True.Or(_unsatisfiedRule).IsSatisfied(10), Is.True);
        }
    }
}