using System.Text;

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
namespace TA4N.Analysis.Criteria
{
    /// <summary>
	/// Versus "buy and hold" criterion.
	/// <para>
	/// Compares the value of a provided <seealso cref="IAnalysisCriterion"/> with the value of a <seealso cref="BuyAndHoldCriterion "buy and hold" criterion"/>.
	/// </para>
	/// </summary>
	public class VersusBuyAndHoldCriterion : AbstractAnalysisCriterion
	{
        private readonly IAnalysisCriterion _criterion;

		/// <summary>
		/// Constructor. </summary>
		/// <param name="criterion"> an analysis criterion to be compared </param>
		public VersusBuyAndHoldCriterion(IAnalysisCriterion criterion)
		{
			_criterion = criterion;
		}

		public override double Calculate(TimeSeries series, TradingRecord tradingRecord)
		{
		    if (series.End == -1) return 1d;

			var fakeRecord = new TradingRecord();
			fakeRecord.Enter(series.Begin);
			fakeRecord.Exit(series.End);

			return _criterion.Calculate(series, tradingRecord) / _criterion.Calculate(series, fakeRecord);
		}

		public override double Calculate(TimeSeries series, Trade trade)
		{
			var fakeRecord = new TradingRecord();
			fakeRecord.Enter(series.Begin);
			fakeRecord.Exit(series.End);

			return _criterion.Calculate(series, trade) / _criterion.Calculate(series, fakeRecord);
		}

		public override bool BetterThan(double criterionValue1, double criterionValue2)
		{
			return criterionValue1 > criterionValue2;
		}

		public override string ToString()
		{
			var sb = new StringBuilder(base.ToString());
			sb.Append(" (").Append(_criterion).Append(')');
			return sb.ToString();
		}
	}
}