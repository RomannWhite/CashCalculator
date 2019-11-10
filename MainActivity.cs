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
        TextView MonthName,
            YearName,
            ResultSalary;
        EditText AllSalary;
        LinearLayout[] Weeks;
        DateTime SelectedDate = DateTime.Now;
        float LastX = 0;
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

            Weeks = new LinearLayout[]
            {
                FindViewById<LinearLayout>(Resource.Id.Week1),
                FindViewById<LinearLayout>(Resource.Id.Week2),
                FindViewById<LinearLayout>(Resource.Id.Week3),
                FindViewById<LinearLayout>(Resource.Id.Week4),
                FindViewById<LinearLayout>(Resource.Id.Week5),
                FindViewById<LinearLayout>(Resource.Id.Week6)
            };
            FrameLayout SelectDateFrame = FindViewById<FrameLayout>(Resource.Id.SelectDateFrame);
            SelectDateFrame.Touch += (s, e) =>
            {
                if(e.Event.Action == MotionEventActions.Down)
                {
                    LastX = e.Event.GetX();
                    return;
                }
                if(e.Event.Action == MotionEventActions.Up || e.Event.Action == MotionEventActions.Cancel)
                {
                    float Delta = LastX - e.Event.GetX();
                    if(Math.Abs(Delta) >= 100)
                    {
                        SelectedDate = SelectedDate.AddMonths(Delta > 0 ? +1 : -1);
                        RefrashView();
                    }
                }
            };
            AllSalary = FindViewById<EditText>(Resource.Id.AllSalary);
            ResultSalary = FindViewById<TextView>(Resource.Id.ResultSalary);
            AllSalary.TextChanged += (s, e) =>
            {
                int WorkDays = CalendarDays.Where(d => d.IsCurrentMonth && !d.IsDayOff).Count();
                int WorkedDays = CalendarDays.Where(d => d.IsCurrentMonth && !d.IsDayOff && d.IsWorked).Count() + 2 * CalendarDays.Where(d => d.IsCurrentMonth && d.IsDayOff && d.IsWorked).Count();
                if (float.TryParse(AllSalary.Text, out float Salary))
                    ResultSalary.Text = (Salary / (float)WorkDays * WorkedDays).ToString("0");
            };
            MonthName = FindViewById<TextView>(Resource.Id.MonthName);
            YearName = FindViewById<TextView>(Resource.Id.YearName);
            RefrashView();
        }

        void RefrashView()
        {
            MonthName.Text = SelectedDate.GetMonthNameRus();
            YearName.Text = SelectedDate.ToString("yyyy");
            DateTime Date = SelectedDate.GetStartDate();
            CalendarDays = new List<CalendarDay>();
            for (int w = 0; w < 6; w++)
            {
                Weeks[w].RemoveAllViews();
                for (int d = 0; d < 7; d++)
                {
                    CalendarDay NewDay = new CalendarDay(Date, SelectedDate.Month);
                    CalendarDays.Add(NewDay);
                    Weeks[w].AddView(NewDay.GetView(this, Weeks[w]));
                    Date = Date.AddDays(1);
                }
            }
        }

        List<CalendarDay> CalendarDays;
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

            if (IsCurrentMonth && !IsDayOff)
                IsWorked = true;
        }

        public View GetView(Context C, ViewGroup ParentLayout)
        {
            View Day = LayoutInflater.From(C).Inflate(Resource.Layout.item_day, ParentLayout, false);
            Day.FindViewById<TextView>(Resource.Id.Tittle).Text = Date.Day.ToString();
            if(IsCurrentMonth)
            {
                Day.FindViewById<TextView>(Resource.Id.Tittle).SetTextColor(IsDayOff ? Color.Red : Color.White);
                Day.FindViewById(Resource.Id.IsWorked).Visibility = IsWorked ? ViewStates.Visible : ViewStates.Gone;

                Day.Click += (s, e) => Day.FindViewById(Resource.Id.IsWorked).Visibility =
                    (IsWorked = !IsWorked) ? ViewStates.Visible : ViewStates.Gone;
                Day.LongClick += (s, e) => Day.FindViewById<TextView>(Resource.Id.Tittle).SetTextColor(
                    (IsDayOff = !IsDayOff) ? Color.Red : Color.White);
            }
            else
            {
                Day.FindViewById<TextView>(Resource.Id.Tittle).SetTextColor(Color.Gray);
                Day.FindViewById(Resource.Id.IsWorked).Visibility = ViewStates.Gone;
            }
            return Day;
        }
    }

    static class Extensions
    {
        public static int GetDayOfWeek(this DateTime Date) => ((int)Date.AddDays(0).DayOfWeek + 6) % 7;

        static string[] Months = new string[]
            {
                "Декабрь",
                "Январь", "Февраль", "Март",
                "Апрель", "Май", "Июнь",
                "Июль", "Август", "Сентябрь",
                "Октябрь", "Ноябрь", "Декабрь"
            };
        public static string GetMonthNameRus(this DateTime Date) => Months[Date.Month];

        public static DateTime GetStartDate(this DateTime Date)
        {
            Date = Date.AddDays(-Date.Day);
            return Date.AddDays(-(int)Date.DayOfWeek + 1);
        }
    }
}