using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using Xamarin.Forms;
using XLabs.Caching;
using XLabs.Ioc;

namespace AdvsoysFormsIgen
{
    public partial class Achievments
    {
        public Achievments()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var cache = Resolver.Resolve<ISimpleCache>();
            var achievements = cache.Get<List<Achievement>>(Achievement.Key);

            if (achievements == null || achievements.Count == 0)
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
                cache.Add(Achievement.Key, achievements);
            }

            AchievementsListView.ItemsSource = new ObservableCollection<Achievement>(achievements);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            AchievementsListView.ItemsSource = null;
        }

        private void AchievementsListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                AchievementsListView.SelectedItem = null;
            }
        }
    }
}