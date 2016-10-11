using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XLabs.Caching;
using XLabs.Ioc;

namespace AdvsoysFormsIgen
{
    public class App : Application
    {
        private readonly AchievementSystem achievementSystem;

        public App()
        {
            try
            {
                achievementSystem = new AchievementSystem();
                MainPage = new NavigationPage(new LoginPage());
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}