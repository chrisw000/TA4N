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
using Microsoft.Extensions.Logging;

namespace TA4N
{
	/// <summary>
	/// A trading strategy.
	/// <para>
	/// A strategy is a pair of complementary <seealso cref="IRule"/>. It may recommend to enter or to exit.
	/// Recommendations are based respectively on the entry rule or on the exit rule.
	/// </para>
	/// </summary>
	public sealed class Strategy
    {
        private readonly ILogger<Strategy> _logger = LogWrapper.Factory?.CreateLogger<Strategy>();

		/// <summary>The entry rule </summary>
		private readonly IRule _entryRule;

		/// <summary>The exit rule </summary>
		private readonly IRule _exitRule;

		/// <summary>
		/// The unstable period (number of ticks).<br/>
		/// During the unstable period of the strategy any order placement will be cancelled.<br/>
		/// I.e. no entry/exit signal will be fired before index == unstablePeriod.
		/// </summary>
		private int _unstablePeriod;

		/// <param name="entryRule"> the entry rule </param>
		/// <param name="exitRule"> the exit rule </param>
		public Strategy(IRule entryRule, IRule exitRule)
		{
            _entryRule = entryRule ?? throw new ArgumentNullException(nameof(entryRule));
			_exitRule = exitRule ?? throw new ArgumentNullException(nameof(entryRule));
        }

		/// <param name="index"> a tick index </param>
		/// <returns> true if this strategy is unstable at the provided index, false otherwise (stable) </returns>
		public bool IsUnstableAt(int index)
		{
			return index < _unstablePeriod;
		}

		/// <param name="unstablePeriod"> number of ticks that will be strip off for this strategy </param>
		public int UnstablePeriod
		{
			set
			{
				_unstablePeriod = value;
			}
		}

		/// <param name="index"> the tick index </param>
		/// <param name="tradingRecord"> the potentially needed trading history </param>
		/// <returns> true to recommend an order, false otherwise (no recommendation) </returns>
		public bool ShouldOperate(int index, TradingRecord tradingRecord)
		{
			var trade = tradingRecord.CurrentTrade;
			if (trade.New)
			{
				return ShouldEnter(index, tradingRecord);
			}
			if (trade.Opened)
			{
				return ShouldExit(index, tradingRecord);
			}
			return false;
		}

		/// <param name="index"> the tick index </param>
		/// <returns> true to recommend to enter, false otherwise </returns>
		public bool ShouldEnter(int index)
		{
			return ShouldEnter(index, null);
		}

		/// <param name="index"> the tick index </param>
		/// <param name="tradingRecord"> the potentially needed trading history </param>
		/// <returns> true to recommend to enter, false otherwise </returns>
		public bool ShouldEnter(int index, TradingRecord tradingRecord)
		{
			if (IsUnstableAt(index))
			{
				return false;
			}

			var enter = _entryRule.IsSatisfied(index, tradingRecord);
			TraceShouldEnter(index, enter);
			return enter;
		}

		/// <param name="index"> the tick index </param>
		/// <returns> true to recommend to exit, false otherwise </returns>
		public bool ShouldExit(int index)
		{
			return ShouldExit(index, null);
		}

		/// <param name="index"> the tick index </param>
		/// <param name="tradingRecord"> the potentially needed trading history </param>
		/// <returns> true to recommend to exit, false otherwise </returns>
		public bool ShouldExit(int index, TradingRecord tradingRecord)
		{
			if (IsUnstableAt(index))
			{
				return false;
			}

			var exit = _exitRule.IsSatisfied(index, tradingRecord);
			TraceShouldExit(index, exit);
			return exit;
		}

		/// <summary>
		/// Traces the shouldEnter() method calls. </summary>
		/// <param name="index"> the tick index </param>
		/// <param name="enter"> true if the strategy should enter, false otherwise </param>
		private void TraceShouldEnter(int index, bool enter)
		{
            _logger?.LogTrace(">>> {0}#shouldEnter({1}): {2}", GetType().Name, index, enter);
		}

		/// <summary>
		/// Traces the shouldExit() method calls. </summary>
		/// <param name="index"> the tick index </param>
		/// <param name="exit"> true if the strategy should exit, false otherwise </param>
		private void TraceShouldExit(int index, bool exit)
        {
            _logger?.LogTrace(">>> {0}#shouldExit({1}): {2}", GetType().Name, index, exit);
		}
	}
}