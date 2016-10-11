using System;
using System.IO;
using Foundation;
using SQLite.Net;
using SQLite.Net.Platform.XamarinIOS;
using Toasts.Forms.Plugin.iOS;
using UIKit;
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

namespace AdvsoysFormsIgen.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : XFormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication uiApp, NSDictionary options)
        {
            Forms.Init();
            ToastNotificatorImplementation.Init();
            var myApp = new App();
            LoadApplication(myApp);

            const string sqliteFilename = "Advosys.db3";
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var libraryPath = Path.Combine(documentsPath, "..", "Library");
            var path = Path.Combine(libraryPath, sqliteFilename);

            var app = new XFormsAppiOS();
            app.Init(this);

            var container = new SimpleContainer();
            container.Register<IGeolocator, Geolocator>();
            container.Register(t => AppleDevice.CurrentDevice)
                .Register(t => t.Resolve<IDevice>().Display)
                .Register<IByteSerializer, JsonSerializer>()
                .Register<IDependencyContainer>(t => container)
                .Register<IXFormsApp>(app);
            container.Register<ISimpleCache>(resolver => new SQLiteSimpleCache(
                new SQLitePlatformIOS(),
                new SQLiteConnectionString(path, true),
                resolver.Resolve<IByteSerializer>()));
            Resolver.SetResolver(container.GetResolver());

            return base.FinishedLaunching(uiApp, options);
        }
    }
}