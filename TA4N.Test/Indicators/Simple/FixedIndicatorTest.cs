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
    public sealed class FixedIndicatorTest
    {
        private FixedDecimalIndicator _fixedDecimalIndicator;
        private FixedBooleanIndicator _fixedBooleanIndicator;

        [Test]
        public void GetValueOnFixedDecimalIndicator()
        {
            _fixedDecimalIndicator = new FixedDecimalIndicator(13.37, 42, -17);
            Assert.That(_fixedDecimalIndicator.GetValue(0).ToDouble(), Is.EqualTo(13.37).Within(TaTestsUtils.TaOffset));
            Assert.That(_fixedDecimalIndicator.GetValue(1), Is.EqualTo(Decimal.ValueOf(42)));
            Assert.That(_fixedDecimalIndicator.GetValue(2).ToDouble(), Is.EqualTo(-17).Within(TaTestsUtils.TaOffset));
            _fixedDecimalIndicator = new FixedDecimalIndicator("3.0", "-123.456", "0");
            Assert.That(_fixedDecimalIndicator.GetValue(0), Is.EqualTo(Decimal.ValueOf("3")));
            Assert.That(_fixedDecimalIndicator.GetValue(1), Is.EqualTo(Decimal.ValueOf("-123.456")));
            Assert.That(_fixedDecimalIndicator.GetValue(2), Is.EqualTo(Decimal.ValueOf("0.0")));
        }

        [Test]
        public void GetValueOnFixedBooleanIndicator()
        {
            _fixedBooleanIndicator = new FixedBooleanIndicator(false, false, true, false, true);
            Assert.That(_fixedBooleanIndicator.GetValue(0), Is.False);
            Assert.That(_fixedBooleanIndicator.GetValue(1), Is.False);
            Assert.That(_fixedBooleanIndicator.GetValue(2), Is.True);
            Assert.That(_fixedBooleanIndicator.GetValue(3), Is.False);
            Assert.That(_fixedBooleanIndicator.GetValue(4), Is.True);
        }
    }
}