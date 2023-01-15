using System;

namespace Helpers.Utilities
{
    public static class UtilsDate
    {
        public static long CurrentMillisecondUtc()
        {
            var now = DateTimeOffset.UtcNow;
            return now.ToUnixTimeMilliseconds();
        }
    }
}