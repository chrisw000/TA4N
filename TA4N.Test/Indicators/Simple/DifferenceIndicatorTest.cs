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
	public sealed class DifferenceIndicatorTest
	{
        private ConstantIndicator<Decimal> _constantIndicator;
        private FixedIndicator<Decimal> _mockIndicator;
        private DifferenceIndicator _differenceIndicator;
        
        [SetUp]
		public void SetUp()
		{
			_constantIndicator = new ConstantIndicator<Decimal>(Decimal.ValueOf(6));
			_mockIndicator = new FixedIndicator<Decimal>(Decimal.ValueOf("-2.0"), Decimal.ValueOf("0.00"), Decimal.ValueOf("1.00"), Decimal.ValueOf("2.53"), Decimal.ValueOf("5.87"), Decimal.ValueOf("6.00"), Decimal.ValueOf("10.0"));
			_differenceIndicator = new DifferenceIndicator(_constantIndicator, _mockIndicator);
		}
        
        [Test]
		public void GetValue()
		{
			TaTestsUtils.AssertDecimalEquals(_differenceIndicator.GetValue(0), "8");
			TaTestsUtils.AssertDecimalEquals(_differenceIndicator.GetValue(1), "6");
			TaTestsUtils.AssertDecimalEquals(_differenceIndicator.GetValue(2), "5");
			TaTestsUtils.AssertDecimalEquals(_differenceIndicator.GetValue(3), "3.47");
			TaTestsUtils.AssertDecimalEquals(_differenceIndicator.GetValue(4), "0.13");
			TaTestsUtils.AssertDecimalEquals(_differenceIndicator.GetValue(5), "0");
			TaTestsUtils.AssertDecimalEquals(_differenceIndicator.GetValue(6), "-4");
		}
	}
}