using Android.Support.V7.App;
using Android.Content;
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
        const string APPPREFERENCE_SAVESETTINGS= "savesettings";
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
        protected static bool SaveChanges
        {
            get => GetSavedValue(APPPREFERENCE_SAVESETTINGS, false);
            set => SetSavedValue(APPPREFERENCE_SAVESETTINGS, value);
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