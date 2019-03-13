using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

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
	/// An abstract analysis criterion.
	/// </summary>
	public abstract class AbstractAnalysisCriterion : IAnalysisCriterion
	{
	    public abstract double Calculate(TimeSeries series, Trade trade);

	    public abstract double Calculate(TimeSeries series, TradingRecord tradingRecord);

        public abstract bool BetterThan(double criterionValue1, double criterionValue2);

        public Strategy ChooseBest(TimeSeries series, IList<Strategy> strategies)
		{
			var bestStrategy = strategies[0];
			var bestCriterionValue = Calculate(series, series.Run(bestStrategy));

			for (var i = 1; i < strategies.Count; i++)
			{
				var currentStrategy = strategies[i];
				var currentCriterionValue = Calculate(series, series.Run(currentStrategy));

				if (BetterThan(currentCriterionValue, bestCriterionValue))
				{
					bestStrategy = currentStrategy;
					bestCriterionValue = currentCriterionValue;
				}
			}
			return bestStrategy;
		}

	    public override string ToString()
	    {
			var tokens = Regex.Split(GetType().Name, "(?=\\p{Lu})");

            var sb = new StringBuilder();

            for (var i = 0; i < tokens.Length - 1; i++)
			{
                sb.Append(tokens[i]).Append(' ');
			}
			return sb.ToString().Trim();
        }
    }
}