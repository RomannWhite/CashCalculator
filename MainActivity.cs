using Android.Support.V7.App;
using Android.Graphics;
using Android.Content;
using Android.Widget;
using Android.Views;
using Android.App;
using Android.OS;
using System;
using System.Linq;
using System.Collections.Generic;

namespace CashCalculator
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        DateTime GetStartDate()
        {
            DateTime StartDate = DateTime.Now;
            StartDate = StartDate.AddDays(-StartDate.Day);
            return StartDate.AddDays(-(int)StartDate.DayOfWeek + 1);
        }
        int GetDayOfWeek(DateTime Date) => ((int)Date.AddDays(0).DayOfWeek + 6) % 7;
        View GetItemView(int LayoutID, ViewGroup ParentLayout) => LayoutInflater.From(this).Inflate(LayoutID, ParentLayout, false);
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
        }
        protected override void OnStart()
        {
            base.OnStart();

            LinearLayout[] Weeks = new LinearLayout[]
            {
                FindViewById<LinearLayout>(Resource.Id.Week1),
                FindViewById<LinearLayout>(Resource.Id.Week2),
                FindViewById<LinearLayout>(Resource.Id.Week3),
                FindViewById<LinearLayout>(Resource.Id.Week4),
                FindViewById<LinearLayout>(Resource.Id.Week5),
                FindViewById<LinearLayout>(Resource.Id.Week6)
            };

            EditText AllSalary = FindViewById<EditText>(Resource.Id.AllSalary);
            TextView ResultSalary = FindViewById<TextView>(Resource.Id.ResultSalary);
            AllSalary.TextChanged += (s, e) =>
            {
                int WorkDays = CalendarDays.Where(d => !d.IsDayOff).Count();
                int WorkedDays = CalendarDays.Where(d => !d.IsDayOff && d.IsWorked).Count() + 2 * CalendarDays.Where(d => d.IsDayOff && d.IsWorked).Count();
                ResultSalary.Text = (float.Parse(AllSalary.Text) / (float)WorkDays * WorkedDays).ToString(".00");
            };

            DateTime Date = GetStartDate();
            for (int w = 0; w < 6; w++)
            {
                Weeks[w].RemoveAllViews();
                for (int d = 0; d < 7; d++)
                {
                    CalendarDay NewDay = new CalendarDay(Date, DateTime.Now.Month);
                    CalendarDays.Add(NewDay);
                    Weeks[w].AddView(NewDay.GetView(this, Weeks[w]));
                    Date = Date.AddDays(1);
                }
            }
        }
        List<CalendarDay> CalendarDays = new List<CalendarDay>();
    }

    class CalendarDay : Java.Lang.Object
    {
        DateTime[] Holidays = new DateTime[]
        {
            new DateTime(2019, 1, 1),
            new DateTime(2019, 1, 2),
            new DateTime(2019, 1, 3),
            new DateTime(2019, 1, 4),
            new DateTime(2019, 1, 7),
            new DateTime(2019, 1, 8),

            new DateTime(2019, 3, 8),

            new DateTime(2019, 5, 1),
            new DateTime(2019, 5, 2),
            new DateTime(2019, 5, 3),
            new DateTime(2019, 5, 9),
            new DateTime(2019, 5, 10),

            new DateTime(2019, 6, 12),

            new DateTime(2019, 11, 4),

            new DateTime(2020, 1, 1),
            new DateTime(2020, 1, 2),
            new DateTime(2020, 1, 3),
            new DateTime(2020, 1, 6),
            new DateTime(2020, 1, 7),
            new DateTime(2020, 1, 8),

            new DateTime(2020, 2, 24),

            new DateTime(2020, 3, 9),

            new DateTime(2020, 5, 1),
            new DateTime(2020, 5, 4),
            new DateTime(2020, 5, 5),
            new DateTime(2020, 5, 11),

            new DateTime(2020, 6, 12),

            new DateTime(2020, 11, 4)
        };
        DateTime Date;
        public bool IsDayOff;
        public bool IsWorked;
        public bool IsCurrentMonth;
        public CalendarDay(DateTime date, int month)
        {
            Date = date;
            IsCurrentMonth = date.Month == month;
            IsDayOff = date.GetDayOfWeek() > 4;
            if (!IsDayOff)
                IsDayOff = Holidays.Contains(date.Date);
        }

        public View GetView(Context C, ViewGroup ParentLayout)
        {
            View Day = LayoutInflater.From(C).Inflate(Resource.Layout.item_day, ParentLayout, false);
            Day.FindViewById<TextView>(Resource.Id.Tittle).Text = Date.Day.ToString();
            if (IsDayOff)
                Day.FindViewById(Resource.Id.IsHoliday).Visibility = ViewStates.Visible;
            if (!IsCurrentMonth)
            {
                Day.SetBackgroundColor(Color.Argb(0x44, 0x00, 0x00, 0x00));
                Day.FindViewById(Resource.Id.IsWorked).Visibility = ViewStates.Gone;
            }
            else
            {
                Day.Click += (s, e) =>
                {
                    IsWorked = !IsWorked;
                    if (IsWorked)
                        Day.FindViewById(Resource.Id.IsWorked).Visibility = ViewStates.Visible;
                    else
                        Day.FindViewById(Resource.Id.IsWorked).Visibility = ViewStates.Gone;
                };
                Day.LongClick += (s, e) =>
                {
                    IsDayOff = !IsDayOff;
                };
            }
            return Day;
        }
    }

    static class Extensions
    {
        public static int GetDayOfWeek(this DateTime Date) => ((int)Date.AddDays(0).DayOfWeek + 6) % 7;
    }
}