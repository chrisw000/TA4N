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
namespace TA4N.Indicators.Statistics
{
    /// <summary>
	/// Correlation coefficient indicator.
	/// <para>
	/// See also: http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:correlation_coeffici
	/// </para>
	/// </summary>
	public class CorrelationCoefficientIndicator : CachedIndicator<Decimal>
	{
		private readonly VarianceIndicator _variance1;
		private readonly VarianceIndicator _variance2;
		private readonly CovarianceIndicator _covariance;

		/// <summary>
		/// Constructor. </summary>
		/// <param name="indicator1"> the first indicator </param>
		/// <param name="indicator2"> the second indicator </param>
		/// <param name="timeFrame"> the time frame </param>
		public CorrelationCoefficientIndicator(IIndicator<Decimal> indicator1, IIndicator<Decimal> indicator2, int timeFrame) : base(indicator1)
		{
			_variance1 = new VarianceIndicator(indicator1, timeFrame);
			_variance2 = new VarianceIndicator(indicator2, timeFrame);
			_covariance = new CovarianceIndicator(indicator1, indicator2, timeFrame);
		}

		protected override Decimal Calculate(int index)
		{
			var cov = _covariance.GetValue(index);
			var var1 = _variance1.GetValue(index);
			var var2 = _variance2.GetValue(index);

			return cov.DividedBy(var1.MultipliedBy(var2).Sqrt());
		}
	}
}