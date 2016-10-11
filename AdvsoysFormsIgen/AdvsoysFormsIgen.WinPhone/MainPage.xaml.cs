using System;
using System.IO;
using Microsoft.Phone.Controls;
using Toasts.Forms.Plugin.WindowsPhone;
using Xamarin.Forms;

namespace AdvsoysFormsIgen.WinPhone
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;

            Forms.Init();
            ToastNotificatorImplementation.Init();
            LoadApplication(new AdvsoysFormsIgen.App());
        }
    }
}
