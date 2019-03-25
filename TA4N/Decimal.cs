using System;
using Newtonsoft.Json;

// Just use decimal --->/?
// http://stackoverflow.com/questions/23017583/is-javas-bigdecimal-the-closest-data-type-corresponding-to-cs-decimal

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
namespace TA4N
{

    /// <summary>
	/// Immutable, arbitrary-precision signed decimal numbers designed for technical analysis.
	/// <para>
	/// A <see cref="Decimal"/> consists of a {@code BigDecimal} with arbitrary <seealso cref="MathContext"/> (precision and rounding mode).
	/// 
	/// </para>
	/// </summary>
    [JsonObject(MemberSerialization.OptIn)]
	public sealed class Decimal : IComparable<Decimal>, IFormattable
    {
	    public Decimal OddsToBookPrice()
	    {
	        return Hundred.DividedBy(this);
	    }

	    public Decimal BookPriceToOdds()
	    {
	        return Hundred.DividedBy(this).Round(2);
        }

        /// <summary>
        /// Not-a-Number instance (infinite error) </summary>
        public static readonly Decimal NaNRenamed = new Decimal();

		public static readonly Decimal Zero = ValueOf(0);
	    public static readonly Decimal Half = ValueOf(0.5);
		public static readonly Decimal One = ValueOf(1);
		public static readonly Decimal Two = ValueOf(2);
		public static readonly Decimal Three = ValueOf(3);
		public static readonly Decimal Ten = ValueOf(10);
		public static readonly Decimal Hundred = ValueOf(100);
		public static readonly Decimal Thousand = ValueOf(1000);

        [JsonProperty]
		private readonly decimal? _delegate;

	    /// <summary>
	    /// Constructor.
	    /// Only used for NaN instance.
	    /// </summary>
	    private Decimal()
		{
			_delegate = null;
		}

        /// <summary>
        /// Constructor. </summary>
        /// <param name="val"> the string representation of the decimal value </param>
        private Decimal(string val)
		{
			_delegate = Convert.ToDecimal(val);
		}

		/// <summary>
		/// Constructor. </summary>
		/// <param name="val"> the double value </param>
		private Decimal(double val)
		{
            _delegate = Convert.ToDecimal(val);
		}

		private Decimal(int val)
		{
		    _delegate = val;
		}

		private Decimal(long val)
		{
		    _delegate = val;
		}

		private Decimal(decimal val)
		{
			_delegate = val;
		}

        public static explicit operator decimal? (Decimal value) => value._delegate;
        public static explicit operator decimal (Decimal value)
        {
            if (value?._delegate != null) return value._delegate.Value;
            return 0m; //TODO: what about NaN?
        }

	    public static implicit operator Decimal(byte value) { return new Decimal(value); }
        public static implicit operator Decimal(sbyte value) { return new Decimal(value); }
        public static implicit operator Decimal(short value) { return new Decimal(value); }
        public static implicit operator Decimal(int value) { return new Decimal(value); }
        public static implicit operator Decimal(long value) { return new Decimal(value); }
        public static implicit operator Decimal(ushort value) { return new Decimal(value); }
        public static implicit operator Decimal(uint value) { return new Decimal(value); }
//        public static implicit operator Decimal(ulong value) { return new Decimal(value); }
        public static implicit operator Decimal(float value) { return new Decimal(value); }
        public static implicit operator Decimal(double value) { return new Decimal(value); }
        public static implicit operator Decimal(decimal value) { return new Decimal(value); }
        //        public static implicit operator Decimal(BigInteger value) { return new Decimal(value); }

        /// <summary>
        /// Returns a <see cref="Decimal"/> whose value is {@code (this + augend)},
        /// with rounding according to the context settings. </summary>
        /// <param name="augend"> value to be added to this <see cref="Decimal"/>. </param>
        /// <returns> {@code this + augend}, rounded as necessary </returns>
        public Decimal Plus(Decimal augend)
		{
			if (_delegate==null || augend?._delegate == null)
			{
				return NaNRenamed;
			}

			return new Decimal(_delegate.Value + augend._delegate.Value);
		}

		/// <summary>
		/// Returns a <see cref="Decimal"/> whose value is {@code (this - augend)},
		/// with rounding according to the context settings. </summary>
		/// <param name="subtrahend"> value to be subtracted from this <see cref="Decimal"/>. </param>
		/// <returns> {@code this - subtrahend}, rounded as necessary </returns>
		public Decimal Minus(Decimal subtrahend)
		{
            if (_delegate == null || subtrahend?._delegate == null)
            {
                return NaNRenamed;
			}
			return new Decimal(_delegate.Value - subtrahend._delegate.Value);
		}

		/// <summary>
		/// Returns a <see cref="Decimal"/> whose value is {@code this * multiplicand},
		/// with rounding according to the context settings. </summary>
		/// <param name="multiplicand"> value to be multiplied by this <see cref="Decimal"/>. </param>
		/// <returns> {@code this * multiplicand}, rounded as necessary </returns>
		public Decimal MultipliedBy(Decimal multiplicand)
		{
            if (_delegate == null || multiplicand?._delegate == null)
            {
                return NaNRenamed;
			}
			return new Decimal(_delegate.Value * multiplicand._delegate.Value);
		}

		/// <summary>
		/// Returns a <see cref="Decimal"/> whose value is {@code (this / divisor)},
		/// with rounding according to the context settings. </summary>
		/// <param name="divisor"> value by which this <see cref="Decimal"/> is to be divided. </param>
		/// <returns> {@code this / divisor}, rounded as necessary </returns>
		public Decimal DividedBy(Decimal divisor)
		{
            if (_delegate == null || divisor?._delegate == null || divisor.IsZero)
            {
				return NaNRenamed;
			}
			return new Decimal(_delegate.Value / divisor._delegate.Value);
		}

	    /// <summary>
	    /// Returns a <see cref="Decimal"/> whose value is has been rounded to the number of places. </summary>
	    /// <param name="places">The number of places to round to. </param>
	    /// <returns>The rounded result</returns>
        public Decimal Round(int places)
	    {
	        if (_delegate == null)
	        {
	            return NaNRenamed;
	        }
	        return new Decimal(Math.Round(_delegate.Value, places));
        }

		/// <summary>
		/// Returns a <see cref="Decimal"/> whose value is {@code (this % divisor)},
		/// with rounding according to the context settings. </summary>
		/// <param name="divisor"> value by which this <see cref="Decimal"/> is to be divided. </param>
		/// <returns> {@code this % divisor}, rounded as necessary. </returns>
		public Decimal Remainder(Decimal divisor)
		{
            if (_delegate == null || divisor?._delegate == null || divisor.IsZero)
			{
				return NaNRenamed;
			}

			return new Decimal(_delegate.Value % divisor._delegate.Value);
		}


		/// <summary>
		/// Returns a <see cref="Decimal"/> whose value is <tt>(this<sup>n</sup>)</tt>. </summary>
		/// <param name="n"> power to raise this <see cref="Decimal"/> to. </param>
		/// <returns> <tt>this<sup>n</sup></tt> </returns>
		public Decimal Pow(int n)
		{
            return _delegate == null ? NaNRenamed
                : new Decimal(Math.Pow((double)_delegate.Value, n));
		}

		/// <summary>
		/// Returns the correctly rounded natural logarithm (base e) of the <code>double</code> value of this <see cref="Decimal"/>.
		/// /!\ Warning! Uses the {@code StrictMath#log(double)} method under the hood. </summary>
		/// <returns> the natural logarithm (base e) of {@code this} </returns>
		public Decimal Log()
		{
		    return _delegate==null ? NaNRenamed 
                : new Decimal(Math.Log((double)_delegate.Value));
		}

		/// <summary>
		/// Returns the correctly rounded positive square root of the <code>double</code> value of this <see cref="Decimal"/>.
		/// /!\ Warning! Uses the {@code StrictMath#sqrt(double)} method under the hood. </summary>
		/// <returns> the positive square root of {@code this} </returns>
		public Decimal Sqrt()
		{
            return _delegate == null ? NaNRenamed
                : new Decimal(Math.Sqrt((double)_delegate.Value));
		}

		/// <summary>
		/// Returns a <see cref="Decimal"/> whose value is the absolute value
		/// of this <see cref="Decimal"/>. </summary>
		/// <returns> {@code abs(this)} </returns>
		public Decimal Abs()
		{
            return _delegate == null ? NaNRenamed
                : new Decimal(Math.Abs(_delegate.Value));
		}

		/// <summary>
		/// Checks if the value is zero. </summary>
		/// <returns> true if the value is zero, false otherwise </returns>
		public bool IsZero
		{
			get
			{
				if (_delegate==null) return false;

				return CompareTo(Zero) == 0;
			}
		}

		/// <summary>
		/// Checks if the value is greater than zero. </summary>
		/// <returns> true if the value is greater than zero, false otherwise </returns>
		public bool IsPositive
		{
			get
			{
				if (_delegate == null) return false;

				return CompareTo(Zero) > 0;
			}
		}

		/// <summary>
		/// Checks if the value is zero or greater. </summary>
		/// <returns> true if the value is zero or greater, false otherwise </returns>
		public bool IsPositiveOrZero
		{
			get
			{
                if (_delegate == null) return false;

                return CompareTo(Zero) >= 0;
			}
		}

		/// <summary>
		/// Checks if the value is Not-a-Number. </summary>
		/// <returns> true if the value is Not-a-Number (NaN), false otherwise </returns>
		public bool NaN => _delegate == null;

	    /// <summary>
		/// Checks if the value is less than zero. </summary>
		/// <returns> true if the value is less than zero, false otherwise </returns>
		public bool IsNegative
		{
			get
			{
                if (_delegate == null) return false;

                return CompareTo(Zero) < 0;
			}
		}

		/// <summary>
		/// Checks if the value is zero or less. </summary>
		/// <returns> true if the value is zero or less, false otherwise </returns>
		public bool IsNegativeOrZero
		{
			get
			{
                if (_delegate == null) return false;

                return CompareTo(Zero) <= 0;
			}
		}

		/// <summary>
		/// Checks if this value is equal to another. </summary>
		/// <param name="other"> the other value, not null </param>
		/// <returns> true is this is greater than the specified value, false otherwise </returns>
		public bool IsEqual(Decimal other)
		{
			if ((_delegate==null) || (other._delegate == null))
			{
				return false;
			}
			return CompareTo(other) == 0;
		}

		/// <summary>
		/// Checks if this value is greater than another. </summary>
		/// <param name="other"> the other value, not null </param>
		/// <returns> true is this is greater than the specified value, false otherwise </returns>
		public bool IsGreaterThan(Decimal other)
		{
            if ((_delegate == null) || (other._delegate == null))
            {
				return false;
			}
			return CompareTo(other) > 0;
		}

		/// <summary>
		/// Checks if this value is greater than or equal to another. </summary>
		/// <param name="other"> the other value, not null </param>
		/// <returns> true is this is greater than or equal to the specified value, false otherwise </returns>
		public bool IsGreaterThanOrEqual(Decimal other)
		{
            if ((_delegate == null) || (other._delegate == null))
            {
				return false;
			}
			return CompareTo(other) > -1;
		}

		/// <summary>
		/// Checks if this value is less than another. </summary>
		/// <param name="other"> the other value, not null </param>
		/// <returns> true is this is less than the specified value, false otherwise </returns>
		public bool IsLessThan(Decimal other)
		{
            if ((_delegate == null) || (other._delegate == null))
            {
				return false;
			}
			return CompareTo(other) < 0;
		}

		/// <summary>
		/// Checks if this value is less than or equal to another. </summary>
		/// <param name="other"> the other value, not null </param>
		/// <returns> true is this is less than or equal to the specified value, false otherwise </returns>
		public bool IsLessThanOrEqual(Decimal other)
		{
            if ((_delegate == null) || (other._delegate == null))
            {
				return false;
			}
			return CompareTo(other) < 1;
		}

		public int CompareTo(Decimal other)
		{
            if ((_delegate == null) || (other._delegate == null))
            {
				return 0;
			}
		    if (_delegate < other._delegate) return -1;
		    return _delegate.Equals(other._delegate) ? 0 : 1;
		}

		/// <summary>
		/// Returns the minimum of this <see cref="Decimal"/> and {@code other}. </summary>
		/// <param name="other"> value with which the minimum is to be computed </param>
		/// <returns> the <see cref="Decimal"/> whose value is the lesser of this
		///         <see cref="Decimal"/> and {@code other}.  If they are equal,
		///         as defined by the <seealso cref="#compareTo(Decimal) compareTo"/>
		///         method, {@code this} is returned. </returns>
		public Decimal Min(Decimal other)
		{
            if ((_delegate == null) || (other._delegate == null))
            {
				return NaNRenamed;
			}
			return (CompareTo(other) <= 0 ? this : other);
		}

		/// <summary>
		/// Returns the maximum of this <see cref="Decimal"/> and {@code other}. </summary>
		/// <param name="other"> value with which the maximum is to be computed </param>
		/// <returns> the <see cref="Decimal"/> whose value is the greater of this
		///         <see cref="Decimal"/> and {@code other}.  If they are equal,
		///         as defined by the <seealso cref="#compareTo(Decimal) compareTo"/>
		///         method, {@code this} is returned. </returns>
		public Decimal Max(Decimal other)
		{
            if ((_delegate == null) || (other._delegate == null))
            {
				return NaNRenamed;
			}
			return (CompareTo(other) >= 0 ? this : other);
		}

		/// <summary>
		/// Converts this <see cref="Decimal"/> to a {@code double}. </summary>
		/// <returns> this <see cref="Decimal"/> converted to a {@code double} </returns>
		public double ToDouble()
		{
		    return _delegate==null ? Double.NaN 
                : (double)_delegate.Value;
		}

        public string ToString(IFormatProvider provider)
        {
            return _delegate == null ? "NaN"
                    : _delegate.Value.ToString(provider);
        }

        public string ToString(string format, IFormatProvider provider)
        {
            return _delegate == null ? "NaN"
                    : _delegate.Value.ToString(format, provider);
        }

        public string ToString(string format)
        {
            return _delegate == null ? "NaN"
                    : _delegate.Value.ToString(format);
        }

        public override string ToString()
		{
		    return _delegate==null ? "NaN" 
                    : _delegate.ToString();
		}

		public override int GetHashCode()
		{
			var hash = 7;
			hash = 53 * hash + (_delegate != null ? _delegate.GetHashCode() : 0);
			return hash;
		}

		/// <summary>
		/// {@inheritDoc}
		/// Warning: This method returns true if `this` and `obj` are both NaN.
		/// </summary>
		public override bool Equals(object obj)
		{
            var other = obj as Decimal;

		    if (other==null) return false;

			return _delegate != null && (CompareTo(other) == 0);
		}

		public static Decimal ValueOf(string val)
		{
		    return "NaN".Equals(val) ? NaNRenamed : new Decimal(val);
		}

		public static Decimal ValueOf(double val)
		{
		    return double.IsNaN(val) ? NaNRenamed : new Decimal(val);
		}

		public static Decimal ValueOf(int val)
		{
			return new Decimal(val);
		}

		public static Decimal ValueOf(long val)
		{
			return new Decimal(val);
		}
	}
    

}