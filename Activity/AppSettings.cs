using Android.Widget;
using Android.App;
using Android.OS;

namespace CashCalculator.Activity
{
    [Activity(
        Label = "@string/app_name",
        Theme = "@style/AppTheme.NoActionBar",
        //LaunchMode = Android.Content.PM.LaunchMode.SingleTask,
        ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class AppSettings : CustomActivity
    {
        EditText AllSalary,
            OverWorkCoefficient_Text;
        Switch SaveSettings;
        protected override void OnCreate(Bundle B)
        {
            base.OnCreate(B);
            SetContentView(Resource.Layout.activity_settings);
            if (FindViewById<ImageButton>(Resource.Id.BackButton) != null)
                FindViewById<ImageButton>(Resource.Id.BackButton).Click += (s, e) => OnBackPressed();
            AllSalary = FindViewById<EditText>(Resource.Id.Salary);
            AllSalary.Text = Salary.ToString();
            AllSalary.TextChanged += (s, e) =>
            {
                if (int.TryParse(AllSalary.Text, out int NewSalary))
                    Salary = NewSalary;
            };
            OverWorkCoefficient_Text = FindViewById<EditText>(Resource.Id.OverWorkCoefficient);
            OverWorkCoefficient_Text.Text = OverWorkCoefficient.ToString();
            OverWorkCoefficient_Text.TextChanged += (s, e) =>
            {
                if (int.TryParse(OverWorkCoefficient_Text.Text, out int NewOverWorkCoefficient))
                    OverWorkCoefficient = NewOverWorkCoefficient;
            };
            SaveSettings = FindViewById<Switch>(Resource.Id.SaveSettings);
            SaveSettings.Checked = SaveChanges;
            SaveSettings.CheckedChange += (s, e) => SaveChanges = e.IsChecked;
        }
        public override void OnBackPressed() => StartNewActivity(typeof(MainActivity));
    }
}