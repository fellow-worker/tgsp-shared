using System;

namespace TGSP.Shared.Extensions
{
    /// <summary>
    /// This class provides numeric extensions
    /// </summary>
    public static class NumericExtensions
    {
        /// <summary>
        /// Defines the accuracy of calculations within Ibis
        /// </summary>
        public const double Delta = 0.00000000001;

        /// <summary>
        /// Defines the accuracy of calculations within Ibis
        /// </summary>
        public const decimal DeltaDecimal = (decimal)Delta;

        /// <summary>
        /// Defines the accuracy of calculations within Ibis
        /// </summary>
        public const float DeltaFloat = 0.00001f;

        /// <summary>
        /// Returns if two values are equal given a delta
        /// </summary>
        /// <param name="value"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool AlmostEquals(this double value, double other)
        {
            return Math.Abs(value - other) < Delta;
        }

        /// <summary>
        /// Returns if two values are equal given a delta
        /// </summary>
        /// <param name="value"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool AlmostEquals(this decimal value, decimal other)
        {
            var diff = value - other;
            return diff < DeltaDecimal && diff > -1 * DeltaDecimal;
        }

        /// <summary>
        /// Returns if two values are equal given a delta
        /// </summary>
        /// <param name="value"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool AlmostEquals(this float value, float other)
        {
            var diff = value - other;
            return diff < DeltaFloat && diff > -1 * DeltaFloat;
        }

        /// <summary>
        /// Converts a given DateTime into a Unix timestamp
        /// </summary>
        /// <param name="value">Any DateTime</param>
        /// <returns>The given DateTime in Unix timestamp format</returns>
        public static long ToUnixTimestamp(this DateTime value)
        {
            var epoch = new DateTime(1970, 1, 1,0,0,0,DateTimeKind.Utc);
            return (long)Math.Truncate((value.ToUniversalTime().Subtract(epoch)).TotalSeconds);
        }

        /// <summary>
        /// Converts a long to a Unix timestamp
        /// </summary>
        /// <param name="value">Any long</param>
        /// <returns>A date time converted from the timestamp</returns>
        public static DateTime FromUnixTimestamp(this long value)
        {
            var epoch = new DateTime(1970, 1, 1,0,0,0,DateTimeKind.Utc);
            return epoch.AddSeconds(value);
        }

        /// <summary>
        /// Implements a fairly simple equals method for bytes
        /// </summary>
        /// <param name="a"></param>
        /// <param name="that"></param>
        /// <returns></returns>
        public static bool ByteEquals(this byte[] a, byte[] that)
        {
            if(a.Length != that.Length) return false;
            for(var i = 0; i < a.Length; i++)
            {
                if(a[i] != that[i]) return false;
            }
            return true;
        }
    }
}
