using System;

namespace BuiltCloud.Portal
{
    public static class CsHtmlExtensions
    {
        public static string ToLocalString(this DateTime date)
        {
            return date.ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss");
        }
    }
}