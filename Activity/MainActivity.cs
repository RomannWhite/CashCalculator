using CashCalculator.Classes.Extensions;
using System.Collections.Generic;
using Android.Support.V7.App;
using CashCalculator.Classes;
using Android.Widget;
using Android.Views;
using System.Linq;
using Android.App;
using Android.OS;
using System;

namespace CashCalculator.Activity
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        /// <summary>
        /// Ставка в месяц
        /// </summary>
        int Salary = 30000;
        List<CalendarDay> CalendarDays;
        TextView MonthName,
            YearName,
            ResultSalary,
            AllSalary;
        Button MinusButton,
            PlusButton;
        LinearLayout[] Weeks;
        DateTime SelectedDate = DateTime.Now;
        /// <summary>
        /// Абсцисса начала касания
        /// Надо бы перетащить в отдельную вьюху
        /// </summary>
        float LastX = 0;
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
                if (e.Event.Action == MotionEventActions.Down)
                    LastX = e.Event.GetX();
                if (e.Event.Action == MotionEventActions.Up || e.Event.Action == MotionEventActions.Cancel)
                {
                    float Delta = LastX - e.Event.GetX();
                    if (Math.Abs(Delta) >= 100)
                    {
                        SelectedDate = SelectedDate.AddMonths(Delta > 0 ? +1 : -1);
                        RefrashView();
                        RefrashCash();
                    }
                }
            };

            MinusButton = FindViewById<Button>(Resource.Id.MinusButton);
            PlusButton = FindViewById<Button>(Resource.Id.PlusButton);
            MinusButton.Click += (s, e) =>
            {
                if(Salary >= 1000)
                    Salary -= 1000;
                AllSalary.Text = Salary.ToString();
                RefrashCash();
            };
            PlusButton.Click += (s, e) =>
            {
                if (Salary < 100000)
                    Salary += 1000;
                AllSalary.Text = Salary.ToString();
                RefrashCash();
            };
            AllSalary = FindViewById<TextView>(Resource.Id.AllSalary);
            ResultSalary = FindViewById<TextView>(Resource.Id.ResultSalary);
            AllSalary.TextChanged += (s, e) => RefrashCash();
            MonthName = FindViewById<TextView>(Resource.Id.MonthName);
            YearName = FindViewById<TextView>(Resource.Id.YearName);
            RefrashView();
            CalendarDay.OneDayChanged += RefrashCash;
            RefrashCash();
        }
        void RefrashCash()
        {
            int OverWorkCff = 2;
            //Рабочих дней
            int WorkDays = CalendarDays.Where(c => c.IsCurrentMonth).Where(c => !c.IsDayOff).Count();
            //ЗП за день
            int CashPerDay = (int)((float)Salary / (float)WorkDays);
            CashPerDay -= CashPerDay % 50;
            //Отработано дней
            int WorkedDays = CalendarDays.Where(c => c.IsCurrentMonth).Where(c => c.IsWorked).Count();
            //Переработка
            int OverWorkedDays = Math.Max(WorkedDays - WorkDays, 0);
            //Итого
            int ResultCash = CashPerDay * (WorkedDays - OverWorkedDays) + CashPerDay * OverWorkedDays * OverWorkCff;
            ResultSalary.Text = ResultCash.ToString() + " = " + CashPerDay.ToString() + " * " + (WorkedDays - OverWorkedDays).ToString();
            if (OverWorkedDays > 0)
                ResultSalary.Text += " + " +  CashPerDay.ToString() + " * " + OverWorkedDays.ToString() + " * " + OverWorkCff.ToString();
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
    }
}