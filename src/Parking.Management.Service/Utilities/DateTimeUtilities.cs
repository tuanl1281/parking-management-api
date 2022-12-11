using System.Globalization;
using System.Text.RegularExpressions;
using Parking.Management.Data.Constants.Common;

namespace Parking.Management.Service.Utilities
{  
    public static class DateTimeUtilities
    {
        #region --- Parse ---
        private static readonly Regex TimeStampRegex =
            new Regex(@"^\d+$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        public static DateTime Parse(string dateString)
        {
            /* Time stamp */
            if (TimeStampRegex.IsMatch(dateString))
            {
                if (long.TryParse(dateString, out var convertedFromTimeStamp))
                    return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(convertedFromTimeStamp);
            }        
            /* Normal */
            string[] dateFormats = { "yyyy-MM-dd", "yyyy-dd-MM", "MM/dd/yyyy", "dd/MM/yyyy", "MMddyyyy", "ddMMyyyy", "yyyy" };
            if (DateTime.TryParseExact(dateString, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var convertedFromFormat))
                return convertedFromFormat;
            
            return DateTime.Now.ToSystemDateTime();
        }
        #endregion
        
        public static DateTime ToSystemDateTime(this double timeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(timeStamp).ToLocalTime();

            return dateTime;
        }
        
        public static DateTime ToSystemDateTime(this DateTime date, bool isDate = true)
        {
            return new DateTime(
                date.Year,
                date.Month,
                date.Day, 
                !isDate ? date.Hour : 0,
                !isDate ? date.Minute : 0,
                !isDate ? date.Second : 0,
                DateTimeKind.Utc
            );
        }

        public static DateTime ToLocalDateTime(this DateTime date, bool isDate = true)
        {
            var convertedDate = new DateTime(
                date.Year,
                date.Month,
                date.Day,
                !isDate ? date.Hour : 0,
                !isDate ? date.Minute : 0,
                !isDate ? date.Second : 0,
                DateTimeKind.Utc
            );
            
            return TimeZoneInfo.ConvertTimeFromUtc(convertedDate, TimeZoneInfo.FindSystemTimeZoneById(TimeConstants.TimeZoneId));
        }

        public static long ToUnixDateTime(this DateTime time)
        {
            var date = new DateTime(1970, 1, 1, 0, 0, 0, time.Kind);
            return Convert.ToInt64((time - date).TotalSeconds);
        }

        public static DateTime StartOfDay(this DateTime date)
            => new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0, DateTimeKind.Utc);
        
        public static DateTime StartOfWeek(DateTime date, DayOfWeek startOfWeek)
        {
            int difference = (7 + (date.DayOfWeek - startOfWeek)) % 7;
            return date.AddDays(-1 * difference).Date;
        }
        
        public static DateTime StartOfMonth(this DateTime date)
            => new DateTime(date.Year, date.Month, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime EndOfDay(this DateTime date)
            => new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999, DateTimeKind.Utc);
        
        public static DateTime EndOfWeek(this DateTime date, DayOfWeek startOfWeek)
        {
            int difference = (7 - (date.DayOfWeek - startOfWeek)) % 7;
            return date.AddDays(1 * difference).Date;
        }
        
        public static DateTime EndOfMonth(this DateTime date)
            => new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month), 0, 0, 0, 0, DateTimeKind.Utc);

        public static bool IsDay(this DateTime date, DayOfWeek dayOfWeek)
            => date.DayOfWeek == dayOfWeek;

        public static bool IsSunday(this DateTime date)
            => IsDay(date, DayOfWeek.Sunday);
        
        public static DateTime GetLocalDateTime(bool isDate = true)
            => DateTime.UtcNow.ToLocalDateTime(isDate);
        
        public static int GetWeekNumberInYear(DateTime date)
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            int weekNumber = currentCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNumber;
        }

        public static int GetDayOfWeekInRange(DateTime fromDate, DateTime toDate, DayOfWeek dayOfWeek)
        {
            var result = 0;
            for (var date = fromDate; date <= toDate; date = date.AddDays(1))
                if (date.DayOfWeek == dayOfWeek)
                    result++;

            return result;
        }
    }
}

