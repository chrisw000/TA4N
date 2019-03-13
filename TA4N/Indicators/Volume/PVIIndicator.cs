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
namespace TA4N.Indicators.Volume
{
    /// <summary>
	/// Positive Volume Index (PVI) indicator.
	/// </summary>
	/// <see href="http://www.metastock.com/Customer/Resources/TAAZ/Default.aspx?p=92">metastock.com</see>
	/// <see href="http://www.investopedia.com/terms/p/pvi.asp">Investopedia.com</see>
    public class PviIndicator : RecursiveCachedIndicator<Decimal>
	{
        private readonly TimeSeries _series;

		public PviIndicator(TimeSeries series) : base(series)
		{
			_series = series;
		}

		protected override Decimal Calculate(int index)
		{
			if (index == 0)
			{
				return Decimal.Thousand;
			}

			var currentTick = _series.GetTick(index);
			var previousTick = _series.GetTick(index - 1);
			var previousValue = GetValue(index - 1);

			if (currentTick.Volume.IsGreaterThan(previousTick.Volume))
			{
				var currentPrice = currentTick.ClosePrice;
				var previousPrice = previousTick.ClosePrice;
				var priceChangeRatio = currentPrice.Minus(previousPrice).DividedBy(previousPrice);
				return previousValue.Plus(priceChangeRatio.MultipliedBy(previousValue));
			}
			return previousValue;
		}
	}
}