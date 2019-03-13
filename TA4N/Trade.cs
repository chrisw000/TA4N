using System;

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
	/// <summary>
	/// Pair of two <seealso cref="Order"/>.
	/// <para>
	/// The exit order has the complement type of the entry order.<br/>
	/// I.e.:
	///   entry == BUY --> exit == SELL
	///   entry == SELL --> exit == BUY
	/// </para>
	/// </summary>
	[Serializable]
	public sealed class Trade
	{
		/// <summary>
		/// The entry order </summary>
		private Order _entry;

		/// <summary>
		/// The exit order </summary>
		private Order _exit;

		/// <summary>
		/// The type of the entry order </summary>
		private readonly OrderType _startingType;

		/// <summary>
		/// Constructor.
		/// </summary>
		public Trade() : this(OrderType.Buy)
		{
		}

		/// <summary>
		/// Constructor. </summary>
		/// <param name="startingType"> the starting <seealso cref="OrderType"/> of the trade (i.e. type of the entry order) </param>
		public Trade(OrderType startingType)
		{
            _startingType = startingType ?? throw new ArgumentNullException(nameof(startingType));
		}

		/// <summary>
		/// Constructor. </summary>
		/// <param name="entry"> the entry <seealso cref="Order"/> </param>
		/// <param name="exit"> the exit <seealso cref="Order"/> </param>
		public Trade(Order entry, Order exit)
		{
			if (entry.OrderType.Equals(exit.OrderType))
			{
				throw new ArgumentException("Both orders must have different OrderTypes");
			}
			_startingType = entry.OrderType;
			_entry = entry;
			_exit = exit;
		}

		/// <returns> the entry <seealso cref="Order"/> of the trade </returns>
		public Order Entry => _entry;

	    /// <returns> the exit <seealso cref="Order"/> of the trade </returns>
		public Order Exit => _exit;

	    public override bool Equals(object obj)
		{
		    var t = obj as Trade;
		    if (t == null) return false;

    		return _entry.Equals(t.Entry) && _exit.Equals(t.Exit);
		}

		public override int GetHashCode()
		{
			return (_entry.GetHashCode() * 31) + (_exit.GetHashCode() * 17);
		}

		/// <summary>
		/// Operates the trade at the index-th position </summary>
		/// <param name="index"> the tick index </param>
		/// <returns> the order </returns>
		public Order Operate(int index)
		{
			return Operate(index, Decimal.NaNRenamed, Decimal.NaNRenamed);
		}

		/// <summary>
		/// Operates the trade at the index-th position </summary>
		/// <param name="index"> the tick index </param>
		/// <param name="price"> the price </param>
		/// <param name="amount"> the amount </param>
		/// <returns> the order </returns>
		public Order Operate(int index, Decimal price, Decimal amount)
		{
			Order order = null;
			if (New)
			{
				order = new Order(index, _startingType, price, amount);
				_entry = order;
			}
			else if (Opened)
			{
				if (index < _entry.Index)
				{
					throw new InvalidOperationException("The index i is less than the entryOrder index");
				}
				order = new Order(index, _startingType.ComplementType, price, amount);
				_exit = order;
			}
			return order;
		}

		/// <returns> true if the trade is closed, false otherwise </returns>
		public bool Closed => (_entry != null) && (_exit != null);

	    /// <returns> true if the trade is opened, false otherwise </returns>
		public bool Opened => (_entry != null) && (_exit == null);

	    /// <returns> true if the trade is new, false otherwise </returns>
		public bool New => (_entry == null) && (_exit == null);

	    public override string ToString()
		{
			return $"Entry: {_entry} exit: {_exit}";
		}
	}

}