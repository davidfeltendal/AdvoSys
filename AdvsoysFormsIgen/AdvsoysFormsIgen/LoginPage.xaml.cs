using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XLabs.Caching;
using XLabs.Ioc;

namespace AdvsoysFormsIgen
{
    public partial class LoginPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var cache = Resolver.Resolve<ISimpleCache>();

            if (cache != null)
            {
                var achievements = cache.Get<List<Achievement>>(Achievement.Key);

                if (achievements == null)
                {
                    achievements = new List<Achievement>
                    {
                        new Achievement(1, "one.png", "Foretag din første tidsregistrering."),
                        new Achievement(5, "five.png", "Foretag 5 tidsregistreringer."),
                        new Achievement(10, "ten.png", "Foretag 10 tidsregistreringer."),
                        new Achievement(25, "twentyfive.png", "Foretag 25 tidsregistreringer."),
                        new Achievement(50, "fifty.png", "Foretag 50 tidsregistreringer."),
                        new Achievement(101, "streak.png", "Foretag en tidsregistrering hver dag i én uge."),
                        new Achievement(102, "Star.png", "Foretag en tidsregistrering den 24. december."),
                        new Achievement(103, "Star.png", "Foretag 10 tidsregistrering på samme dag.")
                    };
                    cache.Set(Achievement.Key, achievements);
                }

                var tidsreg = cache.Get<Tidsreg>(Tidsreg.Key);

                if (tidsreg == null)
                {
                    tidsreg = new Tidsreg();
                    cache.Set(Tidsreg.Key, tidsreg);
                }

                var tidsregCache = cache.Get<List<TidsregistreringCache>>(TidsregistreringCache.Key);

                if (tidsregCache == null)
                {
                    tidsregCache = new List<TidsregistreringCache>();
                    cache.Set(TidsregistreringCache.Key, tidsregCache);
                }
            }

            if (Device.OS == TargetPlatform.WinPhone)
            {
                var logo = ImageSource.FromFile(Device.OnPlatform("logo-512.png", "logo_512.png", "logo-512.png"));
                Logo.Source = logo;   
            }
        }

        private void MenuItem_OnClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new HubPage());
        }
    }
}