using System.IO;
using Android.App;
using Android.Content.PM;
using Android.OS;
using SQLite.Net;
using SQLite.Net.Platform.XamarinAndroid;
using Toasts.Forms.Plugin.Droid;
using Xamarin.Forms;
using XLabs.Caching;
using XLabs.Caching.SQLite;
using XLabs.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Mvvm;
using XLabs.Platform.Services.Geolocation;
using XLabs.Serialization;
using XLabs.Serialization.JsonNET;
using Environment = System.Environment;

namespace AdvsoysFormsIgen.Droid
{
    [Activity(Label = "Advosys", Icon = "@drawable/icon", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : XFormsApplicationDroid
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            string cultureName = "da-DK";
            var locale = new Java.Util.Locale(cultureName);
            Java.Util.Locale.Default = locale;

            var config = new Android.Content.Res.Configuration { Locale = locale };
            BaseContext.Resources.UpdateConfiguration(config, BaseContext.Resources.DisplayMetrics); 

            if (!Resolver.IsSet)
            {
                this.SetIoc();
            }
            else
            {
                var app = Resolver.Resolve<IXFormsApp>() as IXFormsApp<XFormsApplicationDroid>;
                app.AppContext = this;
            }

            Forms.Init(this, bundle);
            ToastNotificatorImplementation.Init();
            LoadApplication(new App());
        }

        private void SetIoc()
        {
            var app = new XFormsAppDroid();
            app.Init(this);

            const string sqliteFilename = "Advosys.db3";
            var documentsPath = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, sqliteFilename);

            var container = new SimpleContainer();
            container.Register(t => t.Resolve<IDevice>().Display)
                .Register<IDependencyContainer>(t => container)
                .Register<IXFormsApp>(app)
                .Register<IByteSerializer, JsonSerializer>();
            container.Register<IGeolocator, Geolocator>();
            container.Register<ISimpleCache>(resolver => new SQLiteSimpleCache(
                new SQLitePlatformAndroid(),
                new SQLiteConnectionString(path, true),
                resolver.Resolve<IByteSerializer>()));
            Resolver.SetResolver(container.GetResolver());
        }
    }
}

