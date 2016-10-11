using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Caching;
using XLabs.Ioc;
using XLabs.Platform.Services.Geolocation;

namespace AdvsoysFormsIgen
{
    public partial class SagslistePage
    {
        private const string Key = "Sager";
        private readonly ISimpleCache cache;
        private readonly IGeolocator geolocator;
        private readonly ObservableCollection<Group<string, Sag>> sager;

        public SagslistePage()
        {
            cache = Resolver.Resolve<ISimpleCache>();
            geolocator = Resolver.Resolve<IGeolocator>();
            sager = new ObservableCollection<Group<string, Sag>>();
            InitializeComponent();
        }

        public event EventHandler<ItemTappedEventArgs> ItemTapped
        {
            add { SagerListView.ItemTapped += value; }
            remove { SagerListView.ItemTapped -= value; }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            SagerListView.ItemsSource = sager;

            var s = cache.Get<List<Sag>>(Key);
            var cached = sager.SingleOrDefault(g => g.Key == "Alle sager");
            sager.Remove(cached);
            sager.Add(new Group<string, Sag>("Alle sager", s));

            await Refresh();
        }

        private async Task Refresh()
        {
            if (geolocator == null)
            {
                return;
            }
            
            //var location = await geolocator.GetPositionAsync(30000);
            //var koordinat = new Koordinat(location.Latitude, location.Longitude);
            var koordinat = new Koordinat(55.708392, 9.526146);

            try
            {
                IsBusy = true;
                SearchBar.IsEnabled = false;

                var svar = await AdvosysKlient.HentSagslisteOgKoderAsync(koordinat);
                var close = sager.SingleOrDefault(g => g.Key == "I nærheden");
                sager.Remove(close);
                sager.Insert(0, new Group<string, Sag>("I nærheden", svar.Sager));
                FejlLabel.IsVisible = false;
            }
            catch
            {
            }
            finally
            {
                SearchBar.IsEnabled = true;
                IsBusy = false;
                SagerListView.IsRefreshing = false;
            }

            FiltrerSagsliste();
        }

        private void SearchBar_OnSearchButtonPressed(object sender, EventArgs e)
        {
            FiltrerSagsliste();
        }

        private void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            FiltrerSagsliste();
        }

        private void FiltrerSagsliste()
        {
            var tekst = SearchBar.Text;

            if (tekst == null)
            {
                tekst = string.Empty;
            }

            if (tekst == string.Empty)
            {
                SagerListView.ItemsSource = sager;
            }
            else
            {
                tekst = tekst.ToLower();
                SagerListView.ItemsSource = new[]
                {
                    new Group<string, Sag>(
                        "Søgeresultat",
                        sager.SelectMany(g => g)
                            .Where(s => s.Vedrørende.ToLower().Contains(tekst))
                            .ToList())
                };
            }
        }
    }
}