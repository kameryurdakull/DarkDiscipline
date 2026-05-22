using System;

namespace DarkDiscipline.Domain.Common
{
    public readonly struct DateKey : IEquatable<DateKey>
    {
        public DateKey(int year, int month, int day)
        {
            Year = year;
            Month = month;
            Day = day;
        }

        public int Year { get; }
        public int Month { get; }
        public int Day { get; }

        public static DateKey FromDateTime(DateTime value)
        {
            var local = value.Kind == DateTimeKind.Utc ? value.ToLocalTime() : value;
            return new DateKey(local.Year, local.Month, local.Day);
        }

        public bool Equals(DateKey other)
        {
            return Year == other.Year && Month == other.Month && Day == other.Day;
        }

        public static bool operator ==(DateKey left, DateKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DateKey left, DateKey right)
        {
            return left.Equals(right) == false;
        }

        public override bool Equals(object obj)
        {
            return obj is DateKey other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Year, Month, Day);
        }

        public override string ToString()
        {
            return $"{Year:D4}-{Month:D2}-{Day:D2}";
        }
    }
}
