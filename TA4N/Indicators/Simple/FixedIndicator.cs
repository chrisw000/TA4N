using System.Collections.Generic;

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

namespace TA4N.Indicators.Simple
{
	/// <summary>
	/// A fixed indicator. </summary>
	/// <param>T the type of returned value (Double, Boolean, etc.) </param>
	public class FixedIndicator<T> : AbstractIndicator<T>
	{
		private readonly IList<T> _values = new List<T>();

		/// <summary>
		/// Constructor. </summary>
		/// <param name="values"> the values to be returned by this indicator </param>
		public FixedIndicator(params T[] values) : base(null)
		{
			((List<T>)_values).AddRange(values);
		}

		public void AddValue(T value)
		{
			_values.Add(value);
		}

		public override T GetValue(int index)
		{
			return _values[index];
		}
	}
}