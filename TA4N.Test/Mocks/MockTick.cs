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

using TA4N;
using NodaTime;

namespace TA4N.Mocks
{
	/// <summary>
	/// A mock tick with sample data.
	/// </summary>
	public class MockTick : Tick
	{
	    public MockTick(double closePrice) : this(new LocalDateTime(), closePrice)
		{
		}

		public MockTick(double closePrice, double volume) : base(new LocalDateTime(), 0, 0, 0, closePrice, volume)
		{
		}

		public MockTick(LocalDateTime endTime, double closePrice) : base(endTime, 0, 0, 0, closePrice, 0)
		{
		}

		public MockTick(double openPrice, double closePrice, double maxPrice, double minPrice) : base(new LocalDateTime(), openPrice, maxPrice, minPrice, closePrice, 1)
		{
		}

		public MockTick(double openPrice, double closePrice, double maxPrice, double minPrice, double volume) : base(new LocalDateTime(), openPrice, maxPrice, minPrice, closePrice, volume)
		{
		}

		public MockTick(LocalDateTime endTime, double openPrice, double closePrice, double maxPrice, double minPrice, double amount, double volume, int trades) : base(endTime, openPrice, maxPrice, minPrice, closePrice, volume)
		{
			Amount = Decimal.ValueOf(amount);
			Trades = trades;
		}

		public override Decimal Amount { get; } = Decimal.Zero;

	    public override int Trades { get; } = 0;
	}
}