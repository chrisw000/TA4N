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
    using HighestValueIndicator = Helpers.HighestValueIndicator;
	using LowestValueIndicator = Helpers.LowestValueIndicator;
	using MaxPriceIndicator = Simple.MaxPriceIndicator;
	using MinPriceIndicator = Simple.MinPriceIndicator;

	/// <summary>
	/// Parabolic SAR indicator.
	/// </summary>
	public class ParabolicSarIndicator : RecursiveCachedIndicator<Decimal>
	{
		private static readonly Decimal DefaultAcceleration = Decimal.ValueOf("0.02");
		private static readonly Decimal AccelerationThreshold = Decimal.ValueOf("0.19");
		private static readonly Decimal MaxAcceleration = Decimal.ValueOf("0.2");
		private static readonly Decimal AccelerationIncrement = Decimal.ValueOf("0.02");
		private Decimal _acceleration = DefaultAcceleration;
		private readonly TimeSeries _series;
		private Decimal _extremePoint;

        private readonly LowestValueIndicator _lowestValueIndicator;
		private readonly HighestValueIndicator _highestValueIndicator;

		public ParabolicSarIndicator(TimeSeries series, int timeFrame) : base(series)
		{
			_series = series;
			_lowestValueIndicator = new LowestValueIndicator(new MinPriceIndicator(series), timeFrame);
			_highestValueIndicator = new HighestValueIndicator(new MaxPriceIndicator(series), timeFrame);
		}

		protected override Decimal Calculate(int index)
		{
			if (index <= 1)
			{
				// Warning: should the min or the max price, according to the trend
				// But we don't know the trend yet, so we use the close price.
				_extremePoint = _series.GetTick(index).ClosePrice;
				return _extremePoint;
			}

			var n2ClosePrice = _series.GetTick(index - 2).ClosePrice;
			var n1ClosePrice = _series.GetTick(index - 1).ClosePrice;
			var nClosePrice = _series.GetTick(index).ClosePrice;

			Decimal sar;
			if (n2ClosePrice.IsGreaterThan(n1ClosePrice) && n1ClosePrice.IsLessThan(nClosePrice))
			{
				// Trend switch: \_/
				sar = _extremePoint ?? _lowestValueIndicator.GetValue(index);
				_extremePoint = _highestValueIndicator.GetValue(index);
				_acceleration = DefaultAcceleration;
			} else if (n2ClosePrice.IsLessThan(n1ClosePrice) && n1ClosePrice.IsGreaterThan(nClosePrice))
			{
				// Trend switch: /¯\
				sar = _extremePoint ?? _highestValueIndicator.GetValue(index);
				_extremePoint = _lowestValueIndicator.GetValue(index);
				_acceleration = DefaultAcceleration;

			} else if (nClosePrice.IsLessThan(n1ClosePrice))
			{
				 // Downtrend: falling SAR
				var lowestValue = _lowestValueIndicator.GetValue(index);
				if (_extremePoint.IsGreaterThan(lowestValue))
				{
					IncrementAcceleration();
					_extremePoint = lowestValue;
				}
				sar = CalculateSar(index);

				var n2MaxPrice = _series.GetTick(index - 2).MaxPrice;
				var n1MaxPrice = _series.GetTick(index - 1).MaxPrice;
				var nMaxPrice = _series.GetTick(index).MaxPrice;

				if (n1MaxPrice.IsGreaterThan(sar))
				{
					sar = n1MaxPrice;
				} else if (n2MaxPrice.IsGreaterThan(sar))
				{
					sar = n2MaxPrice;
				}
				if (nMaxPrice.IsGreaterThan(sar))
				{
					sar = _series.GetTick(index).MinPrice;
				}

			} else
			{
				 // Uptrend: rising SAR
				var highestValue = _highestValueIndicator.GetValue(index);
                if (_extremePoint == null || _extremePoint.IsLessThan(highestValue))
				{
					IncrementAcceleration();
					_extremePoint = highestValue;
				}
				sar = CalculateSar(index);

				var n2MinPrice = _series.GetTick(index - 2).MinPrice;
				var n1MinPrice = _series.GetTick(index - 1).MinPrice;
				var nMinPrice = _series.GetTick(index).MinPrice;

				if (n1MinPrice.IsLessThan(sar))
				{
					sar = n1MinPrice;
				} else if (n2MinPrice.IsLessThan(sar))
				{
					sar = n2MinPrice;
				}
				if (nMinPrice.IsLessThan(sar))
				{
					sar = _series.GetTick(index).MaxPrice;
				}

			}
			return sar;
		}

		/// <summary>
		/// Increments the acceleration factor.
		/// </summary>
		private void IncrementAcceleration()
		{
			if (_acceleration.IsGreaterThanOrEqual(AccelerationThreshold))
			{
				_acceleration = MaxAcceleration;
			} else
			{
				_acceleration = _acceleration.Plus(AccelerationIncrement);
			}
		}

		/// <summary>
		/// Calculates the SAR. </summary>
		/// <param name="index"> the tick index </param>
		/// <returns> the SAR </returns>
		private Decimal CalculateSar(int index)
		{
			var previousSar = GetValue(index - 1);
			return _extremePoint.MultipliedBy(_acceleration).Plus(Decimal.One.Minus(_acceleration).MultipliedBy(previousSar));
		}
	}
}