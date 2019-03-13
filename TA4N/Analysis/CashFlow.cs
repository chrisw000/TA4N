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

using System;
using System.Collections.Generic;
using System.Linq;

namespace TA4N.Analysis
{
	/// <summary>
	/// The cash flow.
	/// <para>
	/// This class allows to follow the money cash flow involved by a list of trades over a time series.
	/// </para>
	/// </summary>
	public class CashFlow : IIndicator<Decimal>
	{
	    /// <summary>
		/// The cash flow values </summary>
		private readonly List<Decimal> _values = new List<Decimal>() { Decimal.One};

		/// <summary>
		/// Constructor. </summary>
		/// <param name="timeSeries"> the time series </param>
		/// <param name="trade"> a single trade </param>
		public CashFlow(TimeSeries timeSeries, Trade trade)
		{
			TimeSeries = timeSeries;
			Calculate(trade);
			FillToTheEnd();
		}

		/// <summary>
		/// Constructor. </summary>
		/// <param name="timeSeries"> the time series </param>
		/// <param name="tradingRecord"> the trading record </param>
		public CashFlow(TimeSeries timeSeries, TradingRecord tradingRecord)
		{
			TimeSeries = timeSeries;
			Calculate(tradingRecord);
			FillToTheEnd();
		}

		/// <param name="index"> the tick index </param>
		/// <returns> the cash flow value at the index-th position </returns>
		public Decimal GetValue(int index)
		{
            if(index<0) return Decimal.NaNRenamed;

		    return index >= _values.Count 
                ? _values[_values.Count - 1]  // Happens when time passes with no other trades being added... could do with an Add Trade method... currently need to remake the whole indicator
                : _values[index];
		}

		public TimeSeries TimeSeries { get; }

	    /// <returns> the size of the time series </returns>
		public virtual int Size => TimeSeries.TickCount;

	    /// <summary>
		/// Calculates the cash flow for a single trade. </summary>
		/// <param name="trade"> a single trade </param>
		private void Calculate(Trade trade)
		{
			var entryIndex = trade.Entry.Index;
			var begin = entryIndex + 1;
			if (begin > _values.Count)
			{
				var lastValue = _values[_values.Count - 1];
                _values.AddRange(Enumerable.Repeat(lastValue, begin - _values.Count).ToList());
			}
			var end = trade.Exit.Index;
			for (var i = Math.Max(begin, 1); i <= end; i++)
			{
				Decimal ratio;
				if (trade.Entry.IsBuy)
				{
				    ratio = TimeSeries.GetTick(i).ClosePrice.DividedBy(TimeSeries.GetTick(entryIndex).ClosePrice);
				}
				else
				{
				    ratio = TimeSeries.GetTick(entryIndex).ClosePrice.DividedBy(TimeSeries.GetTick(i).ClosePrice);      
                }
                _values.Add(_values[entryIndex].MultipliedBy(ratio));
			}
		}

        /// <summary>
        /// I've added this and CashFlowTest.DynamicAddTrade to ensure it acts the same as supplying a new series/trade record
        /// This allows adding dynamic trades as we go and therefore slightly faster cashflow updates.
        /// </summary>
        /// <param name="trade"></param>
	    public void AddTrade(Trade trade)
	    {
	        if (trade == null) return;
	        Calculate(trade);
            FillToTheEnd();
        }
            

		/// <summary>
		/// Calculates the cash flow for a trading record. </summary>
		/// <param name="tradingRecord"> the trading record </param>
		private void Calculate(TradingRecord tradingRecord)
		{
			foreach (var trade in tradingRecord.Trades)
			{
				// For each trade...
				Calculate(trade);
			}
		}

		/// <summary>
		/// Fills with last value till the end of the series.
		/// </summary>
		private void FillToTheEnd()
		{
			if (TimeSeries.End >= _values.Count)
			{
				var lastValue = _values[_values.Count - 1];
                _values.AddRange(Enumerable.Repeat(lastValue, TimeSeries.End - _values.Count + 1).ToList());
			}
		}
	}
}