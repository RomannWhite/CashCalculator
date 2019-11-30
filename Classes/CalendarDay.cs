using CashCalculator.Classes.Extensions;
using Android.Graphics;
using Android.Content;
using Android.Widget;
using Android.Views;
using System.Linq;
using System;

namespace CashCalculator.Classes
{
    class CalendarDay
    {
        #region Holidays
        //DateTime[] Holidays = new DateTime[]
        //{
        //    new DateTime(2019, 1, 1),
        //    new DateTime(2019, 1, 2),
        //    new DateTime(2019, 1, 3),
        //    new DateTime(2019, 1, 4),
        //    new DateTime(2019, 1, 7),
        //    new DateTime(2019, 1, 8),

        //    new DateTime(2019, 3, 8),

        //    new DateTime(2019, 5, 1),
        //    new DateTime(2019, 5, 2),
        //    new DateTime(2019, 5, 3),
        //    new DateTime(2019, 5, 9),
        //    new DateTime(2019, 5, 10),

        //    new DateTime(2019, 6, 12),

        //    new DateTime(2019, 11, 4),

        //    new DateTime(2020, 1, 1),
        //    new DateTime(2020, 1, 2),
        //    new DateTime(2020, 1, 3),
        //    new DateTime(2020, 1, 6),
        //    new DateTime(2020, 1, 7),
        //    new DateTime(2020, 1, 8),

        //    new DateTime(2020, 2, 24),

        //    new DateTime(2020, 3, 9),

        //    new DateTime(2020, 5, 1),
        //    new DateTime(2020, 5, 4),
        //    new DateTime(2020, 5, 5),
        //    new DateTime(2020, 5, 11),

        //    new DateTime(2020, 6, 12),

        //    new DateTime(2020, 11, 4)
        //};
        #endregion
        public delegate void VoidDelegate();
        public static event VoidDelegate OneDayChanged;
        /// <summary>
        /// Дата
        /// </summary>
        public DateTime Date
        {
            get;
            private set;
        }
        /// <summary>
        /// Выходной
        /// </summary>
        public bool IsHolyday;
        /// <summary>
        /// Отработан
        /// </summary>
        public bool IsWorked;
        /// <summary>
        /// Текущий месяц
        /// </summary>
        public bool IsCurrentMonth;


        public CalendarDay(DateTime date, int month)
        {
            Date = date;
            IsCurrentMonth = date.Month == month;

            //IsHolyday = date.GetDayOfWeek() > 4;
            //if (!IsHolyday)
            //    IsHolyday = Holidays.Contains(date.Date);
            //if (IsCurrentMonth && !IsHolyday)
            //    IsWorked = true;
        }
        public View GetView(Context C, ViewGroup ParentLayout)
        {
            View Day = LayoutInflater.From(C).Inflate(Resource.Layout.item_day, ParentLayout, false);
            Day.FindViewById<TextView>(Resource.Id.Tittle).Text = Date.Day.ToString();
            if (IsCurrentMonth)
            {
                if (Date.Date == DateTime.Now.Date)
                    Day.SetBackgroundDrawable(C.GetDrawable(Resource.Drawable.TodayBorder));

                Day.FindViewById<TextView>(Resource.Id.Tittle).SetTextColor(IsHolyday ? Color.Red : Color.White);
                Day.FindViewById(Resource.Id.IsWorked).Visibility = IsWorked ? ViewStates.Visible : ViewStates.Gone;

                Day.Click += (s, e) =>
                {
                    Day.FindViewById(Resource.Id.IsWorked).Visibility =
                      (IsWorked = !IsWorked) ? ViewStates.Visible : ViewStates.Gone;
                    OneDayChanged?.Invoke();
                };
                Day.LongClick += (s, e) =>
                {
                    Day.FindViewById<TextView>(Resource.Id.Tittle).SetTextColor(
                    (IsHolyday = !IsHolyday) ? Color.Red : Color.White);
                    OneDayChanged?.Invoke();
                };
            }
            else
            {
                Day.FindViewById<TextView>(Resource.Id.Tittle).SetBackgroundColor(Color.Argb(0x11, 0xFF, 0xFF, 0xFF));
                Day.FindViewById<TextView>(Resource.Id.Tittle).SetTextColor(Color.Gray);
                Day.FindViewById(Resource.Id.IsWorked).Visibility = ViewStates.Gone;
            }
            return Day;
        }
    }
}