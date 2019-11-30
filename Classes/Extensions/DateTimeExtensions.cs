using System;

namespace CashCalculator.Classes.Extensions
{
    static class DateTimeExtensions
    {
        static string[] Months = new string[]
            {
                "Декабрь",
                "Январь", "Февраль", "Март",
                "Апрель", "Май", "Июнь",
                "Июль", "Август", "Сентябрь",
                "Октябрь", "Ноябрь", "Декабрь"
            };
        /// <summary>
        /// 0 - Mon
        /// 6 - Sun
        /// </summary>
        public static int GetDayOfWeek(this DateTime Date) => ((int)Date.AddDays(0).DayOfWeek + 6) % 7;
        public static string GetMonthNameRus(this DateTime Date) => Months[Date.Month];
        public static DateTime GetStartDate(this DateTime Date)
        {
            Date = Date.AddDays(-Date.Day);
            Date = Date.AddDays(-(int)Date.DayOfWeek + 1);
            if (Date.Day == 1)
                Date = Date.AddDays(-7);
            return Date;
        }
        public static string MMYY(this DateTime Date) => Date.ToString("MM_yy");
        public static string YYYY(this DateTime Date) => Date.ToString("yyyy");
    }
}