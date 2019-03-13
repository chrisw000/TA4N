using System;

namespace TA4N.Library
{
    public class TickLoadDateRange
    {
        /// <summary>
        /// Time of the first Tick loaded.
        /// For HR will be -10 from off.
        /// </summary>
        public DateTime FromUtc { get; private set; }

        /// <summary>
        /// Time the Bot would become Active.
        /// For HR will be -6 from off
        /// </summary>
        public DateTime ActiveFromUtc { get; private set; }

        /// <summary>
        /// Time the Bot will no longer Enter Trades
        /// Typically this will be 1 min before the Off
        /// </summary>
        public DateTime ActiveToUtc { get; private set; }

        /// <summary>
        /// Time the Bot will attempt to Close out any Trades
        /// Typically this will be 2 seconds before the Off
        /// </summary>
        public DateTime GreenUtc { get; private set; }

        /// <summary>
        /// Time of the last Tick loaded.
        /// Typically this will be the time of the Off
        /// </summary>
        public DateTime ToUtc { get; private set; }

        /// <summary>
        /// Setup the key points in time while the Bot or replay projection is underway.
        /// FromUtc -> ActiveFromUtc
        ///    - time period to gather day and allow the Strategy.UnstablePeriod to pass
        /// ActiveFromUtc -> ActiveToUtc
        ///    - time period in which Trades will be Entered
        /// ActiveToUtc -> GreenUtc
        ///    - time period to allow any existing Enters to match, or Exits to fire/match
        /// GreenUtc
        ///    - time period any Unmatched trades are cancelled, and Greening Exits are placed
        ///    - TODO: check if can improve the Pessimistic projection here by:
        ///    --> abort unmatched Opens (I think this happens anyway)
        ///    --> don't just exit at Close price, look at live Greening to ensure do the same
        /// </summary>
        /// <param name="offUtc">Time the market starts</param>
        public TickLoadDateRange(DateTime offUtc)
        {
            if(offUtc.Kind!=DateTimeKind.Utc) throw new ArgumentException($"TickLoadDateRange({nameof(offUtc)}) needs to be in UTC, have received: {offUtc.Kind} on {offUtc:O}");

            FromUtc = offUtc.AddMinutes(-10);
            ActiveFromUtc = offUtc.AddMinutes(-6);
            ActiveToUtc = offUtc.AddMinutes(-1);
            GreenUtc = offUtc.AddSeconds(-2);
            ToUtc = offUtc;
        }
    }
}