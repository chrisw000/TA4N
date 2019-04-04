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
using NUnit.Framework;

namespace TA4N.Test
{
	/// <summary>
	/// Utility class for <see cref="Decimal"/> tests.
	/// </summary>
	public static class TaTestsUtils
	{
		/// <summary>
		/// Offset for double equality checking </summary>
		public const double TaOffset = 0.0001;

        /// <summary>
        /// Verifies that the actual <see cref="Decimal"/> value is equal to the given {@code String} representation. </summary>
        /// <param name="actual"> the actual <see cref="Decimal"/> value </param>
        /// <param name="expected"> the given <see cref="string"/> representation to compare the actual value to </param>
        /// <exception cref="AssertionException"> if the actual value is not equal to the given {@code String} representation </exception>
        public static void AssertDecimalEquals(this Decimal actual, string expected)
		{
			Assert.AreEqual(Decimal.ValueOf(expected), actual);
		}

        /// <summary>
        /// Verifies that the actual <see cref="Decimal"/> value is equal to the given {@code Integer} representation. </summary>
        /// <param name="actual"> the actual <see cref="Decimal"/> value </param>
        /// <param name="expected"> the given <see cref="int"/> representation to compare the actual value to </param>
        /// <exception cref="AssertionException"> if the actual value is not equal to the given {@code Integer} representation </exception>
        public static void AssertDecimalEquals(Decimal actual, int expected)
		{
            Assert.AreEqual(Decimal.ValueOf(expected), actual);
		}

        /// <summary>
        /// Verifies that the actual <see cref="Decimal"/> value is equal (within a positive offset) to the given {@code Double} representation. </summary>
        /// <param name="actual"> the actual <see cref="Decimal"/> value </param>
        /// <param name="expected"> the given <see cref="double"/> representation to compare the actual value to </param>
        /// <exception cref="AssertionException"> if the actual value is not equal to the given {@code Double} representation </exception>
        public static void AssertDecimalEquals(Decimal actual, double expected)
		{
            Assert.AreEqual(expected, actual.ToDouble(), TaOffset);
		}

        public static void AssertDecimalEquals(Decimal actual, Decimal expected)
        {
            Assert.AreEqual(expected.ToDouble(), actual.ToDouble(), TaOffset);
        }

	    public static void ArraysFill<T>(T[] array, T value)
	    {
            ArraysFill(array, 0, array.Length, value);
	    }

        // Note: start is inclusive, end is exclusive (as is conventional
        // in computer science)
        public static void ArraysFill<T>(T[] array, int start, int end, T value)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            if (start < 0 || start >= end)
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }
            if ((end-1) >= array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(end));
            }
            for (var i = start; i < end; i++)
            {
                array[i] = value;
            }
        }
    }
}