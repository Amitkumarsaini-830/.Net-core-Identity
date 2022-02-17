using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace com.aadviktech.IMS.Constant
{
    public static class DateUtility
    {
        public static string GetDateTimeString(DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string GetDateString(DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }

        // Date must be in yyyy-MM-dd format
        public static DateTime GetDateFromString(string date)
        {
            return new DateTime(Convert.ToInt32(date.Split('-')[0]), Convert.ToInt32(date.Split('-')[1]), Convert.ToInt32(date.Split('-')[2]));
        }
        public static DateTime GetDateFromStringDesc(string date)
        {
            return new DateTime(Convert.ToInt32(date.Split('-')[2]), Convert.ToInt32(date.Split('-')[1]), Convert.ToInt32(date.Split('-')[0]));
        }

        public static DateTime GetNowTime()
        {
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            return indianTime;
        }

        public static string GetNowTimeStamp()
        {
            return GetNowTime().ToString("yyyyMMddHHmmssffff", System.Globalization.CultureInfo.InvariantCulture);
        }

        public static DateTime GetDateTimeFromString(string date)
        {
            return DateTime.ParseExact(date, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        }

        public static string GetddmmyyhhmmstTimeString(DateTime date)
        {
            return date.ToString("dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
        }

        public static string GetRoboRechargeDate()
        {
            return GetNowTime().ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        }
        public static string GetRoboRechargeTime()
        {
            return GetNowTime().ToString("HHmmss", System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string GetMPlusString(DateTime date)
        {
            return date.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
        }

        public static string GetBonrixString(DateTime date)
        {
            //5/22/2018 4:13:29 PM
            return date.ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
        }

        public static string GetMarsDate()
        {
            return GetNowTime().ToString("dd/MM/yy", System.Globalization.CultureInfo.InvariantCulture);
        }
        public static string GetMarsTime()
        {
            return GetNowTime().ToString("HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
        }


    }
}
