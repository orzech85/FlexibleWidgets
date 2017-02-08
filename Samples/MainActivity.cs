using Android.App;
using Android.Widget;
using Android.OS;

namespace Samples
{
    [Activity(Label = "Samples", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        Button chartButton;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            chartButton = FindViewById<Button>(Resource.Id.chartButton);
            chartButton.Click += (s,e) => { StartActivity(typeof(ChartActivity)); };
        }
    }
}

