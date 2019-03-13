using System;
using System.Collections.Generic;

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
	/// A history/record of a trading session.
	/// <para>
	/// Holds the full trading record when running a <seealso cref="Strategy"/>.
	/// It is used to:
	/// <ul>
	/// <li>check to satisfaction of some trading rules (when running a strategy)</li>
	/// <li>analyze the performance of a trading strategy</li>
	/// </ul>
	/// </para>
	/// </summary>
	[Serializable]
	public class TradingRecord
	{
		/// <summary>
		/// The recorded orders </summary>
		private readonly IList<Order> _orders = new List<Order>();

		/// <summary>
		/// The recorded BUY orders </summary>
		private readonly IList<Order> _buyOrders = new List<Order>();

		/// <summary>
		/// The recorded SELL orders </summary>
		private readonly IList<Order> _sellOrders = new List<Order>();

		/// <summary>
		/// The recorded entry orders </summary>
		private readonly IList<Order> _entryOrders = new List<Order>();

		/// <summary>
		/// The recorded exit orders </summary>
		private readonly IList<Order> _exitOrders = new List<Order>();

	    /// <summary>
		/// The entry type (BUY or SELL) in the trading session </summary>
		private readonly OrderType _startingType;

	    /// <summary>
		/// Constructor.
		/// </summary>
		public TradingRecord() : this(OrderType.Buy)
		{
		}

		/// <summary>
		/// Constructor. </summary>
		/// <param name="entryOrderType"> the <seealso cref="OrderType"/> of entries in the trading session </param>
		public TradingRecord(OrderType entryOrderType)
		{
		    _startingType = entryOrderType ?? throw new ArgumentNullException(nameof(entryOrderType));
			CurrentTrade = new Trade(entryOrderType);
		}

		/// <summary>
		/// Constructor. </summary>
		/// <param name="orders"> the orders to be recorded (cannot be empty) </param>
		public TradingRecord(params Order[] orders) : this(orders[0].OrderType)
		{
			foreach (var o in orders)
			{
				var newOrderWillBeAnEntry = CurrentTrade.New;
				if (newOrderWillBeAnEntry && !o.OrderType.Equals(_startingType))
				{
					// Special case for entry/exit types reversal
					// E.g.: BUY, SELL,
					//    BUY, SELL,
					//    SELL, BUY,
					//    BUY, SELL
					CurrentTrade = new Trade(o.OrderType);
				}
				var newOrder = CurrentTrade.Operate(o.Index, o.Price, o.Amount);
				RecordOrder(newOrder, newOrderWillBeAnEntry);
			}
		}

		/// <returns> the current trade </returns>
		public Trade CurrentTrade { get; private set; }

	    /// <summary>
		/// Operates an order in the trading record. </summary>
		/// <param name="index"> the index to operate the order </param>
		public void Operate(int index)
		{
			Operate(index, Decimal.NaNRenamed, Decimal.NaNRenamed);
		}

	    /// <summary>
	    /// Operates an order in the trading record. </summary>
	    /// <param name="index"> the index to operate the order </param>
	    /// <param name="price"> the price of the order </param>
	    /// <param name="amount"> the amount to be ordered </param>
	    public void Operate(int index, Decimal price, Decimal amount)
		{
			if (CurrentTrade.Closed)
			{
				// Current trade closed, should not occur
				throw new InvalidOperationException("Current trade should not be closed");
			}
			var newOrderWillBeAnEntry = CurrentTrade.New;
			var newOrder = CurrentTrade.Operate(index, price, amount);
			RecordOrder(newOrder, newOrderWillBeAnEntry);
		}

		/// <summary>
		/// Operates an entry order in the trading record. </summary>
		/// <param name="index"> the index to operate the entry </param>
		/// <returns> true if the entry has been operated, false otherwise </returns>
		public bool Enter(int index)
		{
			return Enter(index, Decimal.NaNRenamed, Decimal.NaNRenamed);
		}

	    /// <summary>
	    /// Operates an entry order in the trading record. </summary>
	    /// <param name="index"> the index to operate the entry </param>
	    /// <param name="price"> the price of the order </param>
	    /// <param name="amount"> the amount to be ordered </param>
	    /// <returns> true if the entry has been operated, false otherwise </returns>
	    public bool Enter(int index, Decimal price, Decimal amount)
		{
			if (CanEnter)
			{
				Operate(index, price, amount);
				return true;
			}
			return false;
		}

        public bool CanEnter => CurrentTrade.New;
	    public bool CanExit => CurrentTrade.Opened;

        /// <summary>
        /// Operates an exit order in the trading record. </summary>
        /// <param name="index"> the index to operate the exit </param>
        /// <returns> true if the exit has been operated, false otherwise </returns>
        public bool Exit(int index)
		{
			return Exit(index, Decimal.NaNRenamed, Decimal.NaNRenamed);
		}

		/// <summary>
		/// Operates an exit order in the trading record. </summary>
		/// <param name="index"> the index to operate the exit </param>
		/// <param name="price"> the price of the order </param>
		/// <param name="amount"> the amount to be ordered </param>
		/// <returns> true if the exit has been operated, false otherwise </returns>
		public bool Exit(int index, Decimal price, Decimal amount)
		{
			if (CanExit)
			{
				Operate(index, price, amount);
				return true;
			}
			return false;
		}

		/// <returns> true if no trade is open, false otherwise </returns>
		public bool Closed => !CurrentTrade.Opened;

	    /// <returns> the recorded trades </returns>
		public IList<Trade> Trades { get; } = new List<Trade>();

	    /// <returns> the number of recorded trades </returns>
		public int TradeCount => Trades.Count;

	    /// <returns> the last trade recorded </returns>
		public Trade LastTrade
		{
			get
			{
				if (Trades.Count > 0)
				{
					return Trades[Trades.Count - 1];
				}
				return null;
			}
		}

		/// <returns> the last order recorded </returns>
		public Order LastOrder
		{
			get
			{
				if (_orders.Count > 0)
				{
					return _orders[_orders.Count - 1];
				}
				return null;
			}
		}

		/// <param name="orderType"> the type of the order to get the last of </param>
		/// <returns> the last order (of the provided type) recorded </returns>
		public Order GetLastOrder(OrderType orderType)
		{
			if (OrderType.Buy.Equals(orderType) && _buyOrders.Count > 0)
			{
				return _buyOrders[_buyOrders.Count - 1];
			}
			else if (OrderType.Sell.Equals(orderType) && _sellOrders.Count > 0)
			{
				return _sellOrders[_sellOrders.Count - 1];
			}
			return null;
		}

		/// <returns> the last entry order recorded </returns>
		public Order LastEntry
		{
			get
			{
				if (_entryOrders.Count > 0)
				{
					return _entryOrders[_entryOrders.Count - 1];
				}
				return null;
			}
		}

		/// <returns> the last exit order recorded </returns>
		public Order LastExit
		{
			get
			{
				if (_exitOrders.Count > 0)
				{
					return _exitOrders[_exitOrders.Count - 1];
				}
				return null;
			}
		}

	    /// <summary>
		/// Records an order and the corresponding trade (if closed). </summary>
		/// <param name="order"> the order to be recorded </param>
		/// <param name="isEntry"> true if the order is an entry, false otherwise (exit) </param>
		private void RecordOrder(Order order, bool isEntry)
		{
			if (order == null)
			{
				throw new ArgumentException("Order should not be null");
			}

			// Storing the new order in entries/exits lists
			if (isEntry)
			{
				_entryOrders.Add(order);
			}
			else
			{
				_exitOrders.Add(order);
			}

			// Storing the new order in orders list
			_orders.Add(order);
			if (OrderType.Buy.Equals(order.OrderType))
			{
				// Storing the new order in buy orders list
				_buyOrders.Add(order);
			}
			else if (OrderType.Sell.Equals(order.OrderType))
			{
				// Storing the new order in sell orders list
				_sellOrders.Add(order);
			}

			// Storing the trade if closed
			if (CurrentTrade.Closed)
			{
				Trades.Add(CurrentTrade);
				CurrentTrade = new Trade(_startingType);
			}
		}
	}

}