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
using System.Collections.Generic;
using NodaTime;
using TA4N.Extend;

namespace TA4N.Mocks
{
	/// <summary>
	/// A time series with sample data.
	/// </summary>
	public class MockTimeSeries : TimeSeries
	{
		public MockTimeSeries(params double[] data) : base(DoublesToTicks(data))
		{
		}

		public MockTimeSeries(IList<Tick> ticks) : base(ticks)
		{
		}

		public MockTimeSeries(double[] data, LocalDateTime[] times) : base(DoublesAndTimesToTicks(data, times))
		{
		}

		public MockTimeSeries(params LocalDateTime[] dates) : base(TimesToTicks(dates))
		{
		}

		public MockTimeSeries() : base(ArbitraryTicks())
		{
		}

		private static IList<Tick> DoublesToTicks(params double[] data)
		{
			var ticks = new List<Tick>();
			for (var i = 0; i < data.Length; i++)
			{
                //TODO: double check the intention here...
				ticks.Add(new MockTick((new LocalDateTime()).WithMillisOfSecond(i), data[i]));
			}
			return ticks;
		}

		private static IList<Tick> DoublesAndTimesToTicks(double[] data, LocalDateTime[] times)
		{
			if (data.Length != times.Length)
			{
				throw new ArgumentException();
			}
			var ticks = new List<Tick>();
			for (var i = 0; i < data.Length; i++)
			{
				ticks.Add(new MockTick(times[i], data[i]));
			}
			return ticks;
		}

		private static IList<Tick> TimesToTicks(params LocalDateTime[] dates)
		{
			var ticks = new List<Tick>();
			var i = 1;
			foreach (var date in dates)
			{
				ticks.Add(new MockTick(date, i++));
			}
			return ticks;
		}

		private static IList<Tick> ArbitraryTicks()
		{
			var ticks = new List<Tick>();
			for (var i = 0d; i < 10; i++)
			{
				ticks.Add(new MockTick(new LocalDateTime(0,1,1,0,0), i, i + 1, i + 2, i + 3, i + 4, i + 5, (int)(i + 6)));
			}
			return ticks;
		}
	}

}