using CashCalculator.Classes.Extensions;
using System;
using System.Linq;

namespace CashCalculator.Classes
{
    class CalendarMonth
    {
        public CalendarDay[] DaysList = new CalendarDay[42];
        public CalendarMonth(DateTime SelectedDate, int[] WorkedDays, int[] Holydays)
        {
            DateTime Date = SelectedDate.GetStartDate();
            for (int Week = 0; Week < 6; Week++)
                for (int WeekDay = 0; WeekDay < 7; WeekDay++)
                {
                    CalendarDay NewDay = new CalendarDay(Date, SelectedDate.Month);
                    if (Date.Month == SelectedDate.Month)
                    {
                        if (WorkedDays.Length != 0 || Holydays.Length != 0)
                        {
                            NewDay.IsWorked = WorkedDays.Contains(Date.Day);
                            NewDay.IsHolyday = Holydays.Contains(Date.Day);
                        }
                        else
                        {
                            NewDay.IsHolyday = Date.GetDayOfWeek() > 4;
                        }
                    }
                    DaysList[WeekDay + 7 * Week] = NewDay;
                    Date = Date.AddDays(1);
                }
        }
        public int[] GetWorkedDays() => DaysList.Where(d => d.IsWorked).Select(d => d.Date.Day).ToArray();
        public int[] GetHolydays() => DaysList.Where(d => d.IsHolyday).Select(d => d.Date.Day).ToArray();
    }
}