using NodaTime;

namespace TA4N.Test.FixtureData
{
    public class GenerateTick
    {
        public static Tick From(double closePrice)
        {
            return From(new LocalDateTime(), closePrice);
        }

        public static Tick From(double closePrice, double volume)
        {
            return new Tick(new LocalDateTime(), 0, 0, 0, closePrice, volume);
        }

        public static Tick From(LocalDateTime endTime, double closePrice)
        {
            return new Tick(endTime, 0, 0, 0, closePrice, 0);
        }

        public static Tick From(double openPrice, double closePrice, double maxPrice, double minPrice)
        {
            return new Tick(new LocalDateTime(), openPrice, maxPrice, minPrice, closePrice, 1);
        }

        public static Tick From(double openPrice, double closePrice, double maxPrice, double minPrice, double volume)
        {
            return new Tick(new LocalDateTime(), openPrice, maxPrice, minPrice, closePrice, volume);
        }

        public static Tick From(LocalDateTime endTime, double openPrice, double closePrice, double maxPrice, double minPrice, double amount, double volume, int trades)
        {
            return new Tick(Period.FromDays(1), endTime, openPrice, maxPrice, minPrice, closePrice, volume, amount, trades);
        }
    }
}