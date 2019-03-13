using System;
using NodaTime;

namespace TA4N.Extend
{
    // Add some of the joda methods
    public static class ExtendLocalDateTime
    {
        public static LocalDateTime WithMillisOfSecond(this LocalDateTime instance, int milliSeconds)
        {
            if (milliSeconds < 0 || milliSeconds > 999) throw new ArgumentException(nameof(milliSeconds));
            return instance.PlusMilliseconds(milliSeconds - instance.Millisecond);
        }

        public static LocalDateTime WithDayOfMonth(this LocalDateTime instance, int day)
        {
            if (day < 1 || day > 31) throw new ArgumentException(nameof(day));
            return instance.PlusDays(day - instance.Day);
        }

        public static LocalDateTime WithMonthOfYear(this LocalDateTime instance, int month)
        {
            if(month< 1 || month > 12) throw new ArgumentException(nameof(month));
            return instance.PlusMonths(month - instance.Month);
        }

        public static LocalDateTime WithYear(this LocalDateTime instance, int year)
        {
            return instance.PlusYears(year - instance.Year);
        }

        public static LocalDateTime WithDate(this LocalDateTime instance, int year, int month, int day)
        {
            return instance.WithYear(year).WithMonthOfYear(month).WithDayOfMonth(day);
        }
    }
}
