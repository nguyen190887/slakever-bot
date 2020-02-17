using System;
using SlakeverBot.Constants;

namespace SlakeverBot.Utils
{
    public static class DateTimeExtensionw
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static string ToFileString(this DateTime dt)
        {
            return dt.ToString(AppConstants.FileDateFormat);
        }

        public static DateTime FromUnixTime(this long unixTime)
        {
            return UnixEpoch.AddSeconds(unixTime);
        }
    }
}
