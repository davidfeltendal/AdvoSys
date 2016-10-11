using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace AdvsoysFormsIgen.Droid
{
    [Activity(Label = "Advosys", Icon = "@drawable/icon", MainLauncher = true,  NoHistory = true,
        Theme = "@style/Theme.Splash",
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            string cultureName = "da-DK";
            var locale = new Java.Util.Locale(cultureName);
            Java.Util.Locale.Default = locale;

            var config = new Android.Content.Res.Configuration { Locale = locale };
            BaseContext.Resources.UpdateConfiguration(config, BaseContext.Resources.DisplayMetrics); 

            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }
    }
}