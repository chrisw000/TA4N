using System;
using System.Collections.Generic;
using System.Linq;

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

namespace TA4N
{
    public static class Extensions
    {
        public static OrderType ComplementType(this OrderType value)
        {
            return value == OrderType.Buy
                                ? OrderType.Sell
                                : OrderType.Buy;
        }
    }

    public enum OrderType
    {
        Buy,
        Sell
    }

    /// <summary>
    /// An order.
    /// <para>
    /// The order is defined by:
    /// <ul>
    /// <li>the index (in the <seealso cref="TimeSeries"/>) it is executed</li>
    /// <li>a <seealso cref="OrderType"/> (BUY or SELL)</li>
    /// <li>a price (optional)</li>
    /// <li>an amount to be (or that was) ordered (optional)</li>
    /// </ul>
    /// A <seealso cref="Trade"/> is a pair of complementary orders.
    /// </para>
    /// </summary>
    [Serializable]
    public class Order
    {
        /// <summary>
        /// Type of the order </summary>
        private readonly OrderType _orderType;

        /// <summary>
        /// The index the order was executed </summary>
        private readonly int _index;

        /// <summary>
        /// The price for the order </summary>
        private readonly Decimal _price = Decimal.NaNRenamed;

        /// <summary>
        /// The amount to be (or that was) ordered </summary>
        private readonly Decimal _amount = Decimal.NaNRenamed;

        /// <summary>
        /// Constructor. </summary>
        /// <param name="index"> the index the order is executed </param>
        /// <param name="orderType"> the type of the order </param>
        protected internal Order(int index, OrderType orderType)
        {
            _orderType = orderType;
            _index = index;
        }

        /// <summary>
        /// Constructor. </summary>
        /// <param name="index"> the index the order is executed </param>
        /// <param name="orderType"> the type of the order </param>
        /// <param name="price"> the price for the order </param>
        /// <param name="amount"> the amount to be (or that was) ordered </param>
        protected internal Order(int index, OrderType orderType, Decimal price, Decimal amount) : this(index, orderType)
        {
            _price = price;
            _amount = amount;
        }

        /// <returns> the type of the order (BUY or SELL) </returns>
        public virtual OrderType OrderType => _orderType;

        /// <returns> true if this is a BUY order, false otherwise </returns>
        public virtual bool IsBuy => _orderType.Equals(OrderType.Buy);

        /// <returns> true if this is a SELL order, false otherwise </returns>
        public virtual bool IsSell => _orderType.Equals(OrderType.Sell);

        /// <returns> the index the order is executed </returns>
        public virtual int Index => _index;

        /// <returns> the price for the order </returns>
        public virtual Decimal Price => _price;

        /// <returns> the amount to be (or that was) ordered </returns>
        public virtual Decimal Amount => _amount;

        public override int GetHashCode()
        {
            var hash = 7;
            hash = 29*hash + _orderType.GetHashCode();
            hash = 29*hash + _index;
            hash = 29*hash + (_price != null ? _price.GetHashCode() : 0);
            hash = 29*hash + (_amount != null ? _amount.GetHashCode() : 0);
            return hash;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (GetType() != obj.GetType())
            {
                return false;
            }

            var other = obj as Order;
            if (_orderType != other?.OrderType)
            {
                return false;
            }
            if (_index != other._index)
            {
                return false;
            }
            if (_price != other._price && (_price == null || !_price.Equals(other._price)))
            {
                return false;
            }
            if (_amount != other._amount && (_amount == null || !_amount.Equals(other._amount)))
            {
                return false;
            }
            return true;
        }

        public override string ToString()
        {
            return "Order{" + "type=" + _orderType + ", index=" + _index + ", price=" + _price + ", amount=" + _amount + '}';
        }

        /// <param name="index"> the index the order is executed </param>
        /// <returns> a BUY order </returns>
        public static Order BuyAt(int index)
        {
            return new Order(index, OrderType.Buy);
        }

        /// <param name="index"> the index the order is executed </param>
        /// <param name="price"> the price for the order </param>
        /// <param name="amount"> the amount to be (or that was) bought </param>
        /// <returns> a BUY order </returns>
        public static Order BuyAt(int index, Decimal price, Decimal amount)
        {
            return new Order(index, OrderType.Buy, price, amount);
        }

        /// <param name="index"> the index the order is executed </param>
        /// <returns> a SELL order </returns>
        public static Order SellAt(int index)
        {
            return new Order(index, OrderType.Sell);
        }

        /// <param name="index"> the index the order is executed </param>
        /// <param name="price"> the price for the order </param>
        /// <param name="amount"> the amount to be (or that was) sold </param>
        /// <returns> a SELL order </returns>
        public static Order SellAt(int index, Decimal price, Decimal amount)
        {
            return new Order(index, OrderType.Sell, price, amount);
        }
    }
}