using System;
using SlakeverBot.Constants;

namespace SlakeverBot.Utils
{
    public static class DateTimeExtensionw
    {
        public static string ToFileString(this DateTime dt)
        {
            return dt.ToString(AppConstants.FileDateFormat);
        }
    }
}
