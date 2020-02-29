﻿using System;
using System.Globalization;
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

        public static DateTime ToGmt7TimeZone(this DateTime dt)
        {
            return dt.AddHours(7);
        }

        public static DateTime FromUnixTime(this long unixTime)
        {
            return UnixEpoch.AddSeconds(unixTime);
        }

        public static DateTime FromFileDate(this string s)
        {
            DateTime.TryParseExact(s, AppConstants.FileDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt);
            return dt;
        }
    }
}
