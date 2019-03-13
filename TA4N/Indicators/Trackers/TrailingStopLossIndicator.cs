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
namespace TA4N.Indicators.Trackers
{
    /// <summary>
	/// This class implenents a basic trailing stop loss indicator.
	/// 
	/// Basic idea: 
	/// Your stop order limit is automatically adjusted while price is rising. 
	/// On falling prices the initial StopLossDistance is reduced. 
	/// Sell signal: When StopLossDistance becomes '0'
	/// 
	/// Usage:
	/// 
	/// // Buying rule
	/// Rule buyingRule = new BooleanRule(true); // No real buying rule
	/// 
	/// // Selling rule
	/// Rule sellingRule = new CrossedDownIndicatorRule(ClosePrice_Indicator, TrailingStopLoss_Indicator).and(new JustOnceRule());
	/// 
	/// // Strategy
	/// Strategy strategy = new Strategy(buyingRule, sellingRule);
	/// 
	/// Hints:
	/// There are two constructors for two use cases: 
	///  - Constructor 1: No initialStopLimit is needed. It is taken from the first indicator value
	///  - Constructor 2: You can set an initialStopLimit 
	/// It may influence the trade signals of the strategy depending which constructor you choose.  
	/// 
	/// @author Bastian Engelmann
	/// </summary>
	public class TrailingStopLossIndicator : CachedIndicator<Decimal>
	{
		private readonly IIndicator<Decimal> _indicator;
        private Decimal _stopLossLimit;
        private readonly Decimal _stopLossDistance;

		/// <summary>
		/// Constructor. </summary>
		/// <param name="indicator"> an indicator </param>
		/// <param name="stopLossDistance"> the stop-loss distance (absolute) </param>
		public TrailingStopLossIndicator(IIndicator<Decimal> indicator, Decimal stopLossDistance) : this(indicator, stopLossDistance, Decimal.NaNRenamed)
		{
		}

		/// <summary>
		/// Constructor. </summary>
		/// <param name="indicator"> an indicator </param>
		/// <param name="stopLossDistance"> the stop-loss distance (absolute) </param>
		/// <param name="initialStopLossLimit"> the initial stop-loss limit </param>
		public TrailingStopLossIndicator(IIndicator<Decimal> indicator, Decimal stopLossDistance, Decimal initialStopLossLimit) : base(indicator)
		{
			_indicator = indicator;
			_stopLossDistance = stopLossDistance;
			_stopLossLimit = initialStopLossLimit;
		}

		/// <summary>
		/// Simple implementation of the trailing stop-loss concept.
		/// Logic:
		/// IF CurrentPrice - StopLossDistance > StopLossLimit THEN StopLossLimit = CurrentPrice - StopLossDistance </summary>
		/// <param name="index"> </param>
		/// <returns> Decimal </returns>
		protected override Decimal Calculate(int index)
		{
			if (_stopLossLimit.NaN)
			{
				// Case without initial stop-loss limit value
				_stopLossLimit = _indicator.GetValue(0).Minus(_stopLossDistance);
			}
			var currentValue = _indicator.GetValue(index);
			var referenceValue = _stopLossLimit.Plus(_stopLossDistance);

			if (currentValue.IsGreaterThan(referenceValue))
			{
				_stopLossLimit = currentValue.Minus(_stopLossDistance);
			}
			return _stopLossLimit;
		}
	}
}