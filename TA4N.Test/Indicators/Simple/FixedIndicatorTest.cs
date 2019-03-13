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
			TaTestsUtils.AssertDecimalEquals(_fixedDecimalIndicator.GetValue(0), 13.37);
			TaTestsUtils.AssertDecimalEquals(_fixedDecimalIndicator.GetValue(1), 42);
			TaTestsUtils.AssertDecimalEquals(_fixedDecimalIndicator.GetValue(2), -17);

			_fixedDecimalIndicator = new FixedDecimalIndicator("3.0", "-123.456", "0");
			TaTestsUtils.AssertDecimalEquals(_fixedDecimalIndicator.GetValue(0), "3");
			TaTestsUtils.AssertDecimalEquals(_fixedDecimalIndicator.GetValue(1), "-123.456");
			TaTestsUtils.AssertDecimalEquals(_fixedDecimalIndicator.GetValue(2), "0.0");
		}

        [Test]
		public void GetValueOnFixedBooleanIndicator()
		{
			_fixedBooleanIndicator = new FixedBooleanIndicator(false, false, true, false, true);
			Assert.IsFalse(_fixedBooleanIndicator.GetValue(0));
			Assert.IsFalse(_fixedBooleanIndicator.GetValue(1));
			Assert.IsTrue(_fixedBooleanIndicator.GetValue(2));
			Assert.IsFalse(_fixedBooleanIndicator.GetValue(3));
			Assert.IsTrue(_fixedBooleanIndicator.GetValue(4));
		}
	}
}