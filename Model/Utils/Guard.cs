using System;
using System.Collections.Generic;
using System.Linq;

namespace StackErp.Model.Utils
{
    public interface IGuardClause
    {
    }
    public class Guard : IGuardClause
    {
        public static IGuardClause Against { get; } = new Guard();

        private Guard() { }
    }

    public static class GuardClauseExtensions
    {
        public static void Null(this IGuardClause guardClause, object input, string parameterName)
        {
            if (null == input)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        public static void NullOrEmpty(this IGuardClause guardClause, string input, string parameterName)
        {
            Guard.Against.Null(input, parameterName);
            if (input == String.Empty)
            {
                throw new ArgumentException($"Required input {parameterName} was empty.", parameterName);
            }
        }

        public static void NullOrEmpty<T>(this IGuardClause guardClause, IEnumerable<T> input, string parameterName)
        {
            Guard.Against.Null(input, parameterName);
            if (!input.Any())
            {
                throw new ArgumentException($"Required input {parameterName} was empty.", parameterName);
            }
        }
        public static void NullOrWhiteSpace(this IGuardClause guardClause, string input, string parameterName)
        {
            Guard.Against.NullOrEmpty(input, parameterName);
            if (String.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException($"Required input {parameterName} was empty.", parameterName);
            }
        }

        public static void OutOfRange(this IGuardClause guardClause, int input, string parameterName, int rangeFrom, int rangeTo)
        {

            OutOfRange<int>(guardClause, input, parameterName, rangeFrom, rangeTo);
        }

        public static void OutOfRange(this IGuardClause guardClause, DateTime input, string parameterName, DateTime rangeFrom, DateTime rangeTo)
        {
            OutOfRange<DateTime>(guardClause, input, parameterName, rangeFrom, rangeTo);
        }
        public static void OutOfSQLDateRange(this IGuardClause guardClause, DateTime input, string parameterName)
        {
            // System.Data is unavailable in .NET Standard so we can't use SqlDateTime.
            const long sqlMinDateTicks = 552877920000000000;
            const long sqlMaxDateTicks = 3155378975999970000;

            OutOfRange<DateTime>(guardClause, input, parameterName, new DateTime(sqlMinDateTicks), new DateTime(sqlMaxDateTicks));
        }

        public static void OutOfRange(this IGuardClause guardClause, decimal input, string parameterName, decimal rangeFrom, decimal rangeTo)
        {
            OutOfRange<decimal>(guardClause, input, parameterName, rangeFrom, rangeTo);
        }
        public static void OutOfRange(this IGuardClause guardClause, short input, string parameterName, short rangeFrom, short rangeTo)
        {
            OutOfRange<short>(guardClause, input, parameterName, rangeFrom, rangeTo);
        }

        private static void OutOfRange<T>(this IGuardClause guardClause, T input, string parameterName, T rangeFrom, T rangeTo)
        {
            Comparer<T> comparer = Comparer<T>.Default;

            if (comparer.Compare(rangeFrom, rangeTo) > 0)
            {
                throw new ArgumentException($"{nameof(rangeFrom)} should be less or equal than {nameof(rangeTo)}");
            }

            if (comparer.Compare(input, rangeFrom) < 0 || comparer.Compare(input, rangeTo) > 0)
            {
                throw new ArgumentOutOfRangeException(parameterName, $"Input {parameterName} was out of range");
            }
        }
        public static void Zero(this IGuardClause guardClause, int input, string parameterName)
        {
            Zero<int>(guardClause, input, parameterName);
        }

        public static void Zero(this IGuardClause guardClause, long input, string parameterName)
        {
            Zero<long>(guardClause, input, parameterName);
        }

        public static void Zero(this IGuardClause guardClause, decimal input, string parameterName)
        {
            Zero<decimal>(guardClause, input, parameterName);
        }

        public static void Zero(this IGuardClause guardClause, float input, string parameterName)
        {
            Zero<float>(guardClause, input, parameterName);
        }

        public static void Zero(this IGuardClause guardClause, double input, string parameterName)
        {
            Zero<double>(guardClause, input, parameterName);
        }
        private static void Zero<T>(this IGuardClause guardClause, T input, string parameterName)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (EqualityComparer<T>.Default.Equals(input, default(T)))
            {
                throw new ArgumentException($"Required input {parameterName} cannot be zero.", parameterName);
            }
        }

        public static void OutOfRange<T>(this IGuardClause guardClause, int input, string parameterName) where T : Enum
        {
            if (Enum.IsDefined(typeof(T), input)) return;
            var enumName = typeof(T).ToString();
            throw new ArgumentOutOfRangeException(parameterName, $"Required input {parameterName} was not a valid enum value for {typeof(T).ToString()}.");
        }

        public static void Default<T>(this IGuardClause guardClause, T input, string parameterName)
        {
            if (EqualityComparer<T>.Default.Equals(input, default(T)))
            {
                throw new ArgumentException($"Parameter [{parameterName}] is default value for type {typeof(T).Name}", parameterName);
            }
        }
    }
}
