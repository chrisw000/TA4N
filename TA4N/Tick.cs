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
	using NodaTime;
    
	/// <summary>
	/// End tick of a time period.
	/// <para>
	/// </para>
	/// </summary>
	[Serializable]
	public class Tick
	{
		/// <summary>
		/// Time period (e.g. 1 day, 15 min, etc.) of the tick </summary>
		private readonly Period _timePeriod;
		/// <summary>
		/// End time of the tick </summary>
		private LocalDateTime _endTime;
		/// <summary>
		/// Begin time of the tick </summary>
		private readonly LocalDateTime _beginTime;
		/// <summary>
		/// Open price of the period </summary>
		private Decimal _openPrice;
		/// <summary>
		/// Close price of the period </summary>
		private Decimal _closePrice;
		/// <summary>
		/// Max price of the period </summary>
		private Decimal _maxPrice;
		/// <summary>
		/// Min price of the period </summary>
		private Decimal _minPrice;
		/// <summary>
		/// Traded amount during the period </summary>
		private Decimal _amount = Decimal.Zero;
		/// <summary>
		/// Volume of the period </summary>
		private Decimal _volume = Decimal.Zero;
		/// <summary>
		/// Trade count </summary>
		private int _trades;

		/// <summary>
		/// Constructor. </summary>
		/// <param name="timePeriod"> the time period </param>
		/// <param name="endTime"> the end time of the tick period </param>
		public Tick(Period timePeriod, LocalDateTime endTime)
		{
			CheckTimeArguments(timePeriod, endTime);
			_timePeriod = timePeriod;
			_endTime = endTime;
			_beginTime = endTime.Minus(timePeriod);
		}

		/// <summary>
		/// Constructor. </summary>
		/// <param name="endTime"> the end time of the tick period </param>
		/// <param name="openPrice"> the open price of the tick period </param>
		/// <param name="highPrice"> the highest price of the tick period </param>
		/// <param name="lowPrice"> the lowest price of the tick period </param>
		/// <param name="closePrice"> the close price of the tick period </param>
		/// <param name="volume"> the volume of the tick period </param>
		public Tick(LocalDateTime endTime, double openPrice, double highPrice, double lowPrice, double closePrice, double volume) : this(endTime, Decimal.ValueOf(openPrice), Decimal.ValueOf(highPrice), Decimal.ValueOf(lowPrice), Decimal.ValueOf(closePrice), Decimal.ValueOf(volume))
		{
		}

		/// <summary>
		/// Constructor. </summary>
		/// <param name="endTime"> the end time of the tick period </param>
		/// <param name="openPrice"> the open price of the tick period </param>
		/// <param name="highPrice"> the highest price of the tick period </param>
		/// <param name="lowPrice"> the lowest price of the tick period </param>
		/// <param name="closePrice"> the close price of the tick period </param>
		/// <param name="volume"> the volume of the tick period </param>
		public Tick(LocalDateTime endTime, string openPrice, string highPrice, string lowPrice, string closePrice, string volume) : this(endTime, Decimal.ValueOf(openPrice), Decimal.ValueOf(highPrice), Decimal.ValueOf(lowPrice), Decimal.ValueOf(closePrice), Decimal.ValueOf(volume))
		{
		}

		/// <summary>
		/// Constructor. </summary>
		/// <param name="endTime"> the end time of the tick period </param>
		/// <param name="openPrice"> the open price of the tick period </param>
		/// <param name="highPrice"> the highest price of the tick period </param>
		/// <param name="lowPrice"> the lowest price of the tick period </param>
		/// <param name="closePrice"> the close price of the tick period </param>
		/// <param name="volume"> the volume of the tick period </param>
		public Tick(LocalDateTime endTime, Decimal openPrice, Decimal highPrice, Decimal lowPrice, Decimal closePrice, Decimal volume) : 
            this(Period.FromDays(1), endTime, openPrice, highPrice, lowPrice, closePrice, volume)
		{
		}

		/// <summary>
		/// Constructor. </summary>
		/// <param name="timePeriod"> the time period </param>
		/// <param name="endTime"> the end time of the tick period </param>
		/// <param name="openPrice"> the open price of the tick period </param>
		/// <param name="highPrice"> the highest price of the tick period </param>
		/// <param name="lowPrice"> the lowest price of the tick period </param>
		/// <param name="closePrice"> the close price of the tick period </param>
		/// <param name="volume"> the volume of the tick period </param>
		public Tick(Period timePeriod, LocalDateTime endTime, Decimal openPrice, Decimal highPrice, Decimal lowPrice, Decimal closePrice, Decimal volume)
		{
			CheckTimeArguments(timePeriod, endTime);
			_timePeriod = timePeriod;
			_endTime = endTime;
			_beginTime = endTime.Minus(timePeriod);
			_openPrice = openPrice;
			_maxPrice = highPrice;
			_minPrice = lowPrice;
			_closePrice = closePrice;
			_volume = volume;
		}

		/// <returns> the close price of the period </returns>
		public virtual Decimal ClosePrice => _closePrice;

	    /// <returns> the open price of the period </returns>
		public virtual Decimal OpenPrice => _openPrice;

	    /// <returns> the number of trades in the period </returns>
		public virtual int Trades => _trades;

	    /// <returns> the max price of the period </returns>
		public virtual Decimal MaxPrice => _maxPrice;

	    /// <returns> the whole traded amount of the period </returns>
		public virtual Decimal Amount => _amount;

	    /// <returns> the whole traded volume in the period </returns>
		public virtual Decimal Volume => _volume;

	    /// <summary>
		/// Adds a trade at the end of tick period. </summary>
		/// <param name="tradeAmount"> the tradable amount </param>
		/// <param name="tradePrice"> the price </param>
		public virtual void AddTrade(double tradeAmount, double tradePrice, bool volumeIsSizeOnly = false)
		{
			AddTrade(Decimal.ValueOf(tradeAmount), Decimal.ValueOf(tradePrice), volumeIsSizeOnly);
		}

		/// <summary>
		/// Adds a trade at the end of tick period. </summary>
		/// <param name="tradeAmount"> the tradable amount </param>
		/// <param name="tradePrice"> the price </param>
		public virtual void AddTrade(string tradeAmount, string tradePrice, bool volumeIsSizeOnly = false)
		{
			AddTrade(Decimal.ValueOf(tradeAmount), Decimal.ValueOf(tradePrice), volumeIsSizeOnly);
		}

		/// <summary>
		/// Adds a trade at the end of tick period. </summary>
		/// <param name="tradeAmount"> the tradable amount </param>
		/// <param name="tradePrice"> the price </param>
		public virtual void AddTrade(Decimal tradeAmount, Decimal tradePrice, bool volumeIsSizeOnly = false)
		{
			if (_openPrice == null)
			{
				_openPrice = tradePrice;
			}
			_closePrice = tradePrice;

			if (_maxPrice == null)
			{
				_maxPrice = tradePrice;
			}
			else
			{
				_maxPrice = _maxPrice.IsLessThan(tradePrice) ? tradePrice : _maxPrice;
			}
			if (_minPrice == null)
			{
				_minPrice = tradePrice;
			}
			else
			{
				_minPrice = _minPrice.IsGreaterThan(tradePrice) ? tradePrice : _minPrice;
			}
			_amount = _amount.Plus(tradeAmount);
		    if (volumeIsSizeOnly)
		    {
		        _volume = _volume.Plus(tradeAmount);
		    }
		    else
		    {
		        _volume = _volume.Plus(tradeAmount.MultipliedBy(tradePrice));
		    }
			_trades++;
		}

		/// <returns> the min price of the period </returns>
		public virtual Decimal MinPrice => _minPrice;

	    /// <returns> the time period of the tick </returns>
		public virtual Period TimePeriod => _timePeriod;

	    /// <returns> the begin timestamp of the tick period </returns>
		public virtual LocalDateTime BeginTime => _beginTime;

	    /// <returns> the end timestamp of the tick period </returns>
		public virtual LocalDateTime EndTime => _endTime;

	    public override string ToString()
		{
			return $"[time: {_endTime:dd/MM/yyyy HH:mm:ss}, close price: {_closePrice}]";
		}

		/// <param name="timestamp"> a timestamp </param>
		/// <returns> true if the provided timestamp is between the begin time and the end time of the current period, false otherwise </returns>
		public virtual bool InPeriod(LocalDateTime? timestamp)
		{
            return timestamp != null && timestamp >= _beginTime && timestamp < _endTime;
		}

		/// <returns> true if this is a bearish tick, false otherwise </returns>
		public virtual bool Bearish => (_openPrice != null) && (_closePrice != null) && _closePrice.IsLessThan(_openPrice);

	    /// <returns> true if this is a bullish tick, false otherwise </returns>
		public virtual bool Bullish => (_openPrice != null) && (_closePrice != null) && _openPrice.IsLessThan(_closePrice);

	    /// <returns> a human-friendly string of the end timestamp </returns>
		public virtual string DateName => _endTime.ToString("HH:mm dd/MM/yyyy", null);

	    /// <returns> a even more human-friendly string of the end timestamp </returns>
		public virtual string SimpleDateName => _endTime.ToString("dd/MM/yyyy", null);

	    /// <param name="timePeriod">the time period </param>
        /// <param name="endTime">the end time of the tick </param>
        /// <exception cref="ArgumentNullException"> if one of the arguments is null </exception>
        private void CheckTimeArguments(Period timePeriod, LocalDateTime endTime)
		{
			if (timePeriod == null) throw new ArgumentNullException(nameof(timePeriod));
			if (endTime == null) throw new ArgumentNullException(nameof(endTime));
		}
	}

}