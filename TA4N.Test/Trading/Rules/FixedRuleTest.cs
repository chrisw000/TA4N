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
    public sealed class FixedRuleTest
    {
        private FixedRule _fixedRule;

        [Test]
        public void IsSatisfied()
        {
            _fixedRule = new FixedRule();
            Assert.That(_fixedRule.IsSatisfied(0), Is.False);
            Assert.That(_fixedRule.IsSatisfied(1), Is.False);
            Assert.That(_fixedRule.IsSatisfied(2), Is.False);
            Assert.That(_fixedRule.IsSatisfied(9), Is.False);
            _fixedRule = new FixedRule(1, 2, 3);
            Assert.That(_fixedRule.IsSatisfied(0), Is.False);
            Assert.That(_fixedRule.IsSatisfied(1), Is.True);
            Assert.That(_fixedRule.IsSatisfied(2), Is.True);
            Assert.That(_fixedRule.IsSatisfied(3), Is.True);
            Assert.That(_fixedRule.IsSatisfied(4), Is.False);
            Assert.That(_fixedRule.IsSatisfied(5), Is.False);
            Assert.That(_fixedRule.IsSatisfied(6), Is.False);
            Assert.That(_fixedRule.IsSatisfied(7), Is.False);
            Assert.That(_fixedRule.IsSatisfied(8), Is.False);
            Assert.That(_fixedRule.IsSatisfied(9), Is.False);
            Assert.That(_fixedRule.IsSatisfied(10), Is.False);
        }
    }
}