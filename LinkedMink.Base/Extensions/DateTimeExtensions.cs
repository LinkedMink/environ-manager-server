using System;

namespace LinkedMink.Base.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime FromUnixTimestamp(long timestamp)
        {
            return UnixEpoch.AddSeconds(timestamp);
        }

        public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
    }
}
