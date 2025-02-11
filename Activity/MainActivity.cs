﻿using CashCalculator.Classes.Extensions;
using Android.Views.InputMethods;
using System.Collections.Generic;
using CashCalculator.Classes;
using Android.Widget;
using Android.Views;
using System.Linq;
using Android.App;
using Android.OS;
using System;

namespace CashCalculator.Activity
{
    [Activity(
        Label = "@string/app_name",
        Theme = "@style/AppTheme.NoActionBar",
        //LaunchMode = Android.Content.PM.LaunchMode.SingleTask,
        ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait,
        MainLauncher = true)]
    public class MainActivity : CustomActivity
    {
        /// <summary>
        /// Ставка в месяц
        /// </summary>
        void SetSalary(int value)
        {
            Salary = value;
            if (AllSalary != null)
                RunOnUiThread(() =>
                {
                    AllSalary.Text = value.ToString();
                    AllSalary.SetSelection(AllSalary.Text.Length);
                });
        }
        CalendarMonth SelectedMonth;
        TextView MonthName,
            YearName,
            ResultSalary;
        EditText AllSalary;
        View AllSalaryMask;
        ImageButton MinusButton,
            PlusButton,
            Settings;
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
            AllSalary = FindViewById<EditText>(Resource.Id.AllSalary);
            AllSalary.Text = Salary.ToString();
            MinusButton = FindViewById<ImageButton>(Resource.Id.MinusButton);
            PlusButton = FindViewById<ImageButton>(Resource.Id.PlusButton);
            Settings = FindViewById<ImageButton>(Resource.Id.Settings);
            Settings.Click += (s, e) => StartNewActivity(typeof(AppSettings));
            AllSalaryMask = FindViewById(Resource.Id.AllSalaryMask);
            ResultSalary = FindViewById<TextView>(Resource.Id.ResultSalary);
            MonthName = FindViewById<TextView>(Resource.Id.MonthName);
            YearName = FindViewById<TextView>(Resource.Id.YearName);

            SelectDateFrame.Touch += (s, e) =>
            {
                if (e.Event.Action == MotionEventActions.Down)
                    LastX = e.Event.GetX();
                if (e.Event.Action == MotionEventActions.Up || e.Event.Action == MotionEventActions.Cancel)
                {
                    float Delta = LastX - e.Event.GetX();
                    if (Math.Abs(Delta) >= 100)
                    {
                        SaveWorkDays(SelectedDate, SelectedMonth.GetWorkedDays());
                        SaveHolidays(SelectedDate, SelectedMonth.GetHolydays());
                        SelectedDate = SelectedDate.AddMonths(Delta > 0 ? +1 : -1);
                        RefrashView();
                        RefrashCash();
                    }
                }
            };
            MinusButton.Click += (s, e) =>
            {
                if (Salary >= 1000)
                    SetSalary(Salary - 1000);
                RefrashCash();
            };
            PlusButton.Click += (s, e) =>
            {
                if (Salary < 100000)
                    SetSalary(Salary + 1000);
                RefrashCash();
            };
            AllSalary.TextChanged += (s, e) =>
            {
                if (AllSalary.Text.Length > 8)
                {
                    AllSalary.Text = AllSalary.Text.Substring(0, 8);
                    AllSalary.SetSelection(AllSalary.Text.Length);
                }
                RefrashCash();
            };
            AllSalary.EditorAction += (s, e) =>
            {
                if (e.ActionId == ImeAction.Done)
                {
                    AllSalaryMask.Visibility = ViewStates.Visible;
                    AllSalary.Enabled = false;
                    RefrashCash();
                }
            };
            AllSalaryMask.LongClick += (s, e) =>
            {
                AllSalaryMask.Visibility = ViewStates.Gone;
                AllSalary.Enabled = true;
                AllSalary.SetSelection(AllSalary.Text.Length);
                ShowKBandFocus(AllSalary);
            };
            CalendarDay.OneDayChanged += RefrashCash;
            RefrashView();
            RefrashCash();
        }
        public override void OnBackPressed() => Finish();
        void ShowKBandFocus(View V)
        {
            V.RequestFocus();
            InputMethodManager inputMethodManager = GetSystemService(InputMethodService) as InputMethodManager;
            inputMethodManager.ShowSoftInput(V, ShowFlags.Forced);
            inputMethodManager.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.ImplicitOnly);
        }
        void RefrashCash()
        {
            //Рабочих дней
            int WorkDays = SelectedMonth.DaysList.Where(c => c.IsCurrentMonth).Where(c => !c.IsHolyday).Count();
            //ЗП за день
            int CashPerDay = (int)((float)Salary / (float)WorkDays);
            CashPerDay -= CashPerDay % 50;
            //Отработано дней
            int WorkedDays = SelectedMonth.DaysList.Where(c => c.IsCurrentMonth).Where(c => c.IsWorked).Count();
            //Переработка
            int OverWorkedDays = Math.Max(WorkedDays - WorkDays, 0);
            //Итого
            int ResultCash = CashPerDay * (WorkedDays - OverWorkedDays) + CashPerDay * OverWorkedDays * OverWorkCoefficient;
            ResultSalary.Text = ResultCash.ToString() + " = " + CashPerDay.ToString() + " * " + (WorkedDays - OverWorkedDays).ToString();
            if (OverWorkedDays > 0)
                ResultSalary.Text += " + " + CashPerDay.ToString() + " * " + OverWorkedDays.ToString() + " * " + OverWorkCoefficient.ToString();
        }
        void RefrashView()
        {
            MonthName.Text = SelectedDate.GetMonthNameRus();
            YearName.Text = SelectedDate.ToString("yyyy");
            SelectedMonth = new CalendarMonth(SelectedDate, GetWorkDays(SelectedDate), GetHolidays(SelectedDate));
            for (int w = 0; w < 6; w++)
            {
                Weeks[w].RemoveAllViews();
                for (int d = 0; d < 7; d++)
                    Weeks[w].AddView(SelectedMonth.DaysList[w * 7 + d].GetView(this, Weeks[w]));
            }
        }
    }
}