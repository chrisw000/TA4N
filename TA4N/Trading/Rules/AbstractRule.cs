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
using NLog;

namespace TA4N.Trading.Rules
{
	/// <summary>
	/// An abstract trading <seealso cref="IRule"/>.
	/// </summary>
	public abstract class AbstractRule : IRule
	{
		/// <summary>
		/// The logger </summary>
		protected internal static readonly Logger Log = LogManager.GetCurrentClassLogger();

		public virtual IRule And(IRule rule)
		{
			return new AndRule(this, rule);
		}

		public virtual IRule Or(IRule rule)
		{
			return new OrRule(this, rule);
		}

		public virtual IRule Xor(IRule rule)
		{
			return new XorRule(this, rule);
		}

		public virtual IRule Negation()
		{
			return new NotRule(this);
		}

		public virtual bool IsSatisfied(int index)
		{
			return IsSatisfied(index, null);
		}

	    public abstract bool IsSatisfied(int index, TradingRecord tradingRecord);

	    /// <summary>
		/// Traces the isSatisfied() method calls. </summary>
		/// <param name="index"> the tick index </param>
		/// <param name="isSatisfied"> true if the rule is satisfied, false otherwise </param>
		protected internal virtual void TraceIsSatisfied(int index, bool isSatisfied)
		{
			Log.Trace("{0}#isSatisfied({1}): {2}", GetType().Name, index, isSatisfied);
		}
	}

}