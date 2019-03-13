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
namespace TA4N
{
	public sealed class OrderTest
	{
	    private Order _opEquals1, _opEquals2, _opNotEquals1, _opNotEquals2;

	    [SetUp]
		public void SetUp()
		{
			_opEquals1 = Order.BuyAt(1);
			_opEquals2 = Order.BuyAt(1);

			_opNotEquals1 = Order.SellAt(1);
			_opNotEquals2 = Order.BuyAt(2);
		}

        [Test]
		public void Type()
		{
            Assert.Multiple(()=>
		    {
		        Assert.AreEqual(OrderType.Sell, _opNotEquals1.OrderType);
		        Assert.IsFalse(_opNotEquals1.IsBuy);
		        Assert.IsTrue(_opNotEquals1.IsSell);
		        Assert.AreEqual(OrderType.Buy, _opNotEquals2.OrderType);
		        Assert.IsTrue(_opNotEquals2.IsBuy);
		        Assert.IsFalse(_opNotEquals2.IsSell);
		    });
		}

        [Test]
		public void OverrideToString()
		{
            Assert.AreEqual(_opEquals1.ToString(), _opEquals2.ToString());

            Assert.AreNotEqual(_opEquals1.ToString(), _opNotEquals1.ToString());
            Assert.AreNotEqual(_opEquals1.ToString(), _opNotEquals2.ToString());
		}
	}

}