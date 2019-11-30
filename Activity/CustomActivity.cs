using CashCalculator.Classes.Extensions;
using System.Collections.Generic;
using Android.Support.V7.App;
using Android.Content;
using System.Linq;
using Android.App;
using Android.OS;
using System;

namespace CashCalculator.Activity
{
    /// <summary>
    /// Base activity class for my app
    /// </summary>
    [Activity]
    public class CustomActivity : AppCompatActivity
    {
        const string APPPREFERENCE = "cashcalculatorpreferences";
        const string APPPREFERENCE_SALARY = "salary";
        const string APPPREFERENCE_OVERWORK = "overwork";
        const string APPPREFERENCE_SAVESETTINGS = "savesettings";
        const string APPPREFERENCE_SAVEDWORKDAYS_ = "savedworkdays_";
        const string APPPREFERENCE_SAVEDHOLYDAYS_ = "savedholydays_";
        static int GetSavedValue(string Key, int Default)
        {
            var prefs = Application.Context.GetSharedPreferences(APPPREFERENCE, FileCreationMode.Private);
            return prefs.GetInt(Key, Default);
        }
        static bool GetSavedValue(string Key, bool Default)
        {
            var prefs = Application.Context.GetSharedPreferences(APPPREFERENCE, FileCreationMode.Private);
            return prefs.GetBoolean(Key, Default);
        }
        static string GetSavedValue(string Key, string Default)
        {
            var prefs = Application.Context.GetSharedPreferences(APPPREFERENCE, FileCreationMode.Private);
            return prefs.GetString(Key, Default);
        }
        static void SetSavedValue(string Key, int Value)
        {
            var prefs = Application.Context.GetSharedPreferences(APPPREFERENCE, FileCreationMode.Private);
            var prefEditor = prefs.Edit();
            prefEditor.PutInt(Key, Value);
            prefEditor.Commit();
        }
        static void SetSavedValue(string Key, bool Value)
        {
            var prefs = Application.Context.GetSharedPreferences(APPPREFERENCE, FileCreationMode.Private);
            var prefEditor = prefs.Edit();
            prefEditor.PutBoolean(Key, Value);
            prefEditor.Commit();
        }
        static void SetSavedValue(string Key, string Value)
        {
            var prefs = Application.Context.GetSharedPreferences(APPPREFERENCE, FileCreationMode.Private);
            var prefEditor = prefs.Edit();
            prefEditor.PutString(Key, Value);
            prefEditor.Commit();
        }
        static int salary = 30000;
        /// <summary>
        /// Зарплата за месяц
        /// </summary>
        protected static int Salary
        {
            get
            {
                if (!SaveChanges)
                    return salary;
                return GetSavedValue(APPPREFERENCE_SALARY, salary);
            }
            set
            {
                if (value >= 1 && value <= 100000)
                {
                    salary = value;
                    if (SaveChanges)
                        SetSavedValue(APPPREFERENCE_SALARY, salary);
                }
            }
        }
        static int overworkcoefficient = 2;
        /// <summary>
        /// Стоимость переработки
        /// </summary>
        protected static int OverWorkCoefficient
        {
            get
            {
                if (!SaveChanges)
                    return overworkcoefficient;
                return GetSavedValue(APPPREFERENCE_OVERWORK, overworkcoefficient);
            }
            set
            {
                if(value >= 1)
                {
                    overworkcoefficient = value;
                    if (SaveChanges)
                        SetSavedValue(APPPREFERENCE_OVERWORK, overworkcoefficient = overworkcoefficient);
                }
            }
        }
        /// <summary>
        /// Сохранять изменения
        /// </summary>
        static bool? savechanges;
        protected static bool SaveChanges
        {
            get
            {
                if(savechanges == null)
                    savechanges = GetSavedValue(APPPREFERENCE_SAVESETTINGS, false);
                return savechanges.Value;
            }
            set
            {
                savechanges = value;
                SetSavedValue(APPPREFERENCE_SAVESETTINGS, value);
            }
        }
        protected void SaveWorkDays(DateTime Date, int[] Days)
        {
            if (SaveChanges)
                SetSavedValue(APPPREFERENCE_SAVEDWORKDAYS_ + Date.MMYY(), string.Join(" ", Days.Select(i => i.ToString()).ToArray()));
        }
        protected int[] GetWorkDays(DateTime Date)
        {
            string RawString = GetSavedValue(APPPREFERENCE_SAVEDWORKDAYS_ + Date.MMYY(), "");
            if(RawString != "")
                return RawString.Split(" ").Select(s => int.Parse(s)).ToArray();
            return new int[0];
        }
        protected void SaveHolidays(DateTime Date, int[] Days)
        {
            if (SaveChanges)
                SetSavedValue(APPPREFERENCE_SAVEDHOLYDAYS_ + Date.MMYY(), string.Join(" ", Days.Select(d => d.ToString()).ToArray()));
        }
        protected int[] GetHolidays(DateTime Date)
        {
            string RawString = GetSavedValue(APPPREFERENCE_SAVEDHOLYDAYS_ + Date.MMYY(), "");
            if (RawString != "")
                return RawString.Split(" ").Select(s => int.Parse(s)).ToArray();
            return new int[0];
        }
        protected void StartNewActivity(Type T, Bundle B = null)
        {
            Intent I = new Intent(this, T);
            if (B != null)
                I.PutExtras(B);
            StartActivity(I);
        }
    }
}