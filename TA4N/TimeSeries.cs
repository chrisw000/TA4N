using System;
using System.Collections.Generic;
using NodaTime;
using NLog;

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
	//using DateTime = org.joda.time.DateTime;
	//using Interval = org.joda.time.Interval;
	//using Period = org.joda.time.Period;

	/// <summary>
	/// Sequence of <seealso cref="Tick"/> separated by a predefined period (e.g. 15 minutes, 1 day, etc.)
	/// <para>
	/// Notably, a <seealso cref="TimeSeries"/> can be:
	/// <ul>
	/// <li>splitted into sub-series</li>
	/// <li>the base of <seealso cref="IIndicator{T}"/> calculations</li>
	/// <li>limited to a fixed number of ticks (e.g. for actual trading)</li>
	/// <li>used to run <seealso cref="Strategy"/></li>
	/// </ul>
	/// </para>
	/// </summary>
	[Serializable]
	public class TimeSeries
	{
        /// <summary>
        /// The logger </summary>
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Name of the series </summary>
        private readonly string _name;
		/// <summary>
		/// Begin index of the time series </summary>
		private int _beginIndex = -1;
		/// <summary>
		/// End index of the time series </summary>
		private int _endIndex = -1;
		/// <summary>
		/// List of ticks </summary>
		private readonly IList<Tick> _ticks;
		/// <summary>
		/// Maximum number of ticks for the time series </summary>
		private int _maximumTickCount = int.MaxValue;
		/// <summary>
		/// Number of removed ticks </summary>
		private int _removedTicksCount;
		/// <summary>
		/// True if the current series is a sub-series, false otherwise </summary>
		private readonly bool _subSeries;

		/// <summary>
		/// Constructor. </summary>
		/// <param name="name"> the name of the series </param>
		/// <param name="ticks"> the list of ticks of the series </param>
		public TimeSeries(string name, IList<Tick> ticks) : this(name, ticks, 0, ticks.Count - 1, false)
		{
		}

		/// <summary>
		/// Constructor of an unnamed series. </summary>
		/// <param name="ticks"> the list of ticks of the series </param>
		public TimeSeries(IList<Tick> ticks) : this("unnamed", ticks)
		{
		}

		/// <summary>
		/// Constructor. </summary>
		/// <param name="name"> the name of the series </param>
		public TimeSeries(string name)
		{
			_name = name;
			_ticks = new List<Tick>();
		}

		/// <summary>
		/// Constructor of an unnamed series.
		/// </summary>
		public TimeSeries() : this("unamed")
		{
		}

		/// <summary>
		/// Constructor. </summary>
		/// <param name="name"> the name of the series </param>
		/// <param name="ticks"> the list of ticks of the series </param>
		/// <param name="beginIndex"> the begin index (inclusive) of the time series </param>
		/// <param name="endIndex"> the end index (inclusive) of the time series </param>
		/// <param name="subSeries"> true if the current series is a sub-series, false otherwise </param>
		private TimeSeries(string name, IList<Tick> ticks, int beginIndex, int endIndex, bool subSeries)
		{
			// TODO: add null checks and out of bounds checks
			if (endIndex < beginIndex - 1)
			{
				throw new IndexOutOfRangeException();
			}
			_name = name;
			_ticks = ticks;
			_beginIndex = beginIndex;
			_endIndex = endIndex;
			_subSeries = subSeries;
		}

		/// <returns> the name of the series </returns>
		public virtual string Name => _name;

	    /// <param name="i"> an index </param>
		/// <returns> the tick at the i-th position </returns>
		public virtual Tick GetTick(int i)
		{
			var innerIndex = i - _removedTicksCount;
			if (innerIndex < 0)
			{
				if (i < 0)
				{
					// Cannot return the i-th tick if i < 0
					throw new IndexOutOfRangeException(BuildOutOfBoundsMessage(this, i));
				}
				Log.Trace("Time series: {0} ({1} ticks): tick {2} already removed, use {3}-th instead", _name, _ticks.Count, i, _removedTicksCount);
				if (_ticks.Count == 0)
				{
					throw new IndexOutOfRangeException(BuildOutOfBoundsMessage(this, _removedTicksCount));
				}
				innerIndex = 0;
			}
			else if (innerIndex >= _ticks.Count)
			{
				// Cannot return the n-th tick if n >= ticks.size()
				throw new IndexOutOfRangeException(BuildOutOfBoundsMessage(this, i));
			}
			return _ticks[innerIndex];
		}

		/// <returns> the first tick of the series </returns>
		public virtual Tick FirstTick => GetTick(_beginIndex);

	    /// <returns> the last tick of the series </returns>
		public virtual Tick LastTick => GetTick(_endIndex);

	    /// <returns> the number of ticks in the series </returns>
		public virtual int TickCount
		{
			get
			{
				if (_endIndex < 0)
				{
					return 0;
				}

				var startIndex = Math.Max(_removedTicksCount, _beginIndex);
				return _endIndex - startIndex + 1;
			}
		}

		/// <returns> the begin index of the series </returns>
		public virtual int Begin => _beginIndex;

	    /// <returns> the end index of the series </returns>
		public virtual int End => _endIndex;

	    /// <returns> the description of the series period (e.g. "from 12:00 21/01/2014 to 12:15 21/01/2014") </returns>
		public virtual string SeriesPeriodDescription
		{
			get
			{
			    if (_ticks.Count <= 0) return string.Empty;

			    const string timeFormat = "HH:mm dd/MM/yyyy";

			    var firstTick = FirstTick;
			    var lastTick = LastTick;

			    return $"{firstTick.EndTime.ToString(timeFormat, null)} - {lastTick.EndTime.ToString(timeFormat, null)}";
			}
		}

		/// <summary>
		/// Sets the maximum number of ticks that will be retained in the series.
		/// <para>
		/// If a new tick is added to the series such that the number of ticks will exceed the maximum tick count,
		/// then the FIRST tick in the series is automatically removed, ensuring that the maximum tick count is not exceeded.
		/// </para>
		/// </summary>
		public virtual int MaximumTickCount
		{
            get
            {
                return _maximumTickCount;
            }
            set
			{
				if (_subSeries)
				{
					throw new InvalidOperationException("Cannot set a maximum tick count on a sub-series");
				}
				if (value <= 0)
				{
					throw new ArgumentException("Maximum tick count must be strictly positive");
				}
				_maximumTickCount = value;
				RemoveExceedingTicks();
			}
		}


		/// <returns> the number of removed ticks </returns>
		public virtual int RemovedTicksCount => _removedTicksCount;

        /// <summary>
        /// Adds a tick at the end of the series.
        /// <para>
        /// Begin index set to 0 if if wasn't initialized.<br/>
        /// End index set to 0 if if wasn't initialized, or incremented if it matches the end of the series.<br/>
        /// Exceeding ticks are removed.
        /// </para>
        /// </summary>
        /// <param name="tick"> the tick to be added </param>
        public virtual void AddTick(Tick tick)
		{
			if (tick == null) throw new ArgumentNullException(nameof(tick));

			var lastTickIndex = _ticks.Count - 1;
			if (_ticks.Count > 0)
			{
				if (tick.EndTime <= _ticks[lastTickIndex].EndTime)
				{
					throw new ArgumentException("Cannot add a tick with end time <= to series end time");
				}
			}

			_ticks.Add(tick);
			if (_beginIndex == -1)
			{
				// Begin index set to 0 only if if wasn't initialized
				_beginIndex = 0;
			}
			_endIndex++;
			RemoveExceedingTicks();
		}

		/// <summary>
		/// Returns a new time series which is a view of a subset of the current series.
		/// <para>
		/// The new series has begin and end indexes which correspond to the bounds of the sub-set into the full series.<br/>
		/// The tick of the series are shared between the original time series and the returned one (i.e. no copy).
		/// </para>
		/// </summary>
		/// <param name="beginIndex"> the begin index (inclusive) of the time series </param>
		/// <param name="endIndex"> the end index (inclusive) of the time series </param>
		/// <returns> a constrained <seealso cref="TimeSeries"/> which is a sub-set of the current series </returns>
		public virtual TimeSeries Subseries(int beginIndex, int endIndex)
		{
			if (_maximumTickCount != int.MaxValue)
			{
				throw new InvalidOperationException("Cannot create a sub-series from a time series for which a maximum tick count has been set");
			}
			return new TimeSeries(_name, _ticks, beginIndex, endIndex, true);
		}

		/// <summary>
		/// Returns a new time series which is a view of a subset of the current series.
		/// <para>
		/// The new series has begin and end indexes which correspond to the bounds of the sub-set into the full series.<br/>
		/// The tick of the series are shared between the original time series and the returned one (i.e. no copy).
		/// </para>
		/// </summary>
		/// <param name="beginIndex"> the begin index (inclusive) of the time series </param>
		/// <param name="duration"> the duration of the time series </param>
		/// <returns> a constrained <seealso cref="TimeSeries"/> which is a sub-set of the current series </returns>
		public virtual TimeSeries Subseries(int beginIndex, Period duration)
		{
            // Calculating the sub-series interval
            var tickEndTime = GetTick(beginIndex).EndTime;
            var beginInterval = Instant.FromDateTimeUtc(tickEndTime.InUtc().ToDateTimeUtc());
            var endInterval = Instant.FromDateTimeUtc(tickEndTime.Plus(duration).InUtc().ToDateTimeUtc());

			var subseriesInterval = new Interval(beginInterval, endInterval);

			// Checking ticks belonging to the sub-series (starting at the provided index)
			var subseriesNbTicks = 0;
			for (var i = beginIndex; i <= _endIndex; i++)
			{
                // For each tick...
                var tickTime = Instant.FromDateTimeUtc(GetTick(i).EndTime.InUtc().ToDateTimeUtc());
				if (!subseriesInterval.Contains(tickTime))
				{
					// Tick out of the interval
					break;
				}
				// Tick in the interval
				// --> Incrementing the number of ticks in the subseries
				subseriesNbTicks++;
			}

			return Subseries(beginIndex, beginIndex + subseriesNbTicks - 1);
		}

		/// <summary>
		/// Splits the time series into sub-series containing nbTicks ticks each.<br/>
		/// The current time series is splitted every nbTicks ticks.<br/>
		/// The last sub-series may have less ticks than nbTicks. </summary>
		/// <param name="nbTicks"> the number of ticks of each sub-series </param>
		/// <returns> a list of sub-series </returns>
		public virtual IList<TimeSeries> Split(int nbTicks)
		{
			var rc = new List<TimeSeries>();
			for (var i = _beginIndex; i <= _endIndex; i += nbTicks)
			{
				// For each nbTicks ticks
				var subseriesBegin = i;
				var subseriesEnd = Math.Min(subseriesBegin + nbTicks - 1, _endIndex);
				rc.Add(Subseries(subseriesBegin, subseriesEnd));
			}
			return rc;
		}

		/// <summary>
		/// Splits the time series into sub-series lasting sliceDuration.<br/>
		/// The current time series is split every splitDuration.<br/>
		/// The last sub-series may last less than sliceDuration. </summary>
		/// <param name="splitDuration"> the duration between 2 splits </param>
		/// <param name="sliceDuration"> the duration of each sub-series </param>
		/// <returns> a list of sub-series </returns>
		public virtual IList<TimeSeries> Split(Period splitDuration, Period sliceDuration)
		{
			var rc = new List<TimeSeries>();
			if (splitDuration != null && !splitDuration.Equals(Period.Zero) && sliceDuration != null && !sliceDuration.Equals(Period.Zero))
			{
				var beginIndexes = GetSplitBeginIndexes(splitDuration);
				foreach (var subseriesBegin in beginIndexes)
				{
				    if (subseriesBegin != null) rc.Add(Subseries(subseriesBegin.Value, sliceDuration));
				}
			}
			return rc;
		}

		/// <summary>
		/// Splits the time series into sub-series lasting duration.<br/>
		/// The current time series is splitted every duration.<br/>
		/// The last sub-series may last less than duration. </summary>
		/// <param name="duration"> the duration between 2 splits (and of each sub-series) </param>
		/// <returns> a list of sub-series </returns>
		public virtual IList<TimeSeries> Split(Period duration)
		{
			return Split(duration, duration);
		}

		/// <summary>
		/// Runs the strategy over the series.
		/// <para>
		/// Opens the trades with <seealso cref="OrderType.Buy"/> orders.
		/// </para>
		/// </summary>
		/// <param name="strategy"> the trading strategy </param>
		/// <returns> the trading record coming from the run </returns>
		public virtual TradingRecord Run(Strategy strategy)
		{
			return Run(strategy, OrderType.Buy);
		}

		/// <summary>
		/// Runs the strategy over the series.
		/// <para>
		/// Opens the trades with <seealso cref="OrderType.Buy"/> orders.
		/// </para>
		/// </summary>
		/// <param name="strategy"> the trading strategy </param>
		/// <param name="orderType"> the <seealso cref="OrderType"/> used to open the trades </param>
		/// <returns> the trading record coming from the run </returns>
		public virtual TradingRecord Run(Strategy strategy, OrderType orderType)
		{
			return Run(strategy, orderType, Decimal.NaNRenamed);
		}

		/// <summary>
		/// Runs the strategy over the series.
		/// <para>
		/// </para>
		/// </summary>
		/// <param name="strategy"> the trading strategy </param>
		/// <param name="orderType"> the <seealso cref="OrderType"/> used to open the trades </param>
		/// <param name="amount"> the amount used to open/close the trades </param>
		/// <returns> the trading record coming from the run </returns>
		public virtual TradingRecord Run(Strategy strategy, OrderType orderType, Decimal amount)
		{
			Log.Trace("Running strategy: {0} (starting with {1})", strategy, orderType);

			var tradingRecord = new TradingRecord(orderType);
			for (var i = _beginIndex; i <= _endIndex; i++)
			{
				// For each tick in the sub-series...       
				if (strategy.ShouldOperate(i, tradingRecord))
				{
					tradingRecord.Operate(i, _ticks[i].ClosePrice, amount);
				}
			}

			if (!tradingRecord.Closed)
			{
				// If the last trade is still opened, we search out of the end index.
				// May works if the current series is a sub-series (but not the last sub-series).
				for (var i = _endIndex + 1; i < _ticks.Count; i++)
				{
					// For each tick out of sub-series bound...
					// --> Trying to close the last trade
					if (strategy.ShouldOperate(i, tradingRecord))
					{
						tradingRecord.Operate(i, _ticks[i].ClosePrice, amount);
						break;
					}
				}
			}
			return tradingRecord;
		}

		/// <summary>
		/// Removes the N first ticks which exceed the maximum tick count.
		/// </summary>
		private void RemoveExceedingTicks()
		{
			var tickCount = _ticks.Count;
		    if (tickCount <= _maximumTickCount) return;

		    // Removing old ticks
		    var nbTicksToRemove = tickCount - _maximumTickCount;
		    for (var i = 0; i < nbTicksToRemove; i++)
		    {
		        _ticks.RemoveAt(0);
		    }
		    // Updating removed ticks count
		    _removedTicksCount += nbTicksToRemove;
		}

		/// <summary>
		/// Builds a list of split indexes from splitDuration. </summary>
		/// <param name="splitDuration"> the duration between 2 splits </param>
		/// <returns> a list of begin indexes after split </returns>
		private IEnumerable<int?> GetSplitBeginIndexes(Period splitDuration)
		{
            // Adding the first begin index
            var beginIndexes = new List<int?> {_beginIndex};

		    // Building the first interval before next split
		    var tickEndTime = GetTick(_beginIndex).EndTime;
            var beginInterval = Instant.FromDateTimeUtc(tickEndTime.InUtc().ToDateTimeUtc());
            var endInterval = Instant.FromDateTimeUtc(tickEndTime.Plus(splitDuration).InUtc().ToDateTimeUtc());
            var splitInterval = new Interval(beginInterval, endInterval);

			for (var i = _beginIndex; i <= _endIndex; i++)
			{
                // For each tick...
                var tickTime = Instant.FromDateTimeUtc(GetTick(i).EndTime.InUtc().ToDateTimeUtc());
			    if (splitInterval.Contains(tickTime)) continue;

			    // Tick out of the interval
			    if (endInterval <= tickTime)
			    {
			        // Tick after the interval
			        // --> Adding a new begin index
			        beginIndexes.Add(i);
			    }

			    // Building the new interval before next split
			    beginInterval = endInterval < tickTime ? tickTime : endInterval;
                endInterval = Instant.FromDateTimeUtc(beginInterval.InUtc().LocalDateTime.Plus(splitDuration).InUtc().ToDateTimeUtc());
			    splitInterval = new Interval(beginInterval, endInterval);
            }
            return beginIndexes;
		}

		/// <param name="series"> a time series </param>
		/// <param name="index"> an out of bounds tick index </param>
		/// <returns> a message for an OutOfBoundsException </returns>
		private static string BuildOutOfBoundsMessage(TimeSeries series, int index)
		{
			return $"Size of series: {series._ticks.Count} ticks, {series._removedTicksCount} ticks removed, index = {index}";
		}
	}
}