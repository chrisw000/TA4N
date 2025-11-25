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
using NodaTime;
using NUnit.Framework;

namespace TA4N.Test
{
    public sealed class TickTest
    {
        private Tick _tick;
        private LocalDateTime _beginTime;
        private LocalDateTime _endTime;

        [SetUp]
        public void SetUp()
        {
            _beginTime = new LocalDateTime(2014, 6, 25, 0, 0);
            _endTime = new LocalDateTime(2014, 6, 25, 1, 0);
            _tick = new Tick(Period.FromHours(1), _endTime);
        }

        static void AssertDiff(Decimal a, Decimal b, decimal diff = 0.0001m)
        {
            Assert.That(Math.Abs((decimal)a.Minus(b)), Is.LessThan(diff));
        }

        [Test]
        public void AddTrades()
        {
            _tick.AddTrade(3.0, 200.0);
            _tick.AddTrade(4.0, 201.0);
            _tick.AddTrade(2.0, 198.0);
            Assert.That(_tick.Trades, Is.EqualTo(3));
            AssertDiff(_tick.Amount, 9);
            AssertDiff(_tick.OpenPrice, 200);
            AssertDiff(_tick.ClosePrice, 198);
            AssertDiff(_tick.MinPrice, 198);
            AssertDiff(_tick.MaxPrice, 201);
            AssertDiff(_tick.Volume, (3 * 200) + (4 * 201) + (2 * 198));
        }

        [Test]
        public void GetTimePeriod()
        {
            Assert.That(_tick.EndTime.Minus(_tick.TimePeriod), Is.EqualTo(_beginTime));
        }

        [Test]
        public void GetBeginTime()
        {
            Assert.That(_tick.BeginTime, Is.EqualTo(_beginTime));
        }

        [Test]
        public void InPeriod()
        {
            Assert.That(_tick.InPeriod(null), Is.False);
            Assert.That(_tick.InPeriod(_beginTime.PlusDays(-1)), Is.False);
            Assert.That(_tick.InPeriod(_beginTime.PlusDays(1)), Is.False);
            Assert.That(_tick.InPeriod(_beginTime.PlusMinutes(30)), Is.True);
            Assert.That(_tick.InPeriod(_beginTime.PlusMinutes(90)), Is.False);
            Assert.That(_tick.InPeriod(_beginTime), Is.True);
            Assert.That(_tick.InPeriod(_endTime), Is.False);
        }
    }
}