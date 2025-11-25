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
using TA4N.Indicators.Simple;
using NUnit.Framework;

namespace TA4N.Test.Indicators.Simple
{
    public sealed class SumIndicatorTest
    {
        private ConstantIndicator<Decimal> _constantIndicator;
        private FixedIndicator<Decimal> _mockIndicator;
        private FixedIndicator<Decimal> _mockIndicator2;
        private SumIndicator _sumIndicator;

        [SetUp]
        public void SetUp()
        {
            _constantIndicator = new ConstantIndicator<Decimal>(Decimal.ValueOf(6));
            _mockIndicator = new FixedIndicator<Decimal>(Decimal.ValueOf("-2.0"), Decimal.ValueOf("0.00"), Decimal.ValueOf("1.00"), Decimal.ValueOf("2.53"), Decimal.ValueOf("5.87"), Decimal.ValueOf("6.00"), Decimal.ValueOf("10.0"));
            _mockIndicator2 = new FixedIndicator<Decimal>(Decimal.Zero, Decimal.One, Decimal.Two, Decimal.Three, Decimal.Ten, Decimal.ValueOf("-42"), Decimal.ValueOf("-1337"));
            _sumIndicator = new SumIndicator(_constantIndicator, _mockIndicator, _mockIndicator2);
        }

        [Test]
        public void GetValue()
        {
            Assert.That(_sumIndicator.GetValue(0), Is.EqualTo(Decimal.ValueOf("4")));
            Assert.That(_sumIndicator.GetValue(1), Is.EqualTo(Decimal.ValueOf("7")));
            Assert.That(_sumIndicator.GetValue(2), Is.EqualTo(Decimal.ValueOf("9")));
            Assert.That(_sumIndicator.GetValue(3), Is.EqualTo(Decimal.ValueOf("11.53")));
            Assert.That(_sumIndicator.GetValue(4), Is.EqualTo(Decimal.ValueOf("21.87")));
            Assert.That(_sumIndicator.GetValue(5), Is.EqualTo(Decimal.ValueOf("-30")));
            Assert.That(_sumIndicator.GetValue(6), Is.EqualTo(Decimal.ValueOf("-1321")));
        }
    }
}